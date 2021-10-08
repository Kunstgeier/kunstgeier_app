using UnityEngine;
using System;

// T ist Datentyp
[Serializable]
public class APIReturnParser<T>
{
    public T data;
    public bool error;
    public MetaObject meta;
}
