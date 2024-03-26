using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class REnemyCtrl : MonoBehaviour
{
    public enum ENEMYSTATE
    {
        IDLE = 0,
        MOVE = 1,
        ATTACK = 2,
        DIE = 3
    }

    public ENEMYSTATE enemyState;

    public float minDistance = 10.0f;
    public float attackDistance = 15.0f;
    public float maxMoveDistance = 2.0f;

    public bool isDie = false;

    int hp = 85;

    Transform enemyTr;
    Transform playerTr;
    Vector3 targetTr;

    NavMeshAgent agent;
    Animator anim;

    readonly int hashMove = Animator.StringToHash("IsMove");
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

            
            if (distance <= minDistance)
            {
                enemyState = ENEMYSTATE.MOVE;
            }
            else if (distance <= attackDistance)
            {
                enemyState = ENEMYSTATE.ATTACK;
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
                    anim.SetBool(hashMove, false);
                    break;
                case ENEMYSTATE.MOVE:
                    //Vector3 dir = (playerTr.position - enemyTr.position).normalized;
                    
                    
                    //agent.SetDestination();
                    //anim.SetBool(hashMove, true);
                    //anim.SetBool(hashAttack, false);
                    //agent.isStopped = false;
                    //break;
                case ENEMYSTATE.ATTACK:
                    Quaternion newRotation = Quaternion.LookRotation(playerTr.position);
                    transform.rotation = newRotation;
                    anim.SetBool(hashAttack, true);
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
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnDrawGizmos()
    {
        if (enemyState == ENEMYSTATE.MOVE)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, minDistance);
        }

        if (enemyState == ENEMYSTATE.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }

}
