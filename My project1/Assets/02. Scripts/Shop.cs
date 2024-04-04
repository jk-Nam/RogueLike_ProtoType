using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> items;
    public List<GameObject> selectedItems;
    public GameObject[] sales;

    private void Start()
    {
        RandomItem(3);
    }



    public void RandomItem(int num)
    {
        selectedItems = new List<GameObject>();

        List<GameObject> list = new List<GameObject>(items);

        for (int i = 0; i < num; i++)
        {
            int rnd = Random.Range(i, list.Count);
            GameObject temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;

            selectedItems.Add(list[i]);
            GameObject saleItem = Instantiate(list[i], sales[i].transform.position, Quaternion.identity, sales[i].transform);
            saleItem.transform.position += new Vector3(0, 0.45f, 0);
            saleItem.transform.rotation = Quaternion.Euler(45.0f, 0, -45.0f);
            saleItem.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
   
}
