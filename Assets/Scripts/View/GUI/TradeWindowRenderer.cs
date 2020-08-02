using JetBrains.Annotations;
using Mono.CecilX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeWindowRenderer : MonoBehaviour
{
    public PlayerTradeManager playerTradeManager;

    public PlayerTradeDisplayRenderer thisPlayerTradeDisplay;
    public PlayerTradeDisplayRenderer otherPlayerTradeDisplay;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.GetLocalPlayer() != null)
        {
            Player player = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerBehaviour>().GetPlayer();

            if (playerTradeManager.offererId.Equals("") || playerTradeManager.receiverId.Equals(""))
            {
                // Trade window should be disabled.
            }
            else
            {
                // This player's resource values should be updated
                thisPlayerTradeDisplay.lumberRenderer.text.text = playerTradeManager.receiverLumber + "";
                thisPlayerTradeDisplay.brickRenderer.text.text = playerTradeManager.receiverBrick + "";
                thisPlayerTradeDisplay.woolRenderer.text.text = playerTradeManager.receiverWool + "";
                thisPlayerTradeDisplay.grainRenderer.text.text = playerTradeManager.receiverGrain + "";
                thisPlayerTradeDisplay.oreRenderer.text.text = playerTradeManager.receiverOre + "";

                // Other player's resource values should be updated
                otherPlayerTradeDisplay.lumberRenderer.text.text = playerTradeManager.offererLumber + "";
                otherPlayerTradeDisplay.brickRenderer.text.text = playerTradeManager.offererBrick + "";
                otherPlayerTradeDisplay.woolRenderer.text.text = playerTradeManager.offererWool + "";
                otherPlayerTradeDisplay.grainRenderer.text.text = playerTradeManager.offererGrain + "";
                otherPlayerTradeDisplay.oreRenderer.text.text = playerTradeManager.offererOre + "";

                if (player.GetId() == playerTradeManager.receiverId)
                {
                    // Enable everything for receiver side
                    if (!thisPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(true); }
                    if (!thisPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(true); }
                    if (!thisPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(true); }
                    if (!thisPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(true); }
                    if (!thisPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(true); }

                    // Disable everything for offerer side
                    if (otherPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(false); }
                    if (otherPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(false); }
                    if (otherPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(false); }
                    if (otherPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(false); }
                    if (otherPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(false); }
                }
                else if (player.GetId() == playerTradeManager.offererId)
                {
                    // Enable everything for offerer side
                    if (thisPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(false); }
                    if (thisPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(false); }
                    if (thisPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(false); }
                    if (thisPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(false); }
                    if (thisPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { thisPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(false); }

                    // Disable everything for receiver side
                    if (!otherPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(true); }
                    if (!otherPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(true); }
                    if (!otherPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(true); }
                    if (!otherPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(true); }
                    if (!otherPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { otherPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(true); }
                }
                else
                {
                    // Something is wrong!
                }
            }
        }
    }

    public void IncrementResource(int resourceType)
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdChangeTradeAmount(resourceType, 1);
    }

    public void DecrementResource(int resourceType)
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdChangeTradeAmount(resourceType, -1);
    }
}
