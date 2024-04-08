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
    public int curUpgrade = 0;
    public int needSteel = 1;

    private void Awake()
    {
        playerCtrl = GetComponentInParent<PlayerCtrl>();
        anim = GetComponentInParent<Animator>();
    }

    public IEnumerator Attack()
    {
        playerCtrl.currentAttack++;
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);

        if (playerCtrl.currentAttack > maxCombo)
        {
            playerCtrl.currentAttack = 1;
        }

        if (playerCtrl.attackDelay > 1.0f)
        {
            playerCtrl.currentAttack = 1;
        }

        anim.SetTrigger("Attack" + playerCtrl.currentAttack);
        Debug.Log("Attack" + playerCtrl.currentAttack);

        playerCtrl.attackDelay = 0.0f;
        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(clipInfo[0].clip.length);
        playerCtrl.playerState = PLAYERSTATE.IDLE;
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
            yield return new WaitForSeconds(2.0f);
            playerCtrl.playerState = PLAYERSTATE.IDLE;
        }
    }

    public void Upgrade()
    {
        curUpgrade++;
        GameManager.Instance.playerSteel -= needSteel;
        needSteel++;

    }
}
