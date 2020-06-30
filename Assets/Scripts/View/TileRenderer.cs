using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRenderer : MonoBehaviour
{
    public Tile tile;

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
        if(tile != null)
        {
            // Change material to match the resource type of this tile
            Material currentMaterial = noneMaterial;
            switch (tile.resourceType)
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
            if (tile.chanceValue == 0 || tile.resourceType == ResourceType.Desert) // If this token value is 0, there is no token tile
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
                chanceToken.GetComponent<ChanceTokenRenderer>().SetValue(tile.chanceValue);
            }
        }
    }
}
