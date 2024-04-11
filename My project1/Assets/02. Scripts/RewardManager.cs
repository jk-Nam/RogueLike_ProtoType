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

            rewardImage[i].sprite = list[i].GetComponent<Book>().itemSprite;
            rewardName[i].text = list[i].GetComponent<Book>().itemName;
            rewardDes[i].text = list[i].GetComponent<Book>().itemDes;
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
        selectedReward[num].GetComponent<Book>().UseItem();
        Debug.Log(num + "번째 아이템 효과가 적용되었습니다.");
        UIManager.Instance.rewardUI.SetActive(false);
        UIManager.Instance.selectBooks.Add(selectedReward[num]);
    }

}
