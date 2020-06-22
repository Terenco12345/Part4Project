using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeBehaviour : Inspectable
{
    public int col;
    public int row;
    public BoardGrid.EdgeSpecifier edgeSpec;

    public override string getInspectionText()
    {
        string text = "Edge\n";
        text += "Col " + col + ", Row " + row+", "+edgeSpec.ToString();
        return text;
    }
}
