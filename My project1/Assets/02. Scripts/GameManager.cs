using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    
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
    public bool isPlay = false;

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

    

    void TimeCheck()
    {
        if (isClear)
        {
            return;
        }

        float clearTime = Time.time - startTime;

        string minutes = ((int)clearTime / 60).ToString("00");
        string seconds = (clearTime % 60).ToString("00");

        //clearTimeText.text = minutes + " : " + seconds;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
