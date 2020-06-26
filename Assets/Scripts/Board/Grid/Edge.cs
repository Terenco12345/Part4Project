using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : Inspectable
{
    public int col;
    public int row;
    public BoardGrid.EdgeSpecifier edgeSpec;

    public GameObject road;

    public override string getInspectionText()
    {
        string text = "Edge\n";
        text += "Col " + col + ", Row " + row+", "+edgeSpec.ToString();
        return text;
    }
}
