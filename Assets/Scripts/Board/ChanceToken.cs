using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToken : MonoBehaviour
{
    public TextMesh textMesh;
    public int value = 0;

    public int GetValue()
    {
        return value;
    }

    public void SetValue(int value)
    {
        this.value = value;
        textMesh.text = value + "";
    }
}
