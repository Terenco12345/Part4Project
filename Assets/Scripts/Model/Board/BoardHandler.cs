using System;
using System.Collections.Generic;

public class BoardHandler
{
    BoardGrid boardGrid;

    public int robberCol;
    public int robberRow;

    // Token initialization variables
    int[] chanceValues = new int[] { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };
    int[] circularOrderCols = new int[] { 3, 3, 3, 4, 5, 6, 7, 7, 7, 6, 5, 4, 4, 4, 5, 6, 6, 5, 5 };
    int[] circularOrderRows = new int[] { 5, 4, 3, 2, 1, 1, 1, 2, 3, 4, 5, 5, 4, 3, 2, 2, 3, 4, 3 };

    public BoardHandler()
    {
        boardGrid = new BoardGrid(10, 10);
        robberCol = 0;
        robberRow = 0;
    }

    public BoardGrid GetBoardGrid()
    {
        return boardGrid;
    }

    /**
     * Initialize all the faces for a settlers board grid layout, with edges and vertices.
     */
    public void CreateSettlersBoard()
    {
        for (int col = 3; col < 6; col++)
        {
            boardGrid.CreateFace(col, 5);
        }

        for (int col = 3; col < 7; col++)
        {
            boardGrid.CreateFace(col, 4);
        }

        for (int col = 3; col < 8; col++)
        {
            boardGrid.CreateFace(col, 3);
        }

        for (int col = 4; col < 8; col++)
        {
            boardGrid.CreateFace(col, 2);
        }
        for (int col = 5; col < 8; col++)
        {
            boardGrid.CreateFace(col, 1);
        }
    }

    /**
     * Randomise the resource types for each tile in the grid.
     */
    public void CreateTiles()
    {
        List<Face> faces = boardGrid.GetFacesAsList();
        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].tile = new Tile();
        }
    }

    /**
     * Create resource types for all tiles
     */
    public void SetupTileResourceTypes()
    {
        // Create resources array
        List<ResourceType> resources = new List<ResourceType>();
        for (int i = 0; i < Game.DEFAULT_FOREST_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Lumber);
        }
        for (int i = 0; i < Game.DEFAULT_HILL_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Brick);
        }
        for (int i = 0; i < Game.DEFAULT_MOUNTAIN_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Ore);
        }
        for (int i = 0; i < Game.DEFAULT_MEADOW_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Wool);
        }
        for (int i = 0; i < Game.DEFAULT_FIELD_TILE_AMOUNT; i++)
        {
            resources.Add(ResourceType.Grain);
        }
        resources.Add(ResourceType.Desert);

        Random random = new Random();
        // Randomize resources array
        for (int i = 0; i < 100; i++)
        {
            int start = random.Next(0, resources.Count);
            int end = random.Next(0, resources.Count);

            ResourceType temp = resources[start];
            resources[start] = resources[end];
            resources[end] = temp;
        }

        List<Face> faces = boardGrid.GetFacesAsList();
        for (int i = 0; i < faces.Count; i++)
        {
            faces[i].tile.resourceType = resources[i];
        }
    }

    /**
     * Setup chance token values for each tile on the grid. Tokens are generated in circular order.
     */
    public void SetupChanceTokens()
    {
        int tokenIndex = 0;
        for (int i = 0; i < circularOrderCols.Length; i++)
        {
            Tile tile = boardGrid.GetFace(circularOrderCols[i], circularOrderRows[i]).tile;
            if (!(tile.resourceType == ResourceType.Desert || tile.resourceType == ResourceType.None))
            {
                tile.chanceValue = chanceValues[tokenIndex];
                tokenIndex++;
            }
        }
    }

    public List<Settlement> GetAllSettlementsForPlayer(string playerId)
    {
        List<Settlement> settlements = new List<Settlement>();
        foreach(Vertex vertex in boardGrid.GetVerticesAsList())
        {
            if(vertex.settlement.owner.GetId() == playerId)
            {
                settlements.Add(vertex.settlement);
            }
        }

        return settlements;
    }

    public List<Road> GetAllRoadsForPlayer(string playerId)
    {
        List<Road> roads = new List<Road>();
        foreach (Edge edge in boardGrid.GetEdgesAsList())
        {
            if (edge.road.owner.GetId() == playerId)
            {
                roads.Add(edge.road);
            }
        }

        return roads;
    }

    /**
     * Robber's first tile is at the desert. The robber represents the most recent "robbery" occurance.
     */
    public void SetupRobberAtDesert()
    {
        for (int col = 0; col < boardGrid.GetColCount(); col++)
        {
            for (int row = 0; row < boardGrid.GetRowCount(); row++)
            {
                if (boardGrid.GetFace(col, row) != null && boardGrid.GetFace(col, row).tile != null)
                {
                    if (boardGrid.GetFace(col, row).tile.resourceType == ResourceType.Desert)
                    {
                        robberCol = col;
                        robberRow = row;
                    }
                }
            }
        }
    }
}
