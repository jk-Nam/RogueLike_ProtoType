using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : Item
{
    public int num;


    private void Start()
    {
        int rnum = Random.Range(10, 26);
        num = rnum;
    }

    public override void UseItem()
    {
        GameManager.Instance.playerCoin += num;
    }

}
