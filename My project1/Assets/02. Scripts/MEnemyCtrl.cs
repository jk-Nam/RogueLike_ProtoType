using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MEnemyCtrl : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        IDLE = 0,
        TRACE = 1,
        ATTACK = 2,
        DIE = 3
    }

    public ENEMYSTATE enemyState;

    public float traceDist = 20.0f;
    public float attackDist = 2.0f;
    public float attackDelay = 1.2f;
    public float dmg = 5.0f;
    public bool isDie = false;
    public bool isAttack = true;

    float dwTime = 0;
    int hp = 100;

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
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        StartCoroutine(CheckEnemyState());
        StartCoroutine(EnemyAction());
    }




    void Update()
    {        

    }

    IEnumerator CheckEnemyState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

            if (enemyState == ENEMYSTATE.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, enemyTr.position);

            if (distance <= attackDist && isAttack)
            {
                enemyState = ENEMYSTATE.ATTACK;
            }
            else if (distance <= traceDist)
            {
                enemyState = ENEMYSTATE.TRACE;
            }
            else
            {
                enemyState = ENEMYSTATE.IDLE;
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
                    anim.SetBool(hashTrace, false);
                    break;
                case ENEMYSTATE.TRACE:
                    agent.SetDestination(playerTr.position);
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    agent.isStopped = false;
                    break;
                case ENEMYSTATE.ATTACK:
                    anim.SetBool(hashAttack, true);
                    isAttack = false;
                    yield return new WaitForSeconds(attackDelay);
                    isAttack = true;
                    enemyState = ENEMYSTATE.IDLE;
                    break;
                case ENEMYSTATE.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
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

}
