using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageChoice : MonoBehaviour
{
    SpawnEnemy spawnEnemy;

    bool isContact = false;

    private void Awake()
    {
        spawnEnemy = GameObject.FindGameObjectWithTag("RandomSpawnGroup").GetComponent<SpawnEnemy>();
    }

    private void Update()
    {
        if (isContact && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Stage" + spawnEnemy.stageNum);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.isClear)
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
