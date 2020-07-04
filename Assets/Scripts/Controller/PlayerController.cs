using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
 * This class has commands which perform player actions.
 */
public class PlayerController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;

    [Command]
    public void CmdPlaceSettlement(int col, int row, int vertexSpec)
    {
        Player player = GameManager.Instance.GetPlayerById(playerBehaviour.netId+"");

        Vertex vertex = GameManager.Instance.GetGame().boardHandler.GetBoardGrid().GetVertex(col, row, (BoardGrid.VertexSpecifier)vertexSpec);
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

        GameManager.Instance.SetDirtyBit(0b1111);
    }

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

        GameManager.Instance.SetDirtyBit(0b1111);
    }

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

        GameManager.Instance.SetDirtyBit(0b1111);
    }

    [Command]
    public void CmdDevGiveEachPlayerResources(int resourceAmount)
    {
        foreach (Player player in GameManager.Instance.GetGame().players)
        {
            Debug.Log("Giving resource to " + player.GetId());
            player.AddResources(resourceAmount, resourceAmount, resourceAmount, resourceAmount, resourceAmount);
        }
        GameManager.Instance.SetDirtyBit(0b1111);
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
        GameManager.Instance.SetDirtyBit(0b1111);
    }

    [Command]
    public void CmdRegisterNewPlayer()
    {
        GameManager.Instance.GetGame().players.Add(new Player(netId + ""));
        GameManager.Instance.SetDirtyBit(0b1111);
    }
}
