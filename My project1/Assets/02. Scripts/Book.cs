using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Item
{
    public enum BooksType
    {
        red = 1,
        green = 2,
        blue =3
    }

    public BooksType booksType;



    public override void UseItem()
    {
        switch (booksType)
        {
            case BooksType.red:
                playerCtrl.dmg += 5.0f;
                break;
            case BooksType.green:
                playerCtrl.maxSkillCnt++;
                break;
            case BooksType.blue:
                playerCtrl.moveSpeed *= 1.2f;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && booksType == BooksType.red)
        {
            Destroy(gameObject);
            UseItem();
        }
        else if (other.CompareTag("Player") && booksType == BooksType.green)
        {
            Destroy(gameObject);
            UseItem();
        }
        else if (other.CompareTag("Player") && booksType == BooksType.blue)
        {
            Destroy(gameObject);
            UseItem();
        }
    }
}
