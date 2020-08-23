using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    public int col;
    public int row;

    public GameObject noneTile;
    public GameObject forestTile;
    public GameObject hillTile;
    public GameObject meadowTile;
    public GameObject mountainTile;
    public GameObject fieldTile;
    public GameObject desertTile;

    public GameObject chanceTokenPrefab;
    GameObject chanceToken = null;

    public ResourceType tileResourceType;
    private ResourceType previousResourceType;
    GameObject tileObject;

    public void Start()
    {
        tileResourceType = ResourceType.None;
        previousResourceType = ResourceType.None;
    }

    public void Update()
    {
        Face face = FindObjectOfType<GameManager>().GetGame().GetBoardHandler().GetBoardGrid().GetFace(col, row);
        
        // Check if face.tile.resourceType has changed
        if (face.tile != null)
        {
            tileResourceType = face.tile.resourceType;
        } else
        {
            tileResourceType = ResourceType.None;
        }

        if (tileResourceType != previousResourceType)
        {
            UpdateTileObject();
        }

        previousResourceType = tileResourceType;
    }

    public void LateUpdate()
    {
        Face face = FindObjectOfType<GameManager>().GetGame().GetBoardHandler().GetBoardGrid().GetFace(col, row);
        if(face.tile != null)
        {
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
        }

        GetComponent<Inspectable>().inspectionText = GetInspectionText();
    }

    public void UpdateTileObject()
    {
        Face face = FindObjectOfType<GameManager>().GetGame().GetBoardHandler().GetBoardGrid().GetFace(col, row);

        if (tileObject != null)
        {
            Destroy(tileObject);
        }

        // Change material to match the resource type of this tile
        GameObject currentTile = noneTile;
        switch (face.tile.resourceType)
        {
            case ResourceType.None:
                currentTile = noneTile;
                break;
            case ResourceType.Lumber:
                currentTile = forestTile;
                break;
            case ResourceType.Brick:
                currentTile = hillTile;
                break;
            case ResourceType.Wool:
                currentTile = meadowTile;
                break;
            case ResourceType.Ore:
                currentTile = mountainTile;
                break;
            case ResourceType.Grain:
                currentTile = fieldTile;
                break;
            case ResourceType.Desert:
                currentTile = desertTile;
                break;
        }

        tileObject = Instantiate(currentTile, transform);
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
