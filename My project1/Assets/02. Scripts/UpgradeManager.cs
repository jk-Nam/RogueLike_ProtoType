using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public GameObject uSwitch;

    public int needForce = 0;


    void Start()
    {

    }


    void Update()
    {
        if (uSwitch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.upgrade.SetActive(true);
            }
        }

        if (UIManager.Instance.upgrade.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.upgrade.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uSwitch.SetActive(true);
            UIManager.Instance.pressEU.SetActive(true);
            UIManager.Instance.playerInfo.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uSwitch.SetActive(false);
            UIManager.Instance.pressEU.SetActive(false);
            UIManager.Instance.playerInfo.SetActive(true);
        }
    }
}
