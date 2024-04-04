using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerCtrl;

public class Sword : MonoBehaviour, IWeapon
{
    PlayerCtrl playerCtrl;
    public Animator anim;
    public PLAYERSTATE playerState;

    public float dmg = 10.0f;
    public float attackSpeed = 0.25f;
    public float range = 1.0f;
    public int maxCombo = 3;
    public int upgrade = 0;
    public int needSteel = 1;

    public AnimationClip attack1AnimationClip;
    public AnimationClip attack2AnimationClip;
    public AnimationClip attack3AnimationClip;

    private void Awake()
    {
        playerCtrl = GetComponentInParent<PlayerCtrl>();
        anim = GetComponentInParent<Animator>();
    }

    public IEnumerator Attack()
    {       
        if (playerCtrl.attackDelay > 0.25f)
        {            
            playerCtrl.currentAttack++;

            if (playerCtrl.currentAttack > maxCombo)
                playerCtrl.currentAttack = 1;

            if (playerCtrl.attackDelay > 0.5f)
                playerCtrl.currentAttack = 1;

            anim.SetTrigger("Attack" + playerCtrl.currentAttack);
            Debug.Log("Attack" + playerCtrl.currentAttack);

            playerCtrl.attackDelay = 0.0f;
            yield return new WaitForSeconds(0.3f);
            playerCtrl.playerState = PLAYERSTATE.IDLE;
        }
    }

    public IEnumerator SAttack()
    {
        if (playerCtrl.sAttackDelay >= playerCtrl.sAttackCoolTime)
        {
            Debug.Log("SAttack!!!");
            anim.SetTrigger("SAttack");
            playerCtrl.sAttackDelay = 0.0f;
            yield return new WaitForSeconds(1.0f);
            playerCtrl.playerState = PLAYERSTATE.IDLE;
        }
    }

    public IEnumerator Skill()
    {
        if (playerCtrl.skillDelay >= playerCtrl.skillCoolTime)
        {
            UIManager.Instance.UpdateSkill();
            //Debug.Log("Fire Ball!!!");
            anim.SetTrigger("Skill");
            yield return new WaitForSeconds(1.0f);
            playerCtrl.playerState = PLAYERSTATE.IDLE;
        }
    }

    public void Upgrade()
    {
            upgrade++;
            GameManager.Instance.playerSteel -= needSteel;
            needSteel++;          
    }
}
