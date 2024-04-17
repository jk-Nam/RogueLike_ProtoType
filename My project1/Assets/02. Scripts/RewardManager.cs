using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public List<GameObject> books;
    public Image[] rewardImage;
    public Text[] rewardName;
    public Text[] rewardDes;

    public List<GameObject> selectedReward;

    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    private void Start()
    {
        //RandomReward(3);
    }


    public void RandomReward(int num)
    {
        selectedReward.Clear();

        List<GameObject> list = new List<GameObject>(books);

        for (int i = 0; i < num; i++)
        {
            int rnd = Random.Range(i, list.Count);
            GameObject temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
            if (list[i].GetComponent<Book>() != null)
            {
                rewardImage[i].sprite = list[i].GetComponent<Book>().itemSprite;
                rewardName[i].text = list[i].GetComponent<Book>().itemName;
                rewardDes[i].text = list[i].GetComponent<Book>().itemDes;
            }
            else if (list[i].GetComponent<Potion>() != null)
            {
                rewardImage[i].sprite = list[i].GetComponent<Potion>().itemSprite;
                rewardName[i].text = list[i].GetComponent<Potion>().itemName;
                rewardDes[i].text = list[i].GetComponent<Potion>().itemDes;
            }
            else if (list[i].GetComponent<Goods>() != null)
            {
                rewardImage[i].sprite = list[i].GetComponent<Goods>().itemSprite;
                rewardName[i].text = list[i].GetComponent<Goods>().itemName;
                rewardDes[i].text = list[i].GetComponent<Goods>().itemDes;
            }
            //rewardImage[i].sprite = list[i].GetComponent<Book>().itemSprite;
            //rewardName[i].text = list[i].GetComponent<Book>().itemName;
            //rewardDes[i].text = list[i].GetComponent<Book>().itemDes;
            selectedReward.Add(list[i]);
        }

        //for (int i = 0; i < num; i++)
        //{
        //    if (i < list.Count)
        //    {
        //        Debug.Log(list[i].name);
        //    }
        //}
    }

    public void selectReward(int num)
    {
        Debug.Log(num + "번을 선택했습니다.");
        if (selectedReward[num].GetComponent<Book>() != null)
        {
            //selectedReward[num].GetComponent<Book>().UseItem();
            Debug.Log(num + "번째 아이템 효과가 적용되었습니다.");
        }
        else if (selectedReward[num].GetComponent<Potion>() != null)
        {
            //selectedReward[num].GetComponent<Potion>().UseItem();
            Debug.Log(num + "번째 아이템 효과가 적용되었습니다.");
        }
        else if (selectedReward[num].GetComponent<Goods>() != null)
        {
            //selectedReward[num].GetComponent<Goods>().UseItem();
            Debug.Log(num + "번째 아이템 효과가 적용되었습니다.");
        }
        UIManager.Instance.rewardUI.SetActive(false);
        UIManager.Instance.selectBooks.Add(selectedReward[num]);
    }

}
