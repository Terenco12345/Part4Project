using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public Text inspectionText;
    public Text playerResourcesText;
    public Text rollText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Player Resources Display
        string resourcesString = "";
        // Obtain local player
        Player player = GameManager.Instance.players[0];

        int lumber = player.GetResourceCount(ResourceType.Lumber);
        int wool = player.GetResourceCount(ResourceType.Wool);
        int brick = player.GetResourceCount(ResourceType.Brick);
        int grain = player.GetResourceCount(ResourceType.Grain);
        int ore = player.GetResourceCount(ResourceType.Ore);

        resourcesString += "Free Roads: " + player.freeRoads + "\n";
        resourcesString += "Free Settlements: " + player.freeSettlements + "\n\n";

        resourcesString += "Lumber: "+lumber+"\n";
        resourcesString += "Wool: " + wool + "\n";
        resourcesString += "Brick: " + brick + "\n";
        resourcesString += "Grain: " + grain + "\n";
        resourcesString += "Ore: " + ore + "\n";

        playerResourcesText.text = resourcesString;
    }
}
