using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnEnemy : MonoBehaviour
{
    RewardManager rewardManager;

    public GameObject mEnemy;
    public GameObject rEnemy;

    public List<Transform> points = new List<Transform>();
    public List<Transform> usedPoints = new List<Transform>();

    public int curMonsterCnt = 0;
    public static int stageNum = 1;

    int curSpawnCnt = 0;
    int maxSpawnCnt = 2;
    int maxMonsterCnt = 10;

    public bool isMonsterClear = false;
    public static bool isBoss1Clear = false;
    public static bool isBoss2Clear = false;
    public static bool isBoss3Clear = false;

    void Start()
    {
        rewardManager = GameObject.FindGameObjectWithTag("RewardManager")?.GetComponent<RewardManager>();        
        //Transform spawnPointGroup = GameObject.FindGameObjectWithTag("RandomSpawnGroup")?.transform;
        //foreach (Transform point in spawnPointGroup)
        //{
        //    points.Add(point);
        //}
        if (SceneManager.GetActiveScene().name == "02. Stage1-Boss" || SceneManager.GetActiveScene().name == "03. Stage2-Boss" || SceneManager.GetActiveScene().name == "04. Stage3-Boss" || SceneManager.GetActiveScene().name == "05. Shop")
        {
            return;
        }
        else
        {
            isMonsterClear = false;
            CreateEnemy();
        }
    }



    void Update()
    {

    }

    void CreateEnemy()
    {
        curSpawnCnt++;
        
        if (stageNum == 1)
        {
            for (int i = 0; i < maxMonsterCnt; i++)
            {
                int idx = GetRandomNum();
                Instantiate(mEnemy, points[idx].position, points[idx].rotation);
                usedPoints.Add(points[idx]);
            }
        }
        else
        {
            for (int i = 0; i < maxMonsterCnt / 2; i++)
            {
                int idx = GetRandomNum();
                Instantiate(mEnemy, points[idx].position, points[idx].rotation);
                usedPoints.Add(points[idx]);  
                
                int idx2 = GetRandomNum();
                Instantiate(rEnemy, points[idx2].position, points[idx2].rotation);
                usedPoints.Add(points[idx2]);
            }
        }

        curMonsterCnt = maxMonsterCnt;
    }

    public void CheckEnemy()
    {
        if (curMonsterCnt == 0 && curSpawnCnt < maxSpawnCnt)
        {
            CreateEnemy();
            isMonsterClear = false;
        }

        if (curMonsterCnt == 0 && curSpawnCnt == maxSpawnCnt)
        {
            isMonsterClear = true;
            rewardManager.RandomReward(3);
            UIManager.Instance.rewardUI.SetActive(true);
        }
    }

    int GetRandomNum()
    {
        int idx = Random.Range(0, points.Count);
        while (usedPoints.Contains(points[idx]))
        {
            idx = Random.Range(0, points.Count);
        }
        return idx;
    }
}
