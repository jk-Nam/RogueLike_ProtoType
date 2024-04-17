using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerCtrl;

public class Sword : MonoBehaviour, IWeapon
{
    PlayerCtrl playerCtrl;
    public Animator anim;
    public AudioSource[] sound;
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
        sound = GetComponents<AudioSource>();
    }

    public IEnumerator Attack()
    {
        playerCtrl.currentAttack++;
        
        if (playerCtrl.currentAttack > maxCombo)
        {
            playerCtrl.currentAttack = 1;
        }

        if (playerCtrl.attackDelay > 1.5f)
        {
            playerCtrl.currentAttack = 1;
        }

        anim.SetTrigger("Attack" + playerCtrl.currentAttack);
        sound[0].Play();
        playerCtrl.attackDelay = 0.0f;
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        Debug.Log("Attack" + playerCtrl.currentAttack);

        //yield return new WaitForSeconds(clipInfo[0].clip.length / 2);
        yield return new WaitForSeconds(1.0f);
        //if (playerCtrl.attackDelay >= 1.0f)
        //{
        //    playerCtrl.playerState = PLAYERSTATE.IDLE;
        //}
        playerCtrl.playerState = PLAYERSTATE.IDLE;
    }

    public IEnumerator SAttack()
    {
        if (playerCtrl.sAttackDelay >= playerCtrl.sAttackCoolTime)
        {
            Debug.Log("SAttack!!!");
            anim.SetTrigger("SAttack");
            playerCtrl.sAttackDelay = 0.0f;
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            yield return new WaitForSeconds(clipInfo[0].clip.length / 2);
            playerCtrl.playerState = PLAYERSTATE.IDLE;
        }
    }

    public IEnumerator Skill()
    {
        if (playerCtrl.skillDelay >= playerCtrl.skillCoolTime)
        {
            UIManager.Instance.UpdateSkill();
            anim.SetTrigger("Skill");
            AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
            yield return new WaitForSeconds(1.5f);
            playerCtrl.SKillEffect.SetActive(true);
            sound[1].Play();
            yield return new WaitForSeconds(clipInfo[0].clip.length - 1.5f);
            playerCtrl.SKillEffect.SetActive(false);
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
