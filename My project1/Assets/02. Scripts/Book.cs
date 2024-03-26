using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Item
{
    

    public override void UseItem()
    {
        playerCtrl.dmg += 10.0f;
    }


}
