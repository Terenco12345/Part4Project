using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehaviour : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject chanceTokenPrefab;
    readonly int[] chanceTokenApplicationOrder = { 7, 3, 0, 1, 2, 6, 11, 15, 18, 17, 16, 12, 8, 4, 5, 10, 14, 13, 9 };
    readonly int[] chanceTokenValues = { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };

    public void SetupBoard()
    {
        SetupTilesRandom();
        SetupChanceTokensRandom();
    }

    /**
     * This method will set up the resource tiles for the game, by changing them to specific resources.
     */ 
    void SetupTilesRandom()
    {
        int forestTileCount = GameConfig.DEFAULT_FOREST_TILE_AMOUNT;
        int hillTileCount = GameConfig.DEFAULT_HILL_TILE_AMOUNT;
        int meadowTileCount = GameConfig.DEFAULT_MEADOW_TILE_AMOUNT;
        int mountainTileCount = GameConfig.DEFAULT_MOUNTAIN_TILE_AMOUNT;
        int fieldTileCount = GameConfig.DEFAULT_FIELD_TILE_AMOUNT;
        int desertTileCount = GameConfig.DEFAULT_DESERT_TILE_AMOUNT;

        // Set up tile randomisation array
        ResourceType[] tileTypesArray = new ResourceType[forestTileCount+ hillTileCount + meadowTileCount+mountainTileCount+ fieldTileCount + desertTileCount];
        int n = 0;
        for(int i = n; i < n+forestTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Lumber;
        }
        n += forestTileCount;
        for (int i = n; i < n+hillTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Brick;
        }
        n += hillTileCount;
        for (int i = n; i < n+meadowTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Wool;
        }
        n += meadowTileCount;
        for (int i = n; i < n+mountainTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Ore;
        }
        n += mountainTileCount;
        for (int i = n; i < n+fieldTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Grain;
        }
        n += fieldTileCount;
        for (int i = n; i < n+desertTileCount; i++)
        {
            tileTypesArray[i] = ResourceType.Desert;
        }

        // Shuffle tile randomisation array
        for(int i = 0; i < 100; i++)
        {
            int firstIndex = Random.Range(0, tileTypesArray.Length);
            int secondIndex = Random.Range(0, tileTypesArray.Length);
            ResourceType temp = tileTypesArray[firstIndex];
            tileTypesArray[firstIndex] = tileTypesArray[secondIndex];
            tileTypesArray[secondIndex] = temp;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            TileBehaviour tile = tiles[i].GetComponent<TileBehaviour>();
            tile.SetResourceType(tileTypesArray[i]);
            tile.Setup();
        }
    }

    /**
     * This method will set up the chance tokens for the game (indicators for which roll will make the tile produce resources)
     */
    void SetupChanceTokensRandom()
    {
        int chanceValueIndex = 0;
        // Place the chance tokens down in a cyclical order.
        for (int i = 0; i < tiles.Length; i++)
        {
            TileBehaviour tile = tiles[chanceTokenApplicationOrder[i]].GetComponent<TileBehaviour>();
            if (tile.resourceType != ResourceType.Desert) // This means this is the desert, skip over it and don't increment chanceValueIndex
            {
                tile.SetChanceTokenValue(chanceTokenValues[chanceValueIndex]);
                chanceValueIndex++;
                tile.Setup();
            }
        }
    }
}
