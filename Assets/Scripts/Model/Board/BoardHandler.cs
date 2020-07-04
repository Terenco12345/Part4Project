using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

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
        boardGrid = new BoardGrid(Game.DEFAULT_BOARD_GRID_COLS, Game.DEFAULT_BOARD_GRID_ROWS);
        robberCol = 0;
        robberRow = 0;
    }

    public BoardGrid GetBoardGrid()
    {
        return boardGrid;
    }

    public void SetBoardGrid(BoardGrid boardGrid)
    {
        this.boardGrid = boardGrid;
    }

    /**
     * Initialize all the faces for a settlers board grid layout, with edges and vertices.
     */
    public void CreateSettlersBoard()
    {
        for (int col = 3; col < 6; col++)
        {
            boardGrid.CreateFaceWithEdgesAndVertices(col, 5);
        }

        for (int col = 3; col < 7; col++)
        {
            boardGrid.CreateFaceWithEdgesAndVertices(col, 4);
        }

        for (int col = 3; col < 8; col++)
        {
            boardGrid.CreateFaceWithEdgesAndVertices(col, 3);
        }

        for (int col = 4; col < 8; col++)
        {
            boardGrid.CreateFaceWithEdgesAndVertices(col, 2);
        }
        for (int col = 5; col < 8; col++)
        {
            boardGrid.CreateFaceWithEdgesAndVertices(col, 1);
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

        System.Random random = new System.Random();
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
            if(vertex.settlement.ownerId == playerId)
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
            if (edge.road.ownerId == playerId)
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

    // Placement
    /**
     * Can place settlements only when there are no neighbouring settlements, and if connected to a road that player already owns.
     * In setup mode, can place settlements not connected to a road.
     */
    public bool CanPlaceSettlement(Player player, int col, int row, BoardGrid.VertexSpecifier vertexSpec)
    {
        // Check if there are any adjacent building -- Spacing requirement
        List<Vertex> adjacentVertices = boardGrid.GetAdjacentVerticesFromVertex(col, row, vertexSpec);
        foreach (Vertex adjacentVertex in adjacentVertices)
        {
            if (adjacentVertex.settlement != null)
            {
                return false;
            }
        }

        // Check if connected to a road owned by this player
        bool validRoadNearby = false;
        List<Edge> adjacentEdges = boardGrid.GetAdjacentEdgesFromVertex(col, row, vertexSpec);
        foreach (Edge adjacentEdge in adjacentEdges)
        {
            if (adjacentEdge.road != null)
            {
                if (adjacentEdge.road.ownerId == player.GetId())
                {
                    validRoadNearby = true;
                }
            }
        }
        if (!validRoadNearby && player.freeSettlements <= 0)
        {
            return false;
        }

        // Check if cost requirements met
        if (!player.CanAffordResourceTransaction(1, 1, 1, 1, 0) && player.freeSettlements <= 0)
        {
            return false;
        }

        // Check to see if tile contains a building and have enough settlements in store
        if (boardGrid.GetVertex(col, row, vertexSpec).settlement == null && player.storeSettlementNum > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
     * Can only place cities the player has the resources, and only to upgrade an existing settlement.
     */
    public bool CanPlaceCity(Player player, int col, int row, BoardGrid.VertexSpecifier vertexSpec)
    {
        // Check if cost requirements met (there is no such thing as a free city)
        if (!player.CanAffordResourceTransaction(0, 0, 2, 0, 3))
        {
            return false;
        }

        Vertex vertex = boardGrid.GetVertex(col, row, vertexSpec);
        // Check to see if vertex contains a settlement and have enough cities in store
        if (vertex.settlement != null && !vertex.settlement.isCity && vertex.settlement.ownerId.Equals(player.GetId()) && player.storeCityNum > 0) // If it is this player's settlement
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
     * Roads must be connected to a settlement or another road.
     */
    public bool CanPlaceRoad(Player player, int col, int row, BoardGrid.EdgeSpecifier edgeSpec)
    {
        // Check if connected to a settlement of the same player
        bool validbuildingNearby = false;
        List<Vertex> adjacentVertices = boardGrid.GetConnectedVerticesFromEdge(col, row, edgeSpec);
        foreach (Vertex vertex in adjacentVertices)
        {
            Settlement buildingObject = vertex.settlement;
            if (buildingObject != null)
            {
                if (buildingObject != null)
                {
                    if (buildingObject.ownerId == player.GetId())
                    {
                        validbuildingNearby = true;
                    }
                }
                else if (buildingObject != null)
                {
                    if (buildingObject.ownerId == player.GetId())
                    {
                        validbuildingNearby = true;
                    }
                }
            }
        }

        // Check if connected to a road of the same player
        bool validRoadNearby = false;
        List<Edge> adjacentEdges = boardGrid.GetConnectedEdgesFromEdge(col, row, edgeSpec);
        foreach (Edge adjacentEdge in adjacentEdges)
        {
            if (adjacentEdge.road != null)
            {
                if (adjacentEdge.road.ownerId == player.GetId())
                {
                    validRoadNearby = true;
                }
            }
        }

        if (!validRoadNearby && !validbuildingNearby)
        {
            return false;
        }

        // Check if cost requirements met
        if (!player.CanAffordResourceTransaction(1, 0, 0, 1, 0) && player.freeRoads <= 0)
        {
            return false;
        }

        // Check to see if tile is empty and have enough roads in store
        Edge edge = boardGrid.GetEdge(col, row, edgeSpec);
        if (edge.road == null && player.storeRoadNum > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}