using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStand : MonoBehaviour
{
    public WeaponManager weaponMgr;


    private void Awake()
    {

    }

    void Update()
    {
        if (UIManager.Instance.weaponStandUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponMgr.ChangeSword();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weaponMgr.ChangeAxe();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                weaponMgr.ChangeBow();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.weaponStandUI.SetActive(true);
            UIManager.Instance.playerInfo.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.weaponStandUI.SetActive(false);
            UIManager.Instance.playerInfo.SetActive(true);
        }
            
    }
}
