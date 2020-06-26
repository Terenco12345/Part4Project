using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : Inspectable
{
    public GameObject tile;

    public override string getInspectionText()
    {
        string text = "Robber\n";
        text += tile.GetComponent<Tile>().getInspectionText()+"\n";
        return text;
    }
}
