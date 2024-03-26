using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goods : Item
{
    public enum GoodsType
    {
        Force = 1,
        Coin = 2,
        Steel = 3
    }

    public GoodsType goodsType;


    public override void UseItem()
    {
        switch (goodsType)
        {
            case GoodsType.Force:
                GameManager.Instance.playerForce += 10;
                break;
            case GoodsType.Coin:
                int CoinNum;
                int rnum = Random.Range(10, 26);
                CoinNum = rnum;
                GameManager.Instance.playerCoin += CoinNum;
                break;
            case GoodsType.Steel:
                GameManager.Instance.playerSteel++;
                break;
            default:
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            Debug.Log(this.name);
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("E키가 눌렸습니다.");
                Destroy(gameObject);
                UseItem();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.name);
            if (Input.GetKeyUp(KeyCode.E))
            {
                Debug.Log("E키가 눌렸습니다.");
                Destroy(gameObject);
                UseItem();
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log(this.name);
            Destroy(gameObject);
            UseItem();
        }
    }
}
