using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject qSwitch;

    private void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        if (qSwitch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                UIManager.Instance.quest.SetActive(true);
            }            
        }

        if (UIManager.Instance.quest.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.quest.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            qSwitch.SetActive(true);
            UIManager.Instance.pressEQ.SetActive(true);
            UIManager.Instance.playerInfo.SetActive(false);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            qSwitch.SetActive(false);
            UIManager.Instance.pressEQ.SetActive(false);
            UIManager.Instance.playerInfo.SetActive(true);
        }            
    }
}
