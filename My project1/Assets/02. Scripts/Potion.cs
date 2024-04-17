using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : Item
{
    public enum PotionType
    {
        Dumplings = 0,
        Tee = 1,
        Congee = 2,
        Dango = 3,
        Dumpling =4
    }

    public PotionType potionType;

    public DDOL priceUI;
    public Text priceTxt;

    public int price;

    bool isContact;

    private void Start()
    {
        priceTxt.text = price.ToString();
    }

    public override void UseItem()
    {
        switch (potionType)
        {
            case PotionType.Dumplings:
                playerCtrl.maxHp += 35.0f;
                break;
            case PotionType.Tee:

                break;
            case PotionType.Congee:

                break;
            case PotionType.Dango:

                break;
            case PotionType.Dumpling:
                playerCtrl.curHp += playerCtrl.maxHp * 0.3f;
                if (playerCtrl.curHp > playerCtrl.maxHp)
                    playerCtrl.curHp = playerCtrl.maxHp;
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
