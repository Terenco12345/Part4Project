using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Text inspectionText;
    public Text playerResourcesText;
    public Text rollText;

    public Button resourceDebugButton;
    public Button devResourceButton;
    public Button giveFreeSettlementsAndRoadsButton;

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
                inspectionText.gameObject.SetActive(true);
                playerResourcesText.gameObject.SetActive(true);
                rollText.gameObject.SetActive(true);

                resourceDebugButton.gameObject.SetActive(true);
                devResourceButton.gameObject.SetActive(true);
                giveFreeSettlementsAndRoadsButton.gameObject.SetActive(true);

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
            }
        }
        else
        {
            inspectionText.gameObject.SetActive(false);
            playerResourcesText.gameObject.SetActive(false);
            rollText.gameObject.SetActive(false);

            resourceDebugButton.gameObject.SetActive(false);
            devResourceButton.gameObject.SetActive(false);
            giveFreeSettlementsAndRoadsButton.gameObject.SetActive(false);
        }
    }

    public void DevGiveResources()
    {
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdDevGiveEachPlayerResources(5);
    }

    public void DevGiveFreeRoadsAndSettlements()
    {
        GameManager.Instance.GetLocalPlayer().GetComponent<PlayerController>().CmdDevGiveEachPlayerFreeRoadsAndSettlements();
    }
}
