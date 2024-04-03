using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;
    PlayerCtrl playerCtrl;

    public GameObject playerInfo;
    public GameObject weaponStandUI;
    public GameObject questUI;
    public GameObject pressEQ;
    public GameObject upgradeUI;
    public GameObject pressEU;
    public GameObject pressES;
    public GameObject pressEStage;
    public GameObject rewardUI;
    public GameObject weaponUpgradeUI;


    public Text curHp;
    public Text maxHp;
    public Text curSkill;
    public Text maxSkill;
    public Text coinCnt;
    public Text forceCnt;
    public Text steelCnt;
    public Slider hpBar;
    public GameObject[] lifeImages;

    public int maxLife = 3;
    public int curLife = 0;

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

        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Start()
    {
        //weaponStand.SetActive(false);
        //quest.SetActive(false);
        //upgrade.SetActive(false);
        //pressEQ.SetActive(false);
        //pressEU.SetActive(false);
        //pressES.SetActive(false);

        curLife = maxLife;
        maxHp.text = playerCtrl.maxHp.ToString();
        maxSkill.text = playerCtrl.maxSkillCnt.ToString();
        UpdateCoin();
        UpdateForce();
        UpdateHpBar();
        UpdateLife();
        UpdateSkill();
        UpdateSteel();
    }

    void Update()
    {
        
    }

    public void UpdateHpBar()
    {
        hpBar.value = playerCtrl.curHp / playerCtrl.maxHp;
        curHp.text = playerCtrl.curHp.ToString();
    }

    public void UpdateLife()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            if (i < playerCtrl.curLifeCnt)
            {
                lifeImages[i].SetActive(true);
            }
            else
            {
                lifeImages[i].SetActive(false);
            }
        }
    }

    public void UpdateSkill()
    {
        curSkill.text = playerCtrl.curSkillCnt.ToString();
    }

    public void UpdateCoin()
    {
        coinCnt.text = GameManager.Instance.playerCoin.ToString();
    }

    public void UpdateForce()
    {
        forceCnt.text = GameManager.Instance.playerForce.ToString();
    }

    public void UpdateSteel()
    {
        steelCnt.text = GameManager.Instance.playerSteel.ToString();
    }
}
