using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Transform spear;
    public float upSpeed = 5f; //spear가 위로 솟는 속도
    public float downSpeed = 1f; //spear가 내려가는 속도
    public float spearTime = 3f; //spear가 위에 머무는 시간
    public float dmg = 5.0f;

    public Vector3 desPos;
    public Vector3 originalPosition;
    public bool isContact = false;
    public bool isSpearUp = false;
    private float dwTime = 0f;


    void Start()
    {
        originalPosition = spear.transform.position;
        desPos = spear.transform.position + new Vector3(0, 0.77f, 0);
    }


    void Update()
    {
        if (isSpearUp)
        {
            dwTime += Time.deltaTime;

            if (dwTime >= spearTime)
            {
                isSpearUp = false;
                dwTime = 0f;
                isContact = false;
            }
        }
        else
        {            
            spear.position = Vector3.MoveTowards(spear.position, originalPosition, downSpeed * Time.deltaTime);
        }

        if (isContact)
        {
            ActivateTrap();
        }
    }

    public void ActivateTrap()
    {
        spear.position = Vector3.MoveTowards(spear.position, desPos, upSpeed * Time.deltaTime);
        isSpearUp = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            isContact = true;
        }
    }
}
