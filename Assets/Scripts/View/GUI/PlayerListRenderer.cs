using JetBrains.Annotations;
using Mirror.Examples.NetworkRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerListRenderer : MonoBehaviour
{
    public PlayerItemRenderer playerOneRenderer;
    public PlayerItemRenderer playerTwoRenderer;
    public PlayerItemRenderer playerThreeRenderer;
    public PlayerItemRenderer playerFourRenderer;

    // Update is called once per frame
    void Update()
    {
        List<PlayerItemRenderer> playerItems = new List<PlayerItemRenderer> { playerOneRenderer, playerTwoRenderer, playerThreeRenderer, playerFourRenderer };
        List<Player> players = GameManager.Instance.GetGame().players;

        // Check if robbing or trading indicator should be shown
        Player localPlayer = null;
        if(GameManager.Instance.GetLocalPlayer() != null)
        {
            localPlayer = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerBehaviour>().GetPlayer();
        }

        if(localPlayer != null)
        {
            // Check if player item should be rendererd
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (i >= players.Count)
                {
                    // Shouldn't render
                    if (playerItems[i].gameObject.activeSelf) { playerItems[i].gameObject.SetActive(false); }
                }
                else
                {
                    if (!playerItems[i].gameObject.activeSelf) { playerItems[i].gameObject.SetActive(true); }
                    if (GameManager.Instance.IsPlayerTurn(localPlayer) && !players[i].GetId().Equals(localPlayer.GetId()))
                    {
                        switch (localPlayer.state)
                        {
                            case PlayerState.SETUP:
                                // Should not be able to trade or rob
                                if (playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(false); }
                                if (playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(false); }
                                break;
                            case PlayerState.ROLLING:
                                // Should not be able to trade or rob
                                if (playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(false); }
                                if (playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(false); }
                                break;
                            case PlayerState.ROBBING:
                                // Should only be able to rob
                                if (playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(false); }
                                if (!playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(true); }
                                break;
                            case PlayerState.TRADING:
                                // Should only be able to trade
                                if (!playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(true); }
                                if (playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(false); }
                                break;
                            case PlayerState.BUILDING:
                                // Should not be able to trade or rob
                                if (playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(false); }
                                if (playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(false); }
                                break;
                        }
                    }
                    else
                    {
                        if (playerItems[i].tradeButton.gameObject.activeSelf) { playerItems[i].tradeButton.gameObject.SetActive(false); }
                        if (playerItems[i].robButton.gameObject.activeSelf) { playerItems[i].robButton.gameObject.SetActive(false); }
                    }
                }
            }
        }
    }

    public void RobPlayer(int index)
    {
        // Figure out the player id from the index
        List<Player> players = GameManager.Instance.GetGame().players;
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdRobPlayer(players[index].GetId());
    }

    public void TradeWithPlayer(int index)
    {
        // Figure out the player id from the index
        List<Player> players = GameManager.Instance.GetGame().players;

        // Open trade window
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>().CmdStartTrade(players[index].GetId());
    }
}
