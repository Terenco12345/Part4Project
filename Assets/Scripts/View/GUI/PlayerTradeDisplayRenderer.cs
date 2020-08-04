using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTradeDisplayRenderer : MonoBehaviour
{
    public Text playerName;
    public Text playerTradeStatus;

    public TradeResourceStatRenderer lumberRenderer;
    public TradeResourceStatRenderer brickRenderer;
    public TradeResourceStatRenderer woolRenderer;
    public TradeResourceStatRenderer grainRenderer;
    public TradeResourceStatRenderer oreRenderer;

    public GameObject confirmCheckbox;
}
