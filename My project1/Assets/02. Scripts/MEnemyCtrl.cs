using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MEnemyCtrl : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        IDLE = 0,
        TRACE = 1,
        ATTACK = 2,
        HIT = 3,
        DIE = 4
    }

    public ENEMYSTATE enemyState;

    PlayerCtrl playerCtrl;
    SpawnEnemy spawnEnemy;

    public Slider hpBar;

    public float traceDist = 20.0f;
    public float attackDist = 2.0f;
    public float attackDelay = 5.0f;
    public float dmg = 5.0f;
    public bool isDie = false;
    public bool isAttack = true;
    public bool isHit = false;

    public float maxHp = 80.0f;
    public float curHp = 0;

    Transform enemyTr;
    Transform playerTr;

    NavMeshAgent agent;
    Animator anim;

    readonly int hashTrace = Animator.StringToHash("IsTrace");
    readonly int hashAttack = Animator.StringToHash("IsAttack");
    readonly int hashHit = Animator.StringToHash("Hit");
    readonly int hashDie = Animator.StringToHash("Die");

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        playerCtrl = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawnEnemy = GameObject.FindGameObjectWithTag("RandomSpawnGroup")?.GetComponent<SpawnEnemy>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(CheckEnemyState());
        StartCoroutine(EnemyAction());

        curHp = maxHp;
    }




    void Update()
    {

    }

    IEnumerator CheckEnemyState()
    {
        while (!isDie)
        {
            //yield return new WaitForSeconds(1.0f);

            if (enemyState == ENEMYSTATE.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, enemyTr.position);


            if (distance <= attackDist && isAttack)
            {
                enemyState = ENEMYSTATE.ATTACK;
                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);
                yield return new WaitForSeconds(curClipInfos[0].clip.length);
            }
            else if (distance <= traceDist && enemyState != ENEMYSTATE.ATTACK)
            {
                enemyState = ENEMYSTATE.TRACE;
                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);
                yield return new WaitForSeconds(curClipInfos[0].clip.length);
            }
            else
            {
                enemyState = ENEMYSTATE.IDLE;
                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);
                yield return new WaitForSeconds(curClipInfos[0].clip.length);
            }
        }
    }

    IEnumerator EnemyAction()
    {
        while (!isDie)
        {
            switch (enemyState)
            {
                case ENEMYSTATE.IDLE:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    anim.SetBool(hashTrace, false);
                    break;
                case ENEMYSTATE.TRACE:
                    agent.SetDestination(playerTr.position);
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    agent.isStopped = false;
                    break;
                case ENEMYSTATE.ATTACK:
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    transform.LookAt(playerTr.position);
                    anim.SetBool(hashAttack, true);
                    isAttack = false;
                    yield return new WaitForSeconds(5.0f);
                    isAttack = true;
                    //if (!isAttack)
                    //    enemyState = ENEMYSTATE.IDLE;
                    break;
                case ENEMYSTATE.HIT:
                    anim.SetTrigger(hashHit);
                    curHp -= playerCtrl.dmg;
                    agent.isStopped = false;
                    break;
                case ENEMYSTATE.DIE:
                    anim.SetTrigger(hashDie);
                    StopCoroutine(CheckEnemyState());
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    Destroy(gameObject, 5.0f);
                    spawnEnemy.curMonsterCnt--;
                    spawnEnemy.CheckEnemy();
                    GetComponent<CapsuleCollider>().enabled = false;
                    isDie = true;
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyState == ENEMYSTATE.TRACE)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        if (enemyState == ENEMYSTATE.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackRange") && curHp > 0)
        {
            enemyState = ENEMYSTATE.HIT;
            hpBar.value = curHp / maxHp;
        }

        if (other.CompareTag("AttackRange") && curHp <= 0)
        {
            enemyState = ENEMYSTATE.DIE;
            hpBar.value = 0;
        }
    }
}
