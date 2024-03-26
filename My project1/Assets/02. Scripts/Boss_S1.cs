using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_S1 : MonoBehaviour
{
    public enum BOSSSTATE
    {
        IDLE = 0,
        ATTACK = 1,
        SKILL = 2,
        DEATH = 3
    }

    PlayerCtrl playerCtrl;

    public GameObject reward;

    public float hp = 200.0f;


    void Start()
    {
        playerCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
    }

    void Update()
    {
        
    }
}
