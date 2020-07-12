using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public bool devMode = false;

    public Text inspectionText;
    public Text playerResourcesText;
    public Text rollText;
    public Text playerStateText;

    
    public Button devResourceButton;
    public Button giveFreeSettlementsAndRoadsButton;

    public Button endTurnButton;
    public Button endPhaseButton;
    public Button diceRollButton;

    public NotificationTextBehaviour notificationText;

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

        // Player Resources Display
        string resourcesString = "";
        // Obtain local player
        GameObject playerObject = GameManager.Instance.GetLocalPlayer();
        if (playerObject != null)
        {
            Player player = playerObject.GetComponent<PlayerBehaviour>().GetPlayer();

            if (player != null)
            {
                // Text
                inspectionText.gameObject.SetActive(true);
                playerResourcesText.gameObject.SetActive(true);
                rollText.gameObject.SetActive(true);
                playerStateText.gameObject.SetActive(true);

                // Dev buttons
                if (devMode)
                {
                    devResourceButton.gameObject.SetActive(true);
                    giveFreeSettlementsAndRoadsButton.gameObject.SetActive(true);
                } else
                {
                    devResourceButton.gameObject.SetActive(false);
                    giveFreeSettlementsAndRoadsButton.gameObject.SetActive(false);
                }

                // Player action buttons - Only shown when it is the local player's turn
                if (GameManager.Instance.IsPlayerTurn(player))
                {
                    diceRollButton.gameObject.SetActive(false);
                    endPhaseButton.gameObject.SetActive(false);
                    endTurnButton.gameObject.SetActive(false);
                    switch (player.state)
                    {
                        case PlayerState.SETUP:
                            endTurnButton.gameObject.SetActive(true);
                            break;
                        case PlayerState.ROLLING:
                            diceRollButton.gameObject.SetActive(true);
                            break;
                        case PlayerState.ROBBING:
                            endPhaseButton.gameObject.SetActive(true);
                            break;
                        case PlayerState.BUILDING:
                            endTurnButton.gameObject.SetActive(true);
                            break;
                        case PlayerState.TRADING:
                            endPhaseButton.gameObject.SetActive(true);
                            break;
                    }
                } else
                {
                    endTurnButton.gameObject.SetActive(false);
                    diceRollButton.gameObject.SetActive(false);
                }

                int lumber = player.GetResourceCount(ResourceType.Lumber);
                int wool = player.GetResourceCount(ResourceType.Wool);
                int brick = player.GetResourceCount(ResourceType.Brick);
                int grain = player.GetResourceCount(ResourceType.Grain);
                int ore = player.GetResourceCount(ResourceType.Ore);

                resourcesString += "Free Roads: " + player.freeRoads + "\n";
                resourcesString += "Free Settlements: " + player.freeSettlements + "\n\n";

                resourcesString += "Lumber: " + lumber + "\n";
                resourcesString += "Wool: " + wool + "\n";
                resourcesString += "Brick: " + brick + "\n";
                resourcesString += "Grain: " + grain + "\n";
                resourcesString += "Ore: " + ore + "\n";

                playerResourcesText.text = resourcesString;

                if (GameManager.Instance.IsPlayerTurn(player))
                {
                    playerStateText.text = player.state.ToString();
                } else
                {
                    playerStateText.text = "";
                }
            }
        }
        else
        {
            inspectionText.gameObject.SetActive(false);
            playerResourcesText.gameObject.SetActive(false);
            rollText.gameObject.SetActive(false);
            playerStateText.gameObject.SetActive(false);

            endTurnButton.gameObject.SetActive(false);
            endPhaseButton.gameObject.SetActive(false);

            diceRollButton.gameObject.SetActive(false);
            devResourceButton.gameObject.SetActive(false);
            giveFreeSettlementsAndRoadsButton.gameObject.SetActive(false);
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
