using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobberBehaviour : Inspectable
{
    public GameObject tile;

    public override string getInspectionText()
    {
        string text = "Robber\n";
        text += tile.GetComponent<TileBehaviour>().getInspectionText()+"\n";
        return text;
    }
}
