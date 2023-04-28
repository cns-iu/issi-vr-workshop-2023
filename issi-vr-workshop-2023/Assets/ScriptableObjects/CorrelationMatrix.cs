using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CorrelationMatrix : ScriptableObject
{
    public string id;
    [field: SerializeField] public List<MatrixCell> rows = new List<MatrixCell>();
}

[Serializable]
public class MatrixCell
{
    public string id;
    public float corValue;
    public bool isFloat = true;

    public MatrixCell(string n, float v, bool isFloat)
    {
        id = n;
        this.isFloat = isFloat;

        corValue = !isFloat ? float.NaN : v;
    }


    //public MatrixCell(string n, float v, string a = "") => (id, corValue, na) = (n, v, a);
}
