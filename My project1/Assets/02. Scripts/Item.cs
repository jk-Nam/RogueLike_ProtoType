using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    book = 0,
    potion = 1,
    goods = 2
}

public abstract class Item : MonoBehaviour
{
    public ItemType type;

    protected PlayerCtrl playerCtrl;

    protected void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    public Sprite itemSprite;
    public string itemName;
    public string itemDes;


    public abstract void UseItem();
  
}
