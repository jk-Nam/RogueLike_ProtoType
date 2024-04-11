using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;
using static WeaponManager;
using UnityEditor;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    IWeapon weapon;
    WeaponType weaponType;
    
    PlayerCtrl playerCtrl;
    public WeaponManager weaponMgr;    

    public GameObject myWeapon;
    public Text clearTimeText;

    public float startTime;
    public int playerCoin;
    public int playerForce;
    public int playerSteel;
    public int upgradeCoin;
    public bool isClear = false;

    public int totalClearCnt = 0;
    public int swordClearCnt = 0;
    public int axeClearCnt = 0;
    public int bowClearCnt = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        playerCtrl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCtrl>();
    }


    void Start()
    {
        playerCoin = 0;
        playerCoin += upgradeCoin * 50;
        myWeapon = weaponMgr.myWeapon;
    }


    void Update()
    {
        TimeCheck();
    }

    public void PlayerInfoLoad()
    {
        string path = Application.persistentDataPath + "/Player1Save.json";
        string jsonString = File.ReadAllText(path);
        JSONObject playerInfo = (JSONObject)JSON.Parse(jsonString);
        myWeapon.name = playerInfo["MyWeapon"];
        if (myWeapon.name == "Sword")
        {
            weaponMgr.SetWeaponType(WeaponType.Sword);
        }
        else if (myWeapon.name == "Axe")
        {
            weaponMgr.SetWeaponType(WeaponType.Axe);
        }
        else if (myWeapon.name == "Bow")
        {
            weaponMgr.SetWeaponType(WeaponType.Bow);
        }
        else
        {
            weaponMgr.SetWeaponType(WeaponType.Sword);
        }

        playerCoin = playerInfo["CoinCnt"];
        playerForce = playerInfo["ForceCnt"];
        playerSteel = playerInfo["SteelCnt"];
        totalClearCnt = playerInfo["TotalClearCnt"];
        swordClearCnt = playerInfo["SwordClearCnt"];
        axeClearCnt = playerInfo["AxeClearCnt"];
        bowClearCnt = playerInfo["BowClearCnt"];

        Debug.Log(playerInfo);
    }
    
    public void PlayerInfoSave()
    { 
        JSONObject playerInfo = new JSONObject();
        playerInfo.Add("MyWeapon", myWeapon.name);
        playerInfo.Add("CoinCnt", playerCoin);
        playerInfo.Add("ForceCnt", playerForce);
        playerInfo.Add("SteelCnt", playerSteel);
        playerInfo.Add("TotalClearCnt", totalClearCnt);
        playerInfo.Add("SwordClearCnt", swordClearCnt);
        playerInfo.Add("AxeClearCnt", axeClearCnt);
        playerInfo.Add("BowClearCnt", bowClearCnt);

        Debug.Log(playerInfo);
        string path = Application.persistentDataPath + "/Player1Save.json";
        File.WriteAllText(path, playerInfo.ToString());
    }

    void TimeCheck()
    {
        if (isClear)
        {
            return;
        }

        float clearTime = Time.time - startTime;

        string minutes = ((int)clearTime / 60).ToString("00");
        string seconds = (clearTime % 60).ToString("00");

        clearTimeText.text = minutes + " : " + seconds;
    }
}
