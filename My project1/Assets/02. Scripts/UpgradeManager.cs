using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Drawing;

public class UpgradeManager : MonoBehaviour
{
    PlayerCtrl playerCtrl;

    public GameObject uSwitch;

    [System.Serializable]
    public class UpgradeData
    {
        public int upgradeIndex;
        public int curUpgrade;
        public int maxUpgrade;
        public int needForce;
        public int forceIncrease;
    }

    public UpgradeData[] upgradeDataArray;

    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }
    void Start()
    {

    }


    void Update()
    {
        if (uSwitch.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.upgradeUI.SetActive(true);
                UIManager.Instance.pressEU.SetActive(false);
            }
        }

        if (UIManager.Instance.upgradeUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIManager.Instance.upgradeUI.SetActive(false);
                UIManager.Instance.pressEU.SetActive(true);
            }
        }
    }

    public void SaveUpgrade()
    {
        JSONObject upgradeInfo = new JSONObject();
        JSONArray upgradesArray = new JSONArray();

        for (int i = 0; i < upgradeDataArray.Length; i++)
        {
            JSONObject upgradeData = new JSONObject();
            upgradeData.Add("Index", upgradeDataArray[i].upgradeIndex);
            upgradeData.Add("CurUpgrade", upgradeDataArray[i].curUpgrade);
            upgradeData.Add("MaxUpgrade", upgradeDataArray[i].maxUpgrade);
            upgradeData.Add("NeedForce", upgradeDataArray[i].needForce);
            upgradeData.Add("ForceIncrease", upgradeDataArray[i].forceIncrease);

            upgradesArray.Add(upgradeData);
        }

        upgradeInfo.Add("Upgrades", upgradesArray);

        Debug.Log(upgradeInfo);
        string path = Application.persistentDataPath + "/Player1UpgradeSave.json";
        File.WriteAllText(path, upgradeInfo.ToString());
    }

    public void LoadUpgrade()
    {
        string path = Application.persistentDataPath + "/Player1UpgradeSave.json";

        string jsonString = File.ReadAllText(path);
        JSONObject upgradeInfo = JSON.Parse(jsonString) as JSONObject;

        JSONArray upgradesArray = upgradeInfo["Upgrades"].AsArray;

        for (int i = 0; i < upgradesArray.Count; i++)
        {
            JSONObject upgradeData = upgradesArray[i].AsObject;
            upgradeDataArray[i].upgradeIndex = upgradeData["Index"].AsInt;
            upgradeDataArray[i].curUpgrade = upgradeData["CurUpgrade"];
            upgradeDataArray[i].maxUpgrade = upgradeData["MaxUpgrade"];
            upgradeDataArray[i].needForce = upgradeData["NeedForce"];
            upgradeDataArray[i].forceIncrease = upgradeData["ForceIncrease"];
        }
    }

    public void UpgradeDmg()
    {
        if (upgradeDataArray[1].curUpgrade >= upgradeDataArray[1].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[1].needForce)
        {
            upgradeDataArray[1].curUpgrade++;
            upgradeDataArray[1].needForce += upgradeDataArray[1].forceIncrease;
            playerCtrl.dmg *= 1.05f * upgradeDataArray[1].curUpgrade;
        }
    }

    public void UpgradeHP()
    {
        if (upgradeDataArray[2].curUpgrade >= upgradeDataArray[2].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[2].needForce)
        {
            upgradeDataArray[2].curUpgrade++;
            upgradeDataArray[2].needForce += upgradeDataArray[2].forceIncrease;
            playerCtrl.maxHp += 5.0f * upgradeDataArray[2].curUpgrade;
        }
    }

    public void UpgradeDash()
    {
        if (upgradeDataArray[3].curUpgrade >= upgradeDataArray[3].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[3].needForce)
        {
            upgradeDataArray[3].curUpgrade++;
            upgradeDataArray[3].needForce += upgradeDataArray[3].forceIncrease;
            playerCtrl.maxDashCnt++;
        }
    }

    public void UpgradeCoin()
    {
        if (upgradeDataArray[4].curUpgrade >= upgradeDataArray[4].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[4].needForce)
        {
            upgradeDataArray[4].curUpgrade++;
            upgradeDataArray[4].needForce += upgradeDataArray[4].forceIncrease;
            GameManager.Instance.upgradeCoin += 50 * upgradeDataArray[4].curUpgrade;
        }
    }

    public void UpgradeLife()
    {
        if (upgradeDataArray[5].curUpgrade >= upgradeDataArray[5].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[5].needForce)
        {
            upgradeDataArray[5].curUpgrade++;
            upgradeDataArray[5].needForce += upgradeDataArray[5].forceIncrease;
            playerCtrl.maxLifeCnt++;
        }
    }

    public void UpgradeSkill()
    {
        if (upgradeDataArray[6].curUpgrade >= upgradeDataArray[6].maxUpgrade && GameManager.Instance.playerForce >= upgradeDataArray[6].needForce)
        {
            upgradeDataArray[6].curUpgrade++;
            upgradeDataArray[6].needForce += upgradeDataArray[6].forceIncrease;
            playerCtrl.maxSkillCnt++;
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
