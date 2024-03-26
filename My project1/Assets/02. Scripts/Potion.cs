using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{


    public override void UseItem()
    {
        playerCtrl.curHp += playerCtrl.maxHp * 0.3f;
        if (playerCtrl.curHp > playerCtrl.maxHp )
            playerCtrl.curHp = playerCtrl.maxHp;
    }
}
