using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject mEnemy;
    public GameObject rEnemy;

    public List<GameObject> liveEnemy;

    public List<Transform> points = new List<Transform>();

    int curSpawnCnt = 0;
    int maxSpawnCnt = 3;
    int maxMonsterCnt = 10;
    public int curMonsterCnt = 0;


    void Start()
    {
        Transform spawnPointGroup = GameObject.FindGameObjectWithTag("RandomSpawnGroup")?.transform;
        foreach (Transform point in spawnPointGroup)
        {
            points.Add(point);
        }

        CreateEnemy();
        StartCoroutine(CheckEnemy());
    }



    void Update()
    {

    }

    void CreateEnemy()
    {
        curSpawnCnt++;        
        for (int i = 0; i < maxMonsterCnt; ++i)
        {
            int idx = Random.Range(0, points.Count);
            Instantiate(mEnemy, points[idx].position, points[idx].rotation);
            curMonsterCnt = maxMonsterCnt;
        }
    }

    IEnumerator CheckEnemy()
    {
        yield return new WaitForSeconds(3.0f);
        

        if (liveEnemy == null && curSpawnCnt <= maxSpawnCnt)
        {
            CreateEnemy();
        }
    }
}
