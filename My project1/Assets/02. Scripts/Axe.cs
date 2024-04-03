using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour, IWeapon
{
    public float dmg = 15.0f;
    public float attackSpeed = 3.0f;
    public float range = 2.0f;
    public int maxCombo = 2;
    public int upgrade = 0;
    public int needSteel = 1;

    public void Attack()
    {
        
    }

    public void SAttack()
    {
        
    }

    public void Skill()
    {
        
    }

    public void Upgrade()
    {
            upgrade++;
            GameManager.Instance.playerSteel -= needSteel;
            needSteel++;          
    }
}
