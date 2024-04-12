using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    public Slider hpBar;

    public float minDistance = 5.0f;
    public float attackDistance = 10.0f;
    public float maxMoveDistance = 15.0f;

    public bool isDie = false;

    
    public float maxHp = 85;
    public float curHp = 0;

    Transform enemyTr;
    Transform playerTr;

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
        

        while (!isDie || enemyState != ENEMYSTATE.MOVE )
        {
            //yield return new WaitForSeconds(2.5f);
            //yield return new WaitForSeconds(curClipInfos[0].clip.length);

            if (enemyState == ENEMYSTATE.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(playerTr.position, enemyTr.position);            

            if (distance <= minDistance && enemyState != ENEMYSTATE.ATTACK)
            {
                enemyState = ENEMYSTATE.MOVE;
                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);
                yield return new WaitForSeconds(curClipInfos[0].clip.length);
            }
            else if (distance <= attackDistance && enemyState != ENEMYSTATE.MOVE)
            {
                enemyState = ENEMYSTATE.ATTACK;
                AnimatorClipInfo[] curClipInfos;
                curClipInfos = anim.GetCurrentAnimatorClipInfo(0);
                yield return new WaitForSeconds(curClipInfos[0].clip.length);
            }
            else
            {
                enemyState = ENEMYSTATE.IDLE;
                yield return new WaitForSeconds(1.0f);
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
                    anim.SetBool(hashMove, false);
                    anim.SetBool(hashAttack, false);
                    break;
                case ENEMYSTATE.MOVE:
                    Vector3 dir = (playerTr.position - enemyTr.position).normalized;
                    Vector3 movePos = enemyTr.position - dir * Random.Range(5.0f, maxMoveDistance);
                    agent.SetDestination(movePos);                    
                    agent.isStopped = false;
                    anim.SetBool(hashMove, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case ENEMYSTATE.ATTACK:
                    Vector3 direction = playerTr.position - transform.position;
                    direction.y = 0f;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = targetRotation;
                    anim.SetBool(hashAttack, true);
                    break;
                case ENEMYSTATE.HIT:
                    anim.SetTrigger(hashHit);
                    curHp -= playerCtrl.dmg;
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
            yield return new WaitForSeconds(2.0f);
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
            hpBar.value = curHp / maxHp;
        }

        if (other.CompareTag("AttackRange") && curHp <= 0)
        {
            enemyState = ENEMYSTATE.DIE;
            hpBar.value = 0;
        }
    }
}
