using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Skill2Projectile : MonoBehaviour
{
    Transform playerTr;
    
    
    public float moveSpeed = 5.0f;
    public float dmg = 10.0f;

    void Start()
    {        
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("SelfDestroy", 5.0f);
        transform.LookAt(playerTr.position);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
