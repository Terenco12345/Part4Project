using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    public int col;
    public int row;

    public Material noneMaterial;
    public Material forestMaterial;
    public Material hillMaterial;
    public Material meadowMaterial;
    public Material mountainMaterial;
    public Material fieldMaterial;
    public Material desertMaterial;

    public GameObject chanceTokenPrefab;
    GameObject chanceToken = null;

    public void Start()
    {

    }

    public void LateUpdate()
    {
        Face face = FindObjectOfType<GameManager>().GetGame().GetBoardHandler().GetBoardGrid().GetFace(col, row);
        if(face.tile != null)
        {
            // Change material to match the resource type of this tile
            Material currentMaterial = noneMaterial;
            switch (face.tile.resourceType)
            {
                case ResourceType.None:
                    currentMaterial = noneMaterial;
                    break;
                case ResourceType.Lumber:
                    currentMaterial = forestMaterial;
                    break;
                case ResourceType.Brick:
                    currentMaterial = hillMaterial;
                    break;
                case ResourceType.Wool:
                    currentMaterial = meadowMaterial;
                    break;
                case ResourceType.Ore:
                    currentMaterial = mountainMaterial;
                    break;
                case ResourceType.Grain:
                    currentMaterial = fieldMaterial;
                    break;
                case ResourceType.Desert:
                    currentMaterial = desertMaterial;
                    break;
            }
            GetComponent<MeshRenderer>().material = currentMaterial;

            // Instantiate a chance token for this tile
            if (face.tile.chanceValue == 0 || face.tile.resourceType == ResourceType.Desert) // If this token value is 0, there is no token tile
            {
                if (chanceToken != null)
                {
                    Destroy(chanceToken);
                }
            }
            else // Otherwise render the correct token value
            {
                if (chanceToken == null)
                {
                    chanceToken = Instantiate(chanceTokenPrefab, transform);

                }
                chanceToken.GetComponent<ChanceTokenRenderer>().SetValue(face.tile.chanceValue);
            }
        } else
        {
            GetComponent<MeshRenderer>().material = noneMaterial;
        }

        GetComponent<Inspectable>().inspectionText = GetInspectionText();
    }

    public string GetInspectionText()
    {
        Face face = FindObjectOfType<GameManager>().GetGame().GetBoardHandler().GetBoardGrid().GetFace(col, row);

        string text = "Tile\n";
        text += "Col " + col + ", Row " + row + "\n";
        text += face.tile.resourceType.ToString() + " tile" + (face.tile.resourceType == ResourceType.Desert || face.tile.resourceType == ResourceType.None ? "" : "\n" + face.tile.chanceValue + " roll needed for production.");
        return text;
    }
}
