using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public Transform target;

    public Vector3 offset;


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }


    void Update()
    {
        transform.position = target.position + offset;
        //transform.LookAt(target.position);
    }
}
