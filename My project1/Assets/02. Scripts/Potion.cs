using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public enum PotionType
    {
        dumpling = 0,
        liquor = 1,
    }

    public PotionType potionType;

    bool isContact;

    public override void UseItem()
    {
        switch (potionType)
        {
            case PotionType.dumpling:
                playerCtrl.curHp += playerCtrl.maxHp * 0.3f;
                if (playerCtrl.curHp > playerCtrl.maxHp)
                    playerCtrl.curHp = playerCtrl.maxHp;
                break;
            case PotionType.liquor:

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = false;
        }
    }
}
