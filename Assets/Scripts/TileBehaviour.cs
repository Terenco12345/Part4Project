using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    public Material noneMaterial;
    public Material forestMaterial;
    public Material hillMaterial;
    public Material meadowMaterial;
    public Material mountainMaterial;
    public Material fieldMaterial;
    public Material desertMaterial;

    public ResourceType resourceType;
    public ChanceTokenBehaviour chanceToken;

    public void Setup()
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

        if (chanceToken.GetValue() == 0)
        {
            chanceToken.gameObject.SetActive(false);
        } else
        {
            chanceToken.gameObject.SetActive(true);
        }
    }

    public void SetChanceTokenValue(int value)
    {
        chanceToken.SetValue(value);
    }

    public int GetChanceTokenValue()
    {
        return chanceToken.GetValue();
    }

    public void SetResourceType(ResourceType resourceType)
    {
        this.resourceType = resourceType;
    }

    public ResourceType GetResourceType()
    {
        return resourceType;
    }
}
