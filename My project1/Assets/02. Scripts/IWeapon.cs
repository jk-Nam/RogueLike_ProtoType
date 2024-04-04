using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{


    public IEnumerator Attack();


    public IEnumerator SAttack();


    public IEnumerator Skill();


    void Upgrade();

}
