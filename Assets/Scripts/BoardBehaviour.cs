using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBehaviour : MonoBehaviour
{

    // Prefabs
    public GameObject tilePrefab;
    public GameObject vertexPrefab;
    public GameObject edgePrefab;

    public BoardGrid boardGrid;

    public GameObject[] tileObjects;

    // Robber object
    public GameObject robber;

    // Robber location
    public GameObject robberTile = null;

    // Token initialization variables
    int[] tokenValues = new int[] { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };
    int[] tokenOrderCol = new int[] { 3, 3, 3, 4, 5, 6, 7, 7, 7, 6, 5, 4, 4, 4, 5, 6, 6, 5, 5};
    int[] tokenOrderRow = new int[] { 5, 4, 3, 2, 1, 1, 1, 2, 3, 4, 5, 5, 4, 3, 2, 2, 3, 4, 3};

    public void Awake()
    {
        boardGrid = new BoardGrid(gameObject, 10, 10);
        tileObjects = new GameObject[19];
    }

    /**
     * The board are set up like a Catan board.
     * Row 1: Col 5->8
     * Row 2: Col 4->8
     * Row 3: Col 3->8
     * Row 4: Col 3->7
     * Row 5: Col 3->6
     */
    public void SetupBoardGrid()
    {   
        for (int col = 3; col < 6; col++)
        {
            boardGrid.CreateFace(tilePrefab, vertexPrefab, edgePrefab, col, 5);
        }

        for (int col = 3; col < 7; col++)
        {
            boardGrid.CreateFace(tilePrefab, vertexPrefab, edgePrefab, col, 4);
        }

        for (int col = 3; col < 8; col++)
        {
            boardGrid.CreateFace(tilePrefab, vertexPrefab, edgePrefab, col, 3);
        }

        for (int col = 4; col < 8; col++)
        {
            boardGrid.CreateFace(tilePrefab, vertexPrefab, edgePrefab, col, 2);
        }
        for (int col = 5; col < 8; col++)
        {
            boardGrid.CreateFace(tilePrefab, vertexPrefab, edgePrefab, col, 1);
        }

        boardGrid.UpdatePositions();
    }

    /**
     * Randomise the resource types for each tile in the grid.
     */
    public void RandomizeTileTypes()
    {
        
        // Store all of the board grid's items in an array here.
        int faceNum = 0;
        for (int col = 0; col < 10; col++)
        {
            for (int row = 0; row < 10; row++)
            {
                GameObject face = boardGrid.GetFace(col, row);
                if (face != null)
                {
                    tileObjects[faceNum] = boardGrid.GetFace(col, row);
                    faceNum++;
                }
            }
        }

        // Create resources array
        List<ResourceType> resources = new List<ResourceType>();
        for(int i = 0; i < GameConfig.DEFAULT_FOREST_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Lumber);
        }
        for(int i = 0; i < GameConfig.DEFAULT_HILL_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Brick);
        }
        for(int i = 0; i < GameConfig.DEFAULT_MOUNTAIN_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Ore);
        }
        for(int i = 0; i < GameConfig.DEFAULT_MEADOW_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Wool);
        }
        for(int i = 0; i < GameConfig.DEFAULT_FIELD_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Grain);
        }
        resources.Add(ResourceType.Desert);

        // Randomize resources array
        for (int i = 0; i < 100; i++)
        {
            int start = Random.Range(0, resources.Count);
            int end = Random.Range(0, resources.Count);

            ResourceType temp = resources[start];
            resources[start] = resources[end];
            resources[end] = temp;
        }

        for(int i = 0; i < tileObjects.Length; i++)
        {
            tileObjects[i].GetComponent<TileBehaviour>().resourceType = resources[i];
        }
    }

    /**
     * Setup chance token values for each tile on the grid.
     */
    public void SetupChanceTokens()
    {
        int tokenIndex = 0;
        for(int i = 0; i < tokenOrderCol.Length; i++)
        {
            GameObject tile = boardGrid.GetFace(tokenOrderCol[i], tokenOrderRow[i]);
            if(!(tile.GetComponent<TileBehaviour>().resourceType == ResourceType.Desert || tile.GetComponent<TileBehaviour>().resourceType == ResourceType.None))
            {
                tile.GetComponent<TileBehaviour>().chanceTokenValue = tokenValues[tokenIndex];
                tokenIndex++;
            }
        }
    }

    /**
     * Move robber to desert
     */
    public void MoveRobberToDesert()
    {
        // Find desert
        for(int i = 0; i < tileObjects.Length; i++)
        {
            GameObject tile = tileObjects[i];
            if (tile.GetComponent<TileBehaviour>().resourceType == ResourceType.Desert)
            {
                robberTile = tile;
            }
        }
    }

    /**
     * Move robber to tile index
     */
    public void MoveRobberToTile(int col, int row)
    {
        robberTile = boardGrid.GetFace(col, row);
    }

    void Update()
    {
        if(robberTile != null)
        {
            robber.transform.localPosition = robberTile.transform.localPosition;
        }
    }
}
