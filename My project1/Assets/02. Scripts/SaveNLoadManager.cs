using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static WeaponManager;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveNLoadManager : MonoBehaviour
{
    IWeapon weapon;
    WeaponType weaponType;
    PlayerCtrl playerCtrl;
    public WeaponManager weaponMgr;

    public GameObject myWeapon;
    public GameObject empty1;
    public GameObject empty2;
    public GameObject empty3;

    public Text player1TotClearCnt;
    public Text player1ForceCnt;
    public Text player1SteelCnt;
    public Text player2TotClearCnt;
    public Text player2ForceCnt;
    public Text player2SteelCnt;
    public Text player3TotClearCnt;
    public Text player3ForceCnt;
    public Text player3SteelCnt;

    private int saveNum;

    private string player1FileName = "Player1Save.json";
    private string player2FileName = "Player2Save.json";
    private string player3FileName = "Player3Save.json";

    void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        myWeapon = weaponMgr.myWeapon;
        ShowPlayerInfo();
    }

    public void ShowPlayerInfo()
    {
        if (File.Exists(GetPlayerFilePath(1)))
        {
            string path = GetPlayerFilePath(1);
            string jsonString = File.ReadAllText(path);
            JSONObject playerInfo = (JSONObject)JSON.Parse(jsonString);
            empty1.SetActive(false);
            player1TotClearCnt.text = playerInfo["TotalClearCnt"];
            player1ForceCnt.text = playerInfo["ForceCnt"];
            player1SteelCnt.text = playerInfo["SteelCnt"];
        }
        else
            empty1.SetActive(true);

        if (File.Exists(GetPlayerFilePath(2)))
        {
            string path = GetPlayerFilePath(2);
            string jsonString = File.ReadAllText(path);
            JSONObject playerInfo = (JSONObject)JSON.Parse(jsonString);
            empty2.SetActive(false);
            player2TotClearCnt.text = playerInfo["TotalClearCnt"];
            player2ForceCnt.text = playerInfo["ForceCnt"];
            player2SteelCnt.text = playerInfo["SteelCnt"];
        }
        else
            empty2.SetActive(true);

        if (File.Exists(GetPlayerFilePath(3)))
        {
            string path = GetPlayerFilePath(3);
            string jsonString = File.ReadAllText(path);
            JSONObject playerInfo = (JSONObject)JSON.Parse(jsonString);
            empty3.SetActive(false);
            player3TotClearCnt.text = playerInfo["TotalClearCnt"];
            player3ForceCnt.text = playerInfo["ForceCnt"];
            player3SteelCnt.text = playerInfo["SteelCnt"];
        }
        else
            empty3.SetActive(true);
    }

    public void PlayerInfoLoad(int playerNum)
    {
        string path = GetPlayerFilePath(playerNum);
        //string path = Application.persistentDataPath + "/Player1Save.json";
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

        GameManager.Instance.playerCoin = playerInfo["CoinCnt"];
        GameManager.Instance.playerForce = playerInfo["ForceCnt"];
        GameManager.Instance.playerSteel = playerInfo["SteelCnt"];
        GameManager.Instance.totalClearCnt = playerInfo["TotalClearCnt"];
        GameManager.Instance.swordClearCnt = playerInfo["SwordClearCnt"];
        GameManager.Instance.axeClearCnt = playerInfo["AxeClearCnt"];
        GameManager.Instance.bowClearCnt = playerInfo["BowClearCnt"];
        playerCtrl.playerNum = playerNum;

        Debug.Log(playerInfo);

        SceneManager.LoadScene("01. Town");
    }

    public void PlayerInfoSave(int playerNum)
    {
        JSONObject playerInfo = new JSONObject();
        playerInfo.Add("MyWeapon",      myWeapon.name);
        playerInfo.Add("CoinCnt",       GameManager.Instance.playerCoin);
        playerInfo.Add("ForceCnt",      GameManager.Instance.playerForce);
        playerInfo.Add("SteelCnt",      GameManager.Instance.playerSteel);
        playerInfo.Add("TotalClearCnt", GameManager.Instance.totalClearCnt);
        playerInfo.Add("SwordClearCnt", GameManager.Instance.swordClearCnt);
        playerInfo.Add("AxeClearCnt",   GameManager.Instance.axeClearCnt);
        playerInfo.Add("BowClearCnt",   GameManager.Instance.bowClearCnt);

        Debug.Log(playerInfo);
        string path = GetPlayerFilePath(playerNum);
        //string path = Application.persistentDataPath + "/Player1Save.json";
        File.WriteAllText(path, playerInfo.ToString());
    }

    public string GetPlayerFilePath(int playerNum)
    {
        string fileName;
        if (playerNum == 1)
            fileName = player1FileName;
        else if (playerNum == 2)
            fileName = player2FileName;
        else 
            fileName = player3FileName;


        return Application.persistentDataPath + "/" + fileName;
    }

    public void NewGame()
    {
        int playerNum = 1;
        while (File.Exists(GetPlayerFilePath(playerNum)))
        {
            playerNum++;
            if (playerNum > 3) 
            {   
                playerNum = 1;
                break;
            }
        }

        PlayerInfoSave(playerNum);
        SceneManager.LoadScene("01. Town");
    }

    public void DeleteSave(int playerNum)
    {
        File.Delete(GetPlayerFilePath(playerNum));
        Debug.Log("데이터가 삭제되었습니다.");
    }
}
