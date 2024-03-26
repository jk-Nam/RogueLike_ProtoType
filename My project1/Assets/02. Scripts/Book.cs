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

                break;
            case BooksType.green:

                break;
            case BooksType.blue:

                break;
        }
    }
}
