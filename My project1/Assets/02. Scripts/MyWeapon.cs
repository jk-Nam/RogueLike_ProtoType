using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyWeapon : MonoBehaviour
{
    IWeapon weapon;


    public void ChangeWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    public void Attack()
    { 
        weapon.Attack(); 
    }

    public void SAttack()
    {
        weapon.SAttack();
    }

    public void Skill()
    {
        weapon.Skill();
    }
}
