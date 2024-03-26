using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public enum PLAYERSTATE
    {
        IDLE = 0,
        MOVE = 1,
        DASH = 2,
        ATTACK = 3,
        SATTACK = 4,
        SKILL = 5,
        HIT = 6,
        DEATH = 7
    }
    
    public PLAYERSTATE playerState;

    public GameObject dashEffect;
    public WeaponManager weaponMgr;

    public Animator anim;
    Rigidbody rb;

    public float moveSpeed = 5.0f;
    public float rotSpeed = 100.0f;
    public float dashPower = 10.0f;
    public float maxHp = 45.0f;
    public float curHp = 0;
    public float dmg = 10.0f;
    public float attackSpeed = 1.0f;
    public float range = 1.0f;
    public int maxDashCnt = 2;
    public int curDashCnt = 0;
    public int maxSkillCnt = 1;
    public int curSkillCnt = 0;
    public int maxReviveCnt = 1; 
    public int curReviveCnt = 0;


    float attackDelay = 0.0f;
    float sAttackDelay = 0.0f;
    float sAttackCoolTime = 2.0f;
    float skillDelay = 0.0f;
    float skillCoolTime = 2.0f;
    float skillRecevery = 3.0f;
    float curHitCnt = 0.0f;
    int currentAttack = 0;

    bool isDash = false;
    bool isHit = false;
    bool isDie = false;

    float dwTime = 0;

    Vector3 forward;
    Vector3 right;

    private void Awake()
    {
        weaponMgr = GameObject.Find("WeaponMgr")?.GetComponent<WeaponManager>();
    }


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward;
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        curHp = maxHp;
        curReviveCnt = maxReviveCnt;
        curSkillCnt = maxSkillCnt;
        isDie = false;
    }


    void Update()
    {
        //공격,특수공격  
        attackDelay += Time.deltaTime;
        sAttackDelay += Time.deltaTime;
        skillDelay += Time.deltaTime;

        //스킬 사용 횟수 재생
        if (curSkillCnt < maxSkillCnt)
        {
            
            dwTime += Time.deltaTime;
            if (dwTime >= skillRecevery)
            {
                curSkillCnt++;
                dwTime = 0;
                if (curSkillCnt > maxSkillCnt)
                    curSkillCnt = maxSkillCnt;
            }
        }

        switch (playerState)
        {
            case PLAYERSTATE.IDLE:
                anim.SetBool("IsMove", false);
                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && playerState != PLAYERSTATE.ATTACK && playerState != PLAYERSTATE.SATTACK && playerState != PLAYERSTATE.SKILL)
                    playerState = PLAYERSTATE.MOVE;
                if (Input.GetKeyDown(KeyCode.Space) && curDashCnt < maxDashCnt)
                {
                    playerState = PLAYERSTATE.DASH;
                }
                break;
            case PLAYERSTATE.MOVE:
                anim.SetBool("IsMove", true);
                Move();
                if (Input.GetKeyDown(KeyCode.Space) && curDashCnt < maxDashCnt)
                {
                    playerState = PLAYERSTATE.DASH;
                }
                break;
            case PLAYERSTATE.DASH:                
                isDash = true;
                StartCoroutine(Dash());

                break;
            case PLAYERSTATE.ATTACK:
                

                break;
            case PLAYERSTATE.SATTACK:

                break;
            case PLAYERSTATE.SKILL:

                break;
            case PLAYERSTATE.HIT:
                isHit = true;
                anim.SetTrigger("Hit");
                curHitCnt += Time.deltaTime;

                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);

                if (curHitCnt >= curClipInfos[0].clip.length)
                {
                    isHit = false;
                    curHitCnt = 0.0f;
                    playerState = PLAYERSTATE.IDLE;
                }
                break;
            case PLAYERSTATE.DEATH:
                anim.SetTrigger("Die");
                if (isDie)
                    StartCoroutine(PlayerDie());
                break;
            default:
                break;
        }

        if (Input.GetMouseButtonDown(0) && !isHit && attackDelay >= 0.3f)
        {
            playerState = PLAYERSTATE.ATTACK;
            StartCoroutine(Combo());
        }
            
        if (Input.GetMouseButtonDown(1) && !isHit)
        {
            playerState = PLAYERSTATE.SATTACK;
            StartCoroutine(SAttack());
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isHit)
        {
            playerState = PLAYERSTATE.SKILL;
            if (curSkillCnt > 0)
            {
                StartCoroutine(Skill());
            }
            
        }
            
        
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveDir = forward * v + right * h;
        moveDir.y = 0;

        Vector3 nextPos = moveDir.normalized * moveSpeed * Time.deltaTime;
        transform.position += nextPos;

        if (h == 0 && v == 0)
        {
            playerState = PLAYERSTATE.IDLE;
            return;
        }            
        else
        {
            Quaternion newRotation = Quaternion.LookRotation(nextPos);
            transform.rotation = Quaternion.Lerp(rb.rotation, newRotation, rotSpeed * Time.deltaTime);
            transform.rotation = newRotation;
            
        }
    }

    IEnumerator Dash()
    {
        curDashCnt++;
        dashEffect.SetActive(isDash);
        transform.position += transform.forward * dashPower;
        yield return new WaitForSeconds(0.2f);
        isDash = false;
        dashEffect.SetActive(isDash);
        playerState = PLAYERSTATE.MOVE;
        curDashCnt = 0;
    }


    IEnumerator Combo()
    {
        if (attackDelay > 0.25f)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;

                Vector3 direction = targetPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }

            currentAttack++;

            if (currentAttack > 3)
                currentAttack = 1;

            if (attackDelay > 0.5f)
                currentAttack = 1;

            //anim.SetTrigger("Attack1");
            anim.SetTrigger("Attack" + currentAttack);
            Debug.Log("Attack" + currentAttack);

            attackDelay = 0.0f;
            yield return new WaitForSeconds(0.3f);
            playerState = PLAYERSTATE.IDLE;
        }
    }

    IEnumerator SAttack()
    {
        if (sAttackDelay >= sAttackCoolTime )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;

                Vector3 direction = targetPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }
            Debug.Log("SAttack!!!");
            anim.SetTrigger("SAttack");
            sAttackDelay = 0.0f;
            yield return new WaitForSeconds(1.0f);
            playerState = PLAYERSTATE.IDLE;
        }
    }

    IEnumerator Skill()
    {
        if (skillDelay >= skillCoolTime)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y;

                Vector3 direction = targetPosition - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            }

            curSkillCnt--;
            Debug.Log("Fire Ball!!!");
            anim.SetTrigger("Skill");
            yield return new WaitForSeconds(1.0f);
            playerState = PLAYERSTATE.IDLE;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && curHp >= 0)
        {
            playerState = PLAYERSTATE.HIT;
            Debug.Log("Hit!!!");
            OnDamage();
        }

        if (other.CompareTag("EnemyWeapon") && curHp <= 0 && curReviveCnt > 0)
        {
            Debug.Log("hp가 0보다 낮아졌습니다.");
            Revive();
        }

        if (other.CompareTag("EnemyWeapon") && curHp <= 0 && curReviveCnt <= 0)
        {
            playerState = PLAYERSTATE.DEATH;
            isDie = true;
            PlayerDie();
        }

    }

    void OnDamage()
    {
        curHp -= 5;
        
    }

    void Revive()
    {
        curReviveCnt--;
        curHp = maxHp;
        Debug.Log("Revive!!!");
    }

    IEnumerator PlayerDie()
    {
        isDie = false;
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Player Die... 마을로 돌아갑니다.");

    }

}
