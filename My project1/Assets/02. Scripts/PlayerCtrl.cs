using System.Collections;
using System.Collections.Generic;
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
    public IWeapon weapon;

    public WeaponManager weaponMgr;

    public GameObject curWeapon;
    public GameObject dashEffect;
    public GameObject attackRange;
    public GameObject attackEffect;    

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
    public float attackDelay = 0.0f;
    public float sAttackDelay = 0.0f;
    public float sAttackCoolTime = 2.0f;
    public float skillDelay = 0.0f;
    public float skillCoolTime = 2.0f;
    public float skillRecevery = 3.0f;
    public float curHitCnt = 0.0f;
    public int maxDashCnt = 2;
    public int curDashCnt = 0;
    public int maxSkillCnt = 1;
    public int curSkillCnt = 0;
    public int maxLifeCnt = 1; 
    public int curLifeCnt = 0;
    public int currentAttack = 0;

    bool isDash = false;
    bool isHit = false;
    bool isDie = false;

    float dwTime = 0;

    Vector3 forward;
    Vector3 right;

    private void Awake()
    {
        weaponMgr = GameObject.Find("WeaponMgr")?.GetComponent<WeaponManager>();
        weapon = GetComponentInChildren<IWeapon>();
    }


    void Start()
    {
        //curWeapon = weaponMgr.myWeapon;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward;
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        curHp = maxHp;
        curLifeCnt = maxLifeCnt;
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
                UIManager.Instance.UpdateSkill();
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
                UIManager.Instance.UpdateHpBar();

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

        if (Input.GetMouseButtonDown(0) && !isHit && attackDelay >= 1.0f)
        {
            playerState = PLAYERSTATE.ATTACK;
            LookMousePointer();
            StartCoroutine(weapon.Attack());          
            //StartCoroutine(EffectOnOff());
        }
            
        if (Input.GetMouseButtonDown(1) && !isHit)
        {
            playerState = PLAYERSTATE.SATTACK;
            LookMousePointer();
            StartCoroutine(weapon.SAttack());
        }

        if (Input.GetKeyDown(KeyCode.Q) && !isHit && curSkillCnt > 0)
        {
            playerState = PLAYERSTATE.SKILL;
            LookMousePointer();
            curSkillCnt--;
            StartCoroutine(weapon.Skill());
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

    public void LookMousePointer()
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


    //IEnumerator SAttack()
    //{
    //    if (sAttackDelay >= sAttackCoolTime)
    //    {
    //        Debug.Log("SAttack!!!");
    //        anim.SetTrigger("SAttack");
    //        sAttackDelay = 0.0f;
    //        yield return new WaitForSeconds(1.0f);
    //        playerState = PLAYERSTATE.IDLE;
    //    }
    //}

    //IEnumerator Skill()
    //{
    //    if (skillDelay >= skillCoolTime)
    //    {
    //        curSkillCnt--;
    //        UIManager.Instance.UpdateSkill();
    //        Debug.Log("Fire Ball!!!");
    //        anim.SetTrigger("Skill");
    //        yield return new WaitForSeconds(1.0f);
    //        playerState = PLAYERSTATE.IDLE;
    //    }        
    //}

    IEnumerator EffectOnOff()
    {
        attackEffect.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        attackEffect.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyWeapon") && curHp >= 0)
        {
            if (other.GetComponent<MEnemyCtrl>().enemyState == MEnemyCtrl.ENEMYSTATE.ATTACK)
            {
                playerState = PLAYERSTATE.HIT;
                Debug.Log("Hit!!!");
                OnDamage(other.GetComponent<MEnemyCtrl>().dmg);
            }            
        }

        if (other.CompareTag("BossWeapon") && curHp >= 0)
        {
            if (other.GetComponentInParent<BossCtrl>().bossState == BossCtrl.BOSSSTATE.ATTACK)
            {
                playerState = PLAYERSTATE.HIT;
                Debug.Log("Hit!!!");
                OnDamage(other.GetComponent<BossCtrl>().dmg);
            }
        }

        if (other.CompareTag("BossSkillEffect"))
        {
            playerState = PLAYERSTATE.HIT;
            Debug.Log("Skill Hit");
            OnDamage(other.GetComponent<Boss1Skill2Projectile>().dmg);
        }

        if ((other.CompareTag("EnemyWeapon") || other.CompareTag("BossWeapon")) && curHp <= 0 && curLifeCnt > 0)
        {
            Debug.Log("hp가 0보다 낮아졌습니다.");
            Revive();
            UIManager.Instance.UpdateLife();
        }

        if (other.CompareTag("EnemyWeapon") && curHp <= 0 && curLifeCnt <= 0)
        {
            playerState = PLAYERSTATE.DEATH;
            isDie = true;
            PlayerDie();
        }
    }

    void OnDamage(float _dmg)
    {
        curHp -= _dmg;
        
    }

    void Revive()
    {
        curLifeCnt--;
        curHp = maxHp;
        Debug.Log("Revive!!!");
    }

    IEnumerator PlayerDie()
    {
        isDie = false;
        yield return new WaitForSeconds(5.0f);
        Debug.Log("Player Die... 마을로 돌아갑니다.");
    }

    public void HitCheckOn()
    {
        attackRange.SetActive(true);
    }

    public void HitCheckOff()
    {
        attackRange.SetActive(false);
    }

}
