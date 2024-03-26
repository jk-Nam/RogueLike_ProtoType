using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    Sword = 0,
    Axe = 1,
    Bow = 2
}

public class WeaponManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public GameObject myWeapon;

    IWeapon weapon;

    public GameObject sword;
    public GameObject axe;
    public GameObject bow;

    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCtrl>();
    }

    void SetWeaponType(WeaponType weaponType)
    {
        Component c = gameObject.GetComponent<IWeapon>() as Component;

        if (c != null)
        {
            Destroy(c);
        }

        switch (weaponType)
        {
            case WeaponType.Sword:
                weapon = gameObject.AddComponent<Sword>();
                myWeapon = sword;
                playerCtrl.anim.SetInteger("Weapon", 0);
                playerCtrl.dmg =         sword.GetComponent<Sword>().dmg + 3 * sword.GetComponent<Sword>().upgrade;
                playerCtrl.attackSpeed = sword.GetComponent<Sword>().attackSpeed;
                playerCtrl.range =       sword.GetComponent<Sword>().range;
                break;
            case WeaponType.Axe:
                weapon = gameObject.AddComponent<Axe>();
                myWeapon = axe;
                playerCtrl.anim.SetInteger("Weapon", 1);
                playerCtrl.dmg =         axe.GetComponent<Axe>().dmg + 5 * axe.GetComponent<Axe>().upgrade;
                playerCtrl.attackSpeed = axe.GetComponent<Axe>().attackSpeed;
                playerCtrl.range =       axe.GetComponent<Axe>().range;
                break;
            case WeaponType.Bow:
                weapon = gameObject.AddComponent<Bow>();
                myWeapon = bow;
                playerCtrl.anim.SetInteger("Weapon", 2);
                playerCtrl.dmg =         bow.GetComponent<Bow>().dmg + 5 * bow.GetComponent<Bow>().upgrade;
                playerCtrl.attackSpeed = bow.GetComponent<Bow>().attackSpeed;
                playerCtrl.range =       bow.GetComponent<Bow>().range;
                break;
            default:
                weapon = gameObject.AddComponent<Sword>();
                myWeapon = sword;
                playerCtrl.anim.SetInteger("Weapon", 0);
                playerCtrl.dmg =         sword.GetComponent<Sword>().dmg;
                playerCtrl.attackSpeed = sword.GetComponent<Sword>().attackSpeed;
                playerCtrl.range =       sword.GetComponent<Sword>().range;
                break;
        }
    }


    void Start()
    {
        SetWeaponType(WeaponType.Sword);
    }

    void Update()
    {
        
    }

    public void ChangeSword()
    {
        SetWeaponType(WeaponType.Sword);
        Debug.Log("°Ë ÀåÂø");
    }

    public void ChangeAxe()
    {
        SetWeaponType(WeaponType.Axe);
        Debug.Log("µµ³¢ ÀåÂø");
    }

    public void ChangeBow()
    {
        SetWeaponType(WeaponType.Bow);
        Debug.Log("È° ÀåÂø");
    }

    public void Attack()
    {
        weapon.Attack();
        Debug.Log("Attack!!!");
    }

    public void SAttack()
    {
        weapon.SAttack();
        Debug.Log("SAttack!!!");
    }

    public void Skill()
    {
        weapon.Skill();
        Debug.Log("Skill!!!");
    }
}
