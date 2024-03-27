using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Skill2Projectile : MonoBehaviour
{
    Transform playerTr;
    
    
    public float moveSpeed = 5.0f;
    public float dmg = 10.0f;

    Vector3 dir;

    void Start()
    {        
        playerTr = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Invoke("SelfDestroy", 3.0f);
        dir = (transform.position - playerTr.position).normalized;
    }

    void Update()
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime);
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
