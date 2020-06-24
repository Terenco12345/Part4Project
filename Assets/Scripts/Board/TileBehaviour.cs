using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A tile may hold a robber, a resource type, or a chance token.
 */
public class TileBehaviour : Inspectable
{
    public Material noneMaterial;
    public Material forestMaterial;
    public Material hillMaterial;
    public Material meadowMaterial;
    public Material mountainMaterial;
    public Material fieldMaterial;
    public Material desertMaterial;

    public ResourceType resourceType = ResourceType.None; // The resource type of this tile, i.e. lumber, brick etc.
    public int chanceTokenValue; // Value that determines what rolls will produce resources

    public GameObject chanceTokenPrefab;

    GameObject chanceToken = null;

    public void LateUpdate()
    {
        // Change material to match the resource type of this tile
        Material currentMaterial = noneMaterial;
        switch (resourceType)
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
        if (chanceTokenValue == 0 || resourceType == ResourceType.Desert) // If this token value is 0, there is no token tile
        {
            if(chanceToken != null)
            {
                Destroy(chanceToken);
            }
        } else // Otherwise render the correct token value
        {
            if (chanceToken == null)
            {
                chanceToken = Instantiate(chanceTokenPrefab, transform);

            }
            chanceToken.GetComponent<ChanceTokenBehaviour>().SetValue(chanceTokenValue);
        }
    }

    public override string getInspectionText()
    {
        string text = "Tile\n";
        text += "Col " + GetComponent<FaceBehaviour>().col + ", Row " + GetComponent<FaceBehaviour>().row+"\n";
        text += resourceType.ToString()+" tile"+(resourceType == ResourceType.Desert || resourceType == ResourceType.None ? "" : "\n" + chanceTokenValue + " roll needed for production.");
        return text;
    }
}
