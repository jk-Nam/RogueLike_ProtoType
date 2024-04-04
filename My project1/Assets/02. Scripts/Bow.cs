using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour, IWeapon
{
    PlayerCtrl playerCtrl;
    public Animator anim;

    public float dmg = 20.0f;
    public float attackSpeed = 4.0f;
    public float range = 8.0f;
    public int maxCombo = 1;
    public int upgrade = 0;
    public int needSteel = 1;

    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public IEnumerator Attack()
    {
        yield return null;
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
