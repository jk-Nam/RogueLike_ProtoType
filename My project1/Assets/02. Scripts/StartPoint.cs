using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartPoint : MonoBehaviour
{
    bool isContact = false;



    void Start()
    {
        
    }

    void Update()
    {
        if (isContact && Input.GetKeyUp(KeyCode.E))
        {
            SceneManager.LoadScene("03. Stage1");
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
