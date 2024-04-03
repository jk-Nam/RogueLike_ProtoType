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
        HIT = 3,
        DIE = 4
    }

    public ENEMYSTATE enemyState;

    PlayerCtrl playerCtrl;
    SpawnEnemy spawnEnemy;

    public float minDistance = 10.0f;
    public float attackDistance = 10.0f;
    public float maxMoveDistance = 15.0f;

    public bool isDie = false;

    
    public float maxHp = 85;
    public float curHp = 0;

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
        playerCtrl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCtrl>();
        enemyTr = GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player")?.GetComponent<Transform>();
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
            yield return new WaitForSeconds(3.0f);
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
                    Vector3 dir = (playerTr.position - enemyTr.position).normalized;
                    Vector3 movePos = enemyTr.position - dir * Random.Range(5.0f ,maxMoveDistance);
                    agent.SetDestination(movePos);
                    
                    anim.SetBool(hashMove, true);
                    anim.SetBool(hashAttack, false);
                    agent.isStopped = false;
                    break;
                case ENEMYSTATE.ATTACK:
                    Vector3 direction = playerTr.position - transform.position;
                    direction.y = 0f;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    targetRotation *= Quaternion.Euler(0, 90, 0);
                    transform.rotation = targetRotation;
                    anim.SetBool(hashAttack, true);
                    break;
                case ENEMYSTATE.HIT:
                    anim.SetTrigger(hashHit);
                    curHp -= playerCtrl.dmg;
                    agent.isStopped = false;
                    break;
                case ENEMYSTATE.DIE:
                    isDie = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    spawnEnemy.curMonsterCnt--;
                    spawnEnemy.CheckEnemy();
                    StopAllCoroutines();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AttackRange") && curHp > 0)
        {
            enemyState = ENEMYSTATE.HIT;
        }

        if (other.CompareTag("AttackRange") && curHp <= 0)
        {
            enemyState = ENEMYSTATE.DIE;
        }
    }
}
