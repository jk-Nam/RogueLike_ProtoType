using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{
    SpawnEnemy spawnEnemy;
    PlayerCtrl playerCtrl;
    SaveNLoadManager saveNLoadManager;

    bool isContact = false;


    private void Awake()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerCtrl>();
        spawnEnemy = GameObject.FindGameObjectWithTag("RandomSpawnGroup")?.GetComponent<SpawnEnemy>();
        saveNLoadManager = GameObject.FindGameObjectWithTag("SNLManager")?.GetComponent<SaveNLoadManager>();
    }

    private void Update()
    {
        if (isContact && Input.GetKeyDown(KeyCode.E) && (spawnEnemy.isMonsterClear || SpawnEnemy.isBoss1Clear || SpawnEnemy.isBoss2Clear))
        {
            if (SceneManager.GetActiveScene().name == "02. Stage1-Boss" || SceneManager.GetActiveScene().name == "03. Stage2-Boss")
            {
                SpawnEnemy.stageNum++;
                if (SpawnEnemy.stageNum <= 3)
                {
                    SpawnEnemy.isBoss1Clear = false;
                    SpawnEnemy.isBoss2Clear = false;
                    SceneManager.LoadScene("05. Shop");
                    playerCtrl.transform.position = Vector3.zero;
                }                
            }
            else if (SceneManager.GetActiveScene().name == "05. Shop")
            {
                if (SpawnEnemy.stageNum == 2)
                {
                    SceneManager.LoadScene(StageManager.Instance.stage2List[0].ToString());
                    playerCtrl.transform.position = Vector3.zero;
                    StageManager.Instance.stage2List.RemoveAt(0);
                }
                else if (SpawnEnemy.stageNum == 3)
                {
                    SceneManager.LoadScene(StageManager.Instance.stage3List[0].ToString());
                    playerCtrl.transform.position = Vector3.zero;
                    StageManager.Instance.stage3List.RemoveAt(0);
                }
            }
            else if (SpawnEnemy.stageNum == 1)
            {
                if (StageManager.Instance.stage1List.Count == 1)
                {
                    SceneManager.LoadScene("02. Stage1-Boss");
                    playerCtrl.transform.position = Vector3.zero;
                }
                else
                {
                    SceneManager.LoadScene(StageManager.Instance.stage1List[0].ToString());
                    playerCtrl.transform.position = Vector3.zero;
                    StageManager.Instance.stage1List.RemoveAt(0);
                }
            }
            else if (SpawnEnemy.stageNum == 2)
            {
                if (StageManager.Instance.stage2List.Count == 1)
                {
                    SceneManager.LoadScene("03. Stage2-Boss");
                    playerCtrl.transform.position = Vector3.zero;
                }
                else
                {
                    SceneManager.LoadScene(StageManager.Instance.stage2List[0].ToString());
                    playerCtrl.transform.position = Vector3.zero;
                    StageManager.Instance.stage2List.RemoveAt(0);
                }
            }
            else if (SpawnEnemy.stageNum == 3)
            {
                if (StageManager.Instance.stage3List.Count == 1)
                {
                    SceneManager.LoadScene("04. Stage3-Boss");
                    playerCtrl.transform.position = Vector3.zero;
                }
                else
                {
                    SceneManager.LoadScene(StageManager.Instance.stage3List[0].ToString());
                    playerCtrl.transform.position = Vector3.zero;
                    StageManager.Instance.stage3List.RemoveAt(0);
                }
            }

            UIManager.Instance.pressEStage.SetActive(false);
        }
        
        if (isContact && Input.GetKeyDown(KeyCode.E) && SpawnEnemy.isBoss3Clear)
        {
            saveNLoadManager.PlayerInfoSave(playerCtrl.playerNum);
            SceneManager.LoadScene("01. Town");
            playerCtrl.transform.position = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ((spawnEnemy.isMonsterClear || SpawnEnemy.isBoss1Clear || SpawnEnemy.isBoss2Clear)))
        {
            isContact = true;
            UIManager.Instance.pressEStage.SetActive(isContact);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isContact = false;
            UIManager.Instance.pressEStage.SetActive(isContact);
        }
    }
}
