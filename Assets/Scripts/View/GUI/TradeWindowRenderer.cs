using JetBrains.Annotations;
using Mono.CecilX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradeWindowRenderer : MonoBehaviour
{
    // Singleton
    public static TradeWindowRenderer Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public PlayerTradeManager playerTradeManager;

    public PlayerTradeDisplayRenderer topPlayerTradeDisplay;
    public PlayerTradeDisplayRenderer bottomPlayerTradeDisplay;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GetLocalPlayer() != null)
        {
            Player player = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerBehaviour>().GetPlayer();

            if (playerTradeManager.offererId.Equals("") || playerTradeManager.receiverId.Equals(""))
            {
                // Trade window should be disabled.
            }
            else
            {
                // This player's resource values should be updated
                topPlayerTradeDisplay.lumberRenderer.text.text = playerTradeManager.receiverLumber + "";
                topPlayerTradeDisplay.brickRenderer.text.text = playerTradeManager.receiverBrick + "";
                topPlayerTradeDisplay.woolRenderer.text.text = playerTradeManager.receiverWool + "";
                topPlayerTradeDisplay.grainRenderer.text.text = playerTradeManager.receiverGrain + "";
                topPlayerTradeDisplay.oreRenderer.text.text = playerTradeManager.receiverOre + "";

                // Other player's resource values should be updated
                bottomPlayerTradeDisplay.lumberRenderer.text.text = playerTradeManager.offererLumber + "";
                bottomPlayerTradeDisplay.brickRenderer.text.text = playerTradeManager.offererBrick + "";
                bottomPlayerTradeDisplay.woolRenderer.text.text = playerTradeManager.offererWool + "";
                bottomPlayerTradeDisplay.grainRenderer.text.text = playerTradeManager.offererGrain + "";
                bottomPlayerTradeDisplay.oreRenderer.text.text = playerTradeManager.offererOre + "";

                if (player.GetId() == playerTradeManager.receiverId)
                {
                    // Enable everything for receiver side
                    if (!topPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(true); }
                    if (!topPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(true); }
                    if (!topPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(true); }
                    if (!topPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(true); }
                    if (!topPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(true); }
                    if (!topPlayerTradeDisplay.confirmCheckbox.activeSelf) { topPlayerTradeDisplay.confirmCheckbox.SetActive(true); }

                    // Disable everything for offerer side
                    if (bottomPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(false); }
                    if (bottomPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(false); }
                    if (bottomPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(false); }
                    if (bottomPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(false); }
                    if (bottomPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(false); }
                    if (bottomPlayerTradeDisplay.confirmCheckbox.activeSelf) { bottomPlayerTradeDisplay.confirmCheckbox.SetActive(false); }
                }
                else if (player.GetId() == playerTradeManager.offererId)
                {
                    // Enable everything for offerer side
                    if (topPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(false); }
                    if (topPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(false); }
                    if (topPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(false); }
                    if (topPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(false); }
                    if (topPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { topPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(false); }
                    if (topPlayerTradeDisplay.confirmCheckbox.activeSelf) { topPlayerTradeDisplay.confirmCheckbox.SetActive(false); }

                    // Disable everything for receiver side
                    if (!bottomPlayerTradeDisplay.lumberRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.lumberRenderer.upDownSelector.SetActive(true); }
                    if (!bottomPlayerTradeDisplay.brickRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.brickRenderer.upDownSelector.SetActive(true); }
                    if (!bottomPlayerTradeDisplay.woolRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.woolRenderer.upDownSelector.SetActive(true); }
                    if (!bottomPlayerTradeDisplay.grainRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.grainRenderer.upDownSelector.SetActive(true); }
                    if (!bottomPlayerTradeDisplay.oreRenderer.upDownSelector.activeSelf) { bottomPlayerTradeDisplay.oreRenderer.upDownSelector.SetActive(true); }
                    if (!bottomPlayerTradeDisplay.confirmCheckbox.activeSelf) { bottomPlayerTradeDisplay.confirmCheckbox.SetActive(true); }
                }
                else
                {
                    // Something is wrong!
                }
            }
        }

        // Try to get the player objects involved in the trade
        Player offererPlayer = GameManager.Instance.GetPlayerById(PlayerTradeManager.Instance.offererId);
        Player receiverPlayer = GameManager.Instance.GetPlayerById(PlayerTradeManager.Instance.receiverId);

        if (offererPlayer != null && receiverPlayer != null)
        {
            topPlayerTradeDisplay.playerName.text = "Receiver: Player ID "+receiverPlayer.GetId();
            topPlayerTradeDisplay.playerTradeStatus.text = PlayerTradeManager.Instance.receiverAccept ?
                "Player has confirmed the trade." : "Player has not confirmed the trade.";

            bottomPlayerTradeDisplay.playerName.text = "Offerer: Player ID "+offererPlayer.GetId();
            bottomPlayerTradeDisplay.playerTradeStatus.text = PlayerTradeManager.Instance.offererAccept ?
                "Player has confirmed the trade." : "Player has not confirmed the trade.";
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

    public void CheckboxOfferer()
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdToggleAcceptAsOfferer(bottomPlayerTradeDisplay.confirmCheckbox.GetComponent<Toggle>().isOn);
    }

    public void CheckboxReceiver()
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdToggleAcceptAsReceiver(topPlayerTradeDisplay.confirmCheckbox.GetComponent<Toggle>().isOn);
    }

    public void AcceptTrade()
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdAttemptTrade();
    }

    public void CancelTrade()
    {
        PlayerTradeController playerTradeController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerTradeController>();
        playerTradeController.CmdCancelTrade();
    }
}
