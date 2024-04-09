using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss2Ctrl : MonoBehaviour
{
    public enum BOSSSTATE
    {
        IDLE = 0,
        Trace = 1,
        ATTACK = 2,
        SKILL1 = 3,
        SKILL2 = 4,
        DIE = 5
    }

    public BOSSSTATE bossState;

    PlayerCtrl playerCtrl;
    Transform bossTr;
    Transform playerTr;
    Animator bossAnim;
    NavMeshAgent agent;

    public GameObject reward;
    public GameObject skill2Effect;
    public Transform skill1Pos;
    public Transform skill2Pos;

    public float hp = 1200.0f;
    public float rotSpeed = 500.0f;
    public float attackDist = 3.0f;
    public float attackCoolTime = 3.0f;
    public float skillDist1 = 2.0f;
    public float skillDist2 = 6.0f;
    public float skill1CoolTime = 8.0f;
    public float skill2CoolTime = 15.0f;
    public float dmg = 10.0f;
    public float skill1Dmg = 13.0f;
    public float skill2Dmg = 15.0f;
    public float moveSpeed;

    public float dwtime = 0;
    public float dwtime1 = 0;
    public float dwtime2 = 0;

    Vector3 vel = Vector3.zero;

    [SerializeField] bool isDie = false;
    [SerializeField] bool isAttack = false;
    [SerializeField] bool isSkill1 = false;
    [SerializeField] bool isSkill2 = false;


    void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        bossAnim = GetComponent<Animator>();
        bossTr = GetComponent<Transform>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        StartCoroutine(CheckBossState());
        StartCoroutine(BossAction());
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckSkillCoolTime();

        if (bossState == BOSSSTATE.SKILL2)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTr.position, Time.deltaTime * moveSpeed);
        }
    }

    IEnumerator CheckBossState()
    {
        AnimatorClipInfo[] curClipInfos;
        curClipInfos = bossAnim.GetCurrentAnimatorClipInfo(0);

        while (!isDie)
        {
            yield return new WaitForSeconds(curClipInfos[0].clip.length);
            if (bossState == BOSSSTATE.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, bossTr.position);

            if (distance >= skillDist2)
            {
                bossState = BOSSSTATE.Trace;
            }

            if (distance <= skillDist2 && isSkill2)
            {
                bossState = BOSSSTATE.SKILL2;
            }
            else if (distance >= skillDist1 && isSkill1)
            {
                bossState = BOSSSTATE.SKILL1;
            }
            else if (distance <= attackDist && isAttack)
            {
                bossState = BOSSSTATE.ATTACK;
            }
            else
            {
                bossState = BOSSSTATE.IDLE;
            }
        }
    }

    IEnumerator BossAction()
    {
        while (!isDie)
        {
            switch (bossState)
            {
                case BOSSSTATE.IDLE:
                    transform.LookAt(playerTr.position);
                    break;
                case BOSSSTATE.Trace:
                    agent.SetDestination(playerTr.position);
                    break;
                case BOSSSTATE.ATTACK:
                    if (isAttack)
                    {
                        AnimatorClipInfo[] curClipInfos;
                        curClipInfos = bossAnim.GetCurrentAnimatorClipInfo(0);

                        transform.LookAt(playerTr.position);
                        bossAnim.SetTrigger("Attack");
                        isAttack = false;
                        yield return new WaitForSeconds(curClipInfos[0].clip.length);
                        bossState = BOSSSTATE.IDLE;
                    }
                    else
                        bossState = BOSSSTATE.IDLE;
                    break;
                case BOSSSTATE.SKILL1:
                    if (isSkill1)
                    {
                        AnimatorClipInfo[] curClipInfos;
                        curClipInfos = bossAnim.GetCurrentAnimatorClipInfo(0);
                        transform.LookAt(playerTr.position);
                        bossAnim.SetTrigger("Skill1");
                        yield return new WaitForSeconds(curClipInfos[0].clip.length);
                        isSkill1 = false;
                        bossState = BOSSSTATE.IDLE;
                    }
                    else
                        bossState = BOSSSTATE.IDLE;
                    break;
                case BOSSSTATE.SKILL2:
                    if (isSkill2)
                    {
                        transform.LookAt(playerTr.position);
                        bossAnim.SetTrigger("Skill2");
                        //agent.SetDestination(playerTr.position);
                        //GameObject effect = Instantiate(skill2Effect, skill2Pos.position, Quaternion.identity);
                        yield return new WaitForSeconds(5.0f);
                        isSkill2 = false;
                        bossAnim.SetTrigger("Skill2End");
                        bossState = BOSSSTATE.IDLE;
                    }
                    else
                        bossState = BOSSSTATE.IDLE;
                    break;
                case BOSSSTATE.DIE:
                    isDie = true;
                    bossAnim.SetTrigger("Die");
                    GetComponent<CapsuleCollider>().enabled = false;
                    yield return new WaitForSeconds(5.0f);
                    Destroy(gameObject);
                    Instantiate(reward, transform.position, Quaternion.Euler(180.0f, 0, 0));
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void CheckSkillCoolTime()
    {
        if (isDie)
            return;
        else
        {
            if (!isAttack)
            {
                dwtime += Time.deltaTime;
                if (dwtime >= attackCoolTime)
                {
                    isAttack = true;
                    dwtime = 0;
                }
            }

            if (!isSkill1)
            {
                dwtime1 += Time.deltaTime;
                if (dwtime1 >= skill1CoolTime)
                {
                    isSkill1 = true;
                    dwtime1 = 0;
                }
            }

            if (!isSkill2)
            {
                dwtime2 += Time.deltaTime;
                if (dwtime2 >= skill2CoolTime)
                {
                    isSkill2 = true;
                    dwtime2 = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackRange") && hp > 0)
        {
            hp -= playerCtrl.dmg;
        }

        if (other.CompareTag("AttackRange") && hp <= 0)
        {
            bossState = BOSSSTATE.DIE;
        }
    }




    private void OnDrawGizmos()
    {
        if (bossState == BOSSSTATE.SKILL1)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, skillDist1);
        }

        if (bossState == BOSSSTATE.SKILL2)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, skillDist2);
        }

        if (bossState == BOSSSTATE.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }
}
