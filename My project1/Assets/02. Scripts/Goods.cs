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

    bool isContact = false;

    private void Update()
    {
        if (isContact && Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("E키가 눌렸습니다.");
            Destroy(gameObject);
            UseItem();
        }
    }

    public override void UseItem()
    {
        switch (goodsType)
        {
            case GoodsType.Force:
                GameManager.Instance.playerForce += 10;
                UIManager.Instance.UpdateForce();
                break;
            case GoodsType.Coin:
                int CoinNum;
                int rnum = Random.Range(10, 26);
                CoinNum = rnum;
                GameManager.Instance.playerCoin += CoinNum;
                UIManager.Instance.UpdateCoin();
                break;
            case GoodsType.Steel:
                GameManager.Instance.playerSteel++;
                UIManager.Instance.UpdateSteel();
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            isContact = true;
        }       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isContact = false;
        }

    }
}
