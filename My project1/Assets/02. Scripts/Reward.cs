using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    public List<GameObject> books;


    void Start()
    {
        
    }

    public void RandomReward(int num)
    {
        List<GameObject> list = new List<GameObject>(books);

        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            GameObject temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
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
