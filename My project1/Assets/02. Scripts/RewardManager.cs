using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{

    public List<GameObject> books;
    public List<GameObject> rewardUI;
    public Sprite rewardImage;
    public Text rewardName;
    public Text rewardDes;
    public GameObject[] selectedRewards;



    public void RandomReward(int num)
    {
        List<GameObject> list = new List<GameObject>(books);

        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            GameObject temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
            rewardImage = list[i].GetComponent<Sprite>();
            rewardName.text = list[i].GetComponent<Book>().itemName;
            rewardDes.text = list[i].GetComponent<Book>().itemDes;
            
        }

        for (int i = 0; i < num; i++)
        {
            if (i < list.Count)
            {
                Debug.Log(list[i].name);
            }
        }
    }
}
