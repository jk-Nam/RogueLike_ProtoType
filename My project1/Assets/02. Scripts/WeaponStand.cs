using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStand : MonoBehaviour
{
    public WeaponManager weaponMgr;
    public Sword sword;
    public Axe axe;
    public Bow bow;

    public GameObject wUSwitch;
    public Text needSteel1;
    public Text needSteel2;
    public Text needSteel3;
    public Button swordUpgradeBtn;
    public Button axeUpgradeBtn;
    public Button bowUpgradeBtn;


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

        if (wUSwitch.activeSelf && Input.GetKeyUp(KeyCode.E))
        {
            UIManager.Instance.weaponUpgradeUI.SetActive(true);
            UIManager.Instance.weaponStandUI.SetActive(false);

        }

        if (UIManager.Instance.weaponUpgradeUI.activeSelf && Input.GetKeyUp(KeyCode.Escape))
        {
            UIManager.Instance.weaponUpgradeUI.SetActive(false);
            UIManager.Instance.weaponStandUI.SetActive(true);
        }
    }


    public void SwordUpgrade()
    {
        if (Convert.ToInt32(needSteel1) <= GameManager.Instance.playerSteel)
        {
            sword.Upgrade();
            
            needSteel1.text = sword.needSteel.ToString();
            if (sword.upgrade >= 5)
                //버튼 클릭 불가
                swordUpgradeBtn.interactable = false;
        }
    }

    public void AxeUpgrade()
    {
        if (Convert.ToInt32(needSteel2) <= GameManager.Instance.playerSteel)
        {
            axe.Upgrade();
            needSteel2.text = axe.needSteel.ToString();
            if (axe.upgrade >= 5)
                axeUpgradeBtn.interactable = false;
        }
    }

    public void BowUpgrade()
    {
        if (Convert.ToInt32(needSteel3) <= GameManager.Instance.playerSteel)
        {
            bow.Upgrade();
            needSteel3.text = bow.needSteel.ToString();
            if (bow.upgrade >= 5)
                bowUpgradeBtn.interactable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wUSwitch.SetActive(true);
            UIManager.Instance.weaponStandUI.SetActive(true);
            UIManager.Instance.playerInfo.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            wUSwitch.SetActive(false);
            UIManager.Instance.weaponStandUI.SetActive(false);
            UIManager.Instance.playerInfo.SetActive(true);
        }            
    }
}
