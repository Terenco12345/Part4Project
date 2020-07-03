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

    }

    [Command]
    public void CmdPlaceCity(int col, int row, int vertexSpec)
    {

    }

    [Command]
    public void CmdPlaceRoad(int col, int row, int edgeSpec)
    {

    }

    [Command]
    public void CmdDevGiveEachPlayerResources(int resourceAmount)
    {
        foreach (Player player in GameManager.Instance.GetGame().players)
        {
            Debug.Log("Giving resource to " + player.GetId());
            player.AddResources(resourceAmount, resourceAmount, resourceAmount, resourceAmount, resourceAmount);
        }
        GameManager.Instance.SetDirtyBit(0x1111);
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
        GameManager.Instance.SetDirtyBit(0x0011);
    }

    [Command]
    public void CmdRegisterNewPlayer()
    {
        GameManager.Instance.GetGame().players.Add(new Player(netId + ""));
        GameManager.Instance.SetDirtyBit(0x0011);
    }
}
