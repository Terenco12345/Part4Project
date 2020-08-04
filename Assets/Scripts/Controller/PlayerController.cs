using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

/**
 * This class has commands which perform player actions.
 */
public class PlayerController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;

    /**
     * This player places a settlement.
     */
    [Command]
    public void CmdPlaceSettlement(int col, int row, int vertexSpec)
    {
        Player player = GameManager.Instance.GetPlayerById(playerBehaviour.netId+"");
        BoardGrid boardGrid = GameManager.Instance.GetGame().boardHandler.GetBoardGrid();

        Vertex vertex = boardGrid.GetVertex(col, row, (BoardGrid.VertexSpecifier)vertexSpec);
        Settlement settlement = new Settlement();
        settlement.ownerId = player.GetId();
        settlement.isCity = false;
        vertex.settlement = settlement;
        if(player.freeSettlements >= 1)
        {
            player.freeSettlements--;
        } else
        {
            player.RemoveResources(1, 1, 1, 1, 0);
        }

        player.storeSettlementNum--;

        // If this was the second turn, give resources related to tiles surrounding this settlement to player.
        if(GameManager.Instance.GetTurnCycle() == 2)
        {
            foreach(Face face in boardGrid.GetFacesFromVertexCoordinate(col, row, (BoardGrid.VertexSpecifier)vertexSpec))
            {
                if(face.tile != null)
                {
                    player.AddResource(face.tile.resourceType, 1);
                }
            }
        }

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    /**
     * This player places a city where a settlement is.
     */
    [Command]
    public void CmdPlaceCity(int col, int row, int vertexSpec)
    {
        Player player = GameManager.Instance.GetPlayerById(playerBehaviour.netId + "");

        Vertex vertex = GameManager.Instance.GetGame().boardHandler.GetBoardGrid().GetVertex(col, row, (BoardGrid.VertexSpecifier)vertexSpec);
        Settlement settlement = new Settlement();
        settlement.ownerId = player.GetId();
        settlement.isCity = true;
        vertex.settlement = settlement;
        player.RemoveResources(0, 0, 3, 0, 2);

        player.storeCityNum--;
        player.storeSettlementNum++;

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    /**
     * This player places a road on an edge.
     */
    [Command]
    public void CmdPlaceRoad(int col, int row, int edgeSpec)
    {
        Player player = GameManager.Instance.GetPlayerById(playerBehaviour.netId + "");

        Edge edge = GameManager.Instance.GetGame().boardHandler.GetBoardGrid().GetEdge(col, row, (BoardGrid.EdgeSpecifier)edgeSpec);
        Road road = new Road();
        road.ownerId = player.GetId();
        edge.road = road;
        if (player.freeRoads >= 1)
        {
            player.freeRoads--;
        }
        else
        {
            player.RemoveResources(1, 0, 0, 1, 0);
        }

        player.storeRoadNum--;

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    /**
     * This player ends their turn, and updates the Game Manager's turn index.
     */
    [Command]
    public void CmdEndTurn()
    {
        // Update turn index
        GameManager.Instance.turnIndex = GameManager.Instance.GetNextTurnIndex();
        GameManager.Instance.turnCount++;

        // Set next player's state to rolling
        Player nextPlayer = null;
        if(GameManager.Instance.GetTurnCycle() == 2)
        {
            nextPlayer = GameManager.Instance.GetGame().GetPlayerAtIndex(GameManager.Instance.GetGame().players.Count - 1 - GameManager.Instance.turnIndex);
        } else
        {
            nextPlayer = GameManager.Instance.GetGame().GetPlayerAtIndex(GameManager.Instance.turnIndex);
        }
        nextPlayer.state = PlayerState.ROLLING;

        // If still within the first two turn cycles, give a free settlement and road to the next player
        if (GameManager.Instance.GetTurnCycle() <= 2)
        {
            nextPlayer.freeRoads++;
            nextPlayer.freeSettlements++;
            nextPlayer.state = PlayerState.SETUP;
        }

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdRollDice()
    {
        // Roll
        System.Random random = new System.Random();
        int roll = random.Next(1, 7) + random.Next(1, 7);
        GameManager.Instance.recentRoll = roll;

        // Update this player's state
        Player thisPlayer = GameManager.Instance.GetPlayerById(playerBehaviour.netId + "");
        if (roll == 7)
        {
            thisPlayer.state = PlayerState.ROBBING;
        }
        else
        {
            thisPlayer.state = PlayerState.TRADING;
        }

        // Produce resources for each player
        BoardGrid boardGrid = GameManager.Instance.GetGame().GetBoardHandler().GetBoardGrid();
        for (int col = 0; col < boardGrid.GetColCount(); col++)
        {
            for (int row = 0; row < boardGrid.GetRowCount(); row++)
            {
                Face face = boardGrid.GetFace(col, row);
                if (face != null && face.tile != null && face.tile.chanceValue == roll) // If tile exists, and roll
                {
                    List<Vertex> vertices = boardGrid.GetVerticesFromFace(col, row);
                    foreach (Vertex vertex in vertices) // Get all vertexes of this tile
                    {
                        if (vertex.settlement != null)
                        {
                            Player player = GameManager.Instance.GetPlayerById(vertex.settlement.ownerId);
                            player.AddResource(face.tile.resourceType, vertex.settlement.isCity ? 2 : 1); // Give two resources if city, one if settlement
                        }
                    }
                }
            }
        }

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdMoveToBuildingPhase()
    {
        Player thisPlayer = GameManager.Instance.GetPlayerById(playerBehaviour.netId + "");
        thisPlayer.state = PlayerState.BUILDING;

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    // Developer commands
    [Command]
    public void CmdDevGiveEachPlayerResources(int resourceAmount)
    {
        foreach (Player player in GameManager.Instance.GetGame().players)
        {
            Debug.Log("Giving resource to " + player.GetId());
            player.AddResources(resourceAmount, resourceAmount, resourceAmount, resourceAmount, resourceAmount);
        }
        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdDevGiveEachPlayerFreeRoadsAndSettlements()
    {
        foreach (Player player in GameManager.Instance.GetGame().players)
        {
            Debug.Log("Giving resource to " + player.GetId());
            player.freeSettlements++;
            player.freeRoads++;
        }
        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdRegisterNewPlayer()
    {
        GameManager.Instance.GetGame().players.Add(new Player(netId + ""));

        // If it is the first player, give a free road and settlement
        if(GameManager.Instance.GetGame().GetPlayerIndexById(netId+"") == 0)
        {
            Player playerOne = GameManager.Instance.GetGame().players[0];
            playerOne.state = PlayerState.SETUP;
            playerOne.freeRoads++;
            playerOne.freeSettlements++;
        }

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdRobPlayer(string playerId)
    {
        Player targetPlayer = GameManager.Instance.GetPlayerById(playerId);
        int resourceIndex = UnityEngine.Random.Range(0, targetPlayer.resources.Count);

        // Steal the resource
        ResourceType resource = targetPlayer.resources[resourceIndex];
        targetPlayer.resources.RemoveAt(resourceIndex);
        playerBehaviour.GetPlayer().resources.Add(resource);
        playerBehaviour.GetPlayer().state = PlayerState.BUILDING;

        GameManager.Instance.SetDirtyBit(0b11111111);
    }
}
