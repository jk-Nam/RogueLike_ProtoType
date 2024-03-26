using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    book = 0,
    potion = 1,
    coin = 2
}

public abstract class Item : MonoBehaviour
{
    public ItemType type;

    protected PlayerCtrl playerCtrl;

    public string itemName;
    public string itemDes;

    protected void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCtrl>();
    }

    public abstract void UseItem();
  
}
