using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : Item
{
    public enum BooksType
    {
        Red = 1,
        Green = 2,
        Blue =3,
        Yellow = 4,
        Cyan = 5,
        Black = 6,
        White = 7,
        Purple = 8,
        Sky = 9,
        Orange = 10
    }

    public BooksType booksType;

    public override void UseItem()
    {
        switch (booksType)
        {
            case BooksType.Red:
                playerCtrl.dmg += 5.0f;
                break;
            case BooksType.Green:
                playerCtrl.maxSkillCnt++;
                break;
            case BooksType.Blue:
                playerCtrl.moveSpeed *= 1.2f;
                break;
            case BooksType.Yellow:

                break;
            case BooksType.Cyan:

                break;
            case BooksType.Black:

                break;
            case BooksType.White:

                break;
            case BooksType.Purple:

                break;
            case BooksType.Sky:

                break;
            case BooksType.Orange:

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {            
            Destroy(gameObject);
            UseItem();
        }       
    }
}
