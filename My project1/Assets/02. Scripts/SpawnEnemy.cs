using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject mEnemy;
    public GameObject rEnemy;

    public List<Transform> points = new List<Transform>();
    public List<Transform> usedPoints = new List<Transform>();

    public int curMonsterCnt = 0;
    public int stageNum = 1;

    int curSpawnCnt = 0;
    int maxSpawnCnt = 3;
    int maxMonsterCnt = 10;


    void Start()
    {
        Transform spawnPointGroup = GameObject.FindGameObjectWithTag("RandomSpawnGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        CreateEnemy();
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
            GameManager.Instance.isClear = false;
        }

        if (curMonsterCnt ==0 && curSpawnCnt >= maxSpawnCnt)
        {
            GameManager.Instance.isClear = true;
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
