﻿using JetBrains.Annotations;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public bool devMode = false;

    public Text lumberText;
    public Text brickText;
    public Text woolText;
    public Text grainText;
    public Text oreText;

    public Text inspectionText;
    public Text rollText;

    public Button endTurnButton;
    public Button endPhaseButton;
    public Button diceRollButton;

    public NotificationTextBehaviour notificationText;

    public GameObject freeSettlementNotification;
    public GameObject freeRoadNotification;

    public GameObject tradeWindow;

    public WinDisplayBehaviour winDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(GameManager.Instance == null)
        {
            return;
        }

        if (VictoryPointManager.Instance.hasSomeoneWon)
        {
            winDisplay.gameObject.SetActive(true);
        } else
        {
            winDisplay.gameObject.SetActive(false);
        }

        // Player Resources Display
        // Obtain local player
        GameObject playerObject = GameManager.Instance.GetLocalPlayer();
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<PlayerBehaviour>().GetPlayer();

            if (player != null)
            {
                // Render the trade window if this local player is in a trade.
                if (PlayerTradeManager.Instance.trading 
                    && (PlayerTradeManager.Instance.receiverId.Equals(player.GetId()) || PlayerTradeManager.Instance.offererId.Equals(player.GetId())))
                {
                    if (!tradeWindow.activeSelf) {
                        tradeWindow.GetComponent<TradeWindowRenderer>().topPlayerTradeDisplay.confirmCheckbox.GetComponent<Toggle>().isOn = false;
                        tradeWindow.GetComponent<TradeWindowRenderer>().bottomPlayerTradeDisplay.confirmCheckbox.GetComponent<Toggle>().isOn = false;
                        tradeWindow.SetActive(true); 
                    };
                } else
                {
                    if (tradeWindow.activeSelf) { tradeWindow.SetActive(false); };
                }

                // Text
                if (!inspectionText.IsActive()) { inspectionText.gameObject.SetActive(true); };

                // Resource Display
                if (!lumberText.IsActive()) { lumberText.gameObject.SetActive(true); };
                if (!brickText.IsActive()) { brickText.gameObject.SetActive(true); };
                if (!woolText.IsActive()) { woolText.gameObject.SetActive(true); };
                if (!grainText.IsActive()) { grainText.gameObject.SetActive(true); };
                if (!oreText.IsActive()) { oreText.gameObject.SetActive(true); };

                // Most recent roll
                if (!rollText.IsActive()) { rollText.gameObject.SetActive(true); };

                // Update the resource counters
                int lumber = player.GetResourceCount(ResourceType.Lumber);
                int wool = player.GetResourceCount(ResourceType.Wool);
                int brick = player.GetResourceCount(ResourceType.Brick);
                int grain = player.GetResourceCount(ResourceType.Grain);
                int ore = player.GetResourceCount(ResourceType.Ore);

                lumberText.text = lumber + "";
                woolText.text = wool + "";
                brickText.text = brick + "";
                grainText.text = grain + "";
                oreText.text = ore + "";

                // Player action buttons - Only shown when it is the local player's turn
                if (GameManager.Instance.IsPlayerTurn(player))
                {
                    switch (player.state)
                    {
                        case PlayerState.SETUP:
                            diceRollButton.gameObject.SetActive(false);
                            endPhaseButton.gameObject.SetActive(false);
                            if (!endTurnButton.IsActive()) { endTurnButton.gameObject.SetActive(true); };
                            break;
                        case PlayerState.ROLLING:
                            endPhaseButton.gameObject.SetActive(false);
                            endTurnButton.gameObject.SetActive(false);
                            if (!diceRollButton.IsActive()) { diceRollButton.gameObject.SetActive(true); };
                            break;
                        case PlayerState.ROBBING:
                            diceRollButton.gameObject.SetActive(false);
                            endTurnButton.gameObject.SetActive(false);
                            endPhaseButton.gameObject.SetActive(false);
                            break;
                        case PlayerState.BUILDING:
                            diceRollButton.gameObject.SetActive(false);
                            endPhaseButton.gameObject.SetActive(false);
                            if (!endTurnButton.IsActive()) { endTurnButton.gameObject.SetActive(true); };
                            break;
                        case PlayerState.TRADING:
                            diceRollButton.gameObject.SetActive(false);
                            endTurnButton.gameObject.SetActive(false);
                            if (!endPhaseButton.IsActive()) { endPhaseButton.gameObject.SetActive(true); };
                            break;
                    }

                    // Notify the player if they have free settlements or roads
                    if (player.freeRoads >= 1)
                    {
                        if (!freeRoadNotification.activeSelf)
                        {
                            freeRoadNotification.SetActive(true);
                        }
                    } else
                    {
                        if (freeRoadNotification.activeSelf)
                        {
                            freeRoadNotification.SetActive(false);
                        }
                    }

                    if (player.freeSettlements >= 1)
                    {
                        if (!freeSettlementNotification.activeSelf)
                        {
                            freeSettlementNotification.SetActive(true);
                        }
                    }
                    else
                    {
                        if (freeSettlementNotification.activeSelf)
                        {
                            freeSettlementNotification.SetActive(false);
                        }
                    }
                } else
                {
                    endTurnButton.gameObject.SetActive(false);
                    diceRollButton.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            inspectionText.gameObject.SetActive(false);

            lumberText.gameObject.SetActive(false);
            brickText.gameObject.SetActive(false);
            woolText.gameObject.SetActive(false);
            grainText.gameObject.SetActive(false);
            oreText.gameObject.SetActive(false);

            rollText.gameObject.SetActive(false);

            endTurnButton.gameObject.SetActive(false);
            endPhaseButton.gameObject.SetActive(false);

            diceRollButton.gameObject.SetActive(false);
        }

        if (GameManager.Instance.recentRoll == 0)
        {
            rollText.text = "There were no recent rolls.";
        } else
        {
            rollText.text = "Recent roll: "+GameManager.Instance.recentRoll;
        }
    }

    public void LocalPlayerMoveToBuildingPhase()
    {
        notificationText.DisplayText("Entering building phase...");
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdMoveToBuildingPhase();
    }

    public void LocalPlayerEndTurn()
    {
        // Shouldn't be able to end turn with free stuff
        Player player = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerBehaviour>().GetPlayer();
        if (player.freeRoads > 0 || player.freeSettlements > 0)
        {
            notificationText.DisplayText("You cannot end a turn when you still have free settlements or roads!");
        } else
        {
            notificationText.DisplayText("Turn ended.");
            GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdEndTurn();
        }
    }

    public void LocalPlayerRollDice()
    {
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdRollDice();
    }

    public void DevGiveResources()
    {
        notificationText.DisplayText("Giving resources to all players.");
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdDevGiveEachPlayerResources(5);
    }

    public void DevGiveFreeRoadsAndSettlements()
    {
        notificationText.DisplayText("Giving free roads and settlements to all players.");
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdDevGiveEachPlayerFreeRoadsAndSettlements();
    }
}
