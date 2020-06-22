using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexBehaviour : Inspectable
{
    public int col;
    public int row;
    public BoardGrid.VertexSpecifier vertexSpec;

    public override string getInspectionText()
    {
        string text = "Vertex\n";
        text += "Col " + col + ", Row " + row + ", " + vertexSpec.ToString() + "\n";
        return text;
    }
}
