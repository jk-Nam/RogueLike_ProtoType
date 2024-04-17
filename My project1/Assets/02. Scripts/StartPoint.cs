using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    public StageManager stageManager;

    bool isContact = false;

    void Start()
    {

    }

    void Update()
    {
        if (isContact && Input.GetKeyUp(KeyCode.E))
        {
            SceneManager.LoadScene(stageManager.stage1List[0].ToString());
            stageManager.stage1List.RemoveAt(0);
            UIManager.Instance.pressES.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.pressES.SetActive(true);
            isContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.pressES.SetActive(false);
            isContact = false;
        }
    }
}
