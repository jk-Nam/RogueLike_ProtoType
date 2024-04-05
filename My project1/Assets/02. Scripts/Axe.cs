using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerCtrl;

public class Axe : MonoBehaviour, IWeapon
{
    PlayerCtrl playerCtrl;
    public Animator anim;

    public float dmg = 15.0f;
    public float attackSpeed = 3.0f;
    public float range = 2.0f;
    public int maxCombo = 2;
    public int upgrade = 0;
    public int needSteel = 1;

    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
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
        yield return new WaitForSeconds(clipInfo[0].clip.length);
        playerCtrl.playerState = PLAYERSTATE.IDLE;
    }

    public IEnumerator SAttack()
    {
        yield return null;
    }

    public IEnumerator Skill()
    {
        yield return null;
    }

    public void Upgrade()
    {
            upgrade++;
            GameManager.Instance.playerSteel -= needSteel;
            needSteel++;          
    }
}
