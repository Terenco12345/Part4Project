using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Mirror;

public class PlayerTradeController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;

    [Command]
    public void CmdStartTrade(string playerId)
    {
        Player thisPlayer = playerBehaviour.GetPlayer();
        Player targetPlayer = GameManager.Instance.GetPlayerById(playerId);

        PlayerTradeManager tradeManager = PlayerTradeManager.Instance;

        // Start a trade if there is no trade occuring right now.
        if (!tradeManager.trading)
        {
            tradeManager.trading = true;
            tradeManager.offererId = thisPlayer.GetId();
            tradeManager.receiverId = targetPlayer.GetId();
        }
    }

    [Command]
    public void CmdToggleAcceptAsReceiver(bool value)
    {
        PlayerTradeManager.Instance.receiverAccept = value;
    }

    [Command]
    public void CmdToggleAcceptAsOfferer(bool value)
    {
        PlayerTradeManager.Instance.offererAccept = value;
    }

    [Command]
    public void CmdAttemptTrade()
    {
        PlayerTradeManager tradeManager = PlayerTradeManager.Instance;
        if (tradeManager.ShouldTradeResolve())
        {
            // Give everyone the resources they need
            Player receiver = GameManager.Instance.GetPlayerById(tradeManager.receiverId);
            Player offerer = GameManager.Instance.GetPlayerById(tradeManager.offererId);

            offerer.AddResources(tradeManager.receiverLumber, tradeManager.receiverWool, tradeManager.receiverGrain, tradeManager.receiverBrick, tradeManager.receiverOre);
            receiver.AddResources(tradeManager.offererLumber, tradeManager.offererWool, tradeManager.offererGrain, tradeManager.offererBrick, tradeManager.offererOre);

            offerer.RemoveResources(tradeManager.offererLumber, tradeManager.offererWool, tradeManager.offererGrain, tradeManager.offererBrick, tradeManager.offererOre);
            receiver.RemoveResources(tradeManager.receiverLumber, tradeManager.receiverWool, tradeManager.receiverGrain, tradeManager.receiverBrick, tradeManager.receiverOre);
            
            PlayerTradeManager.Instance.ResetTrade();
        }

        GameManager.Instance.SetDirtyBit(0b11111111);
    }

    [Command]
    public void CmdCancelTrade()
    {
        PlayerTradeManager tradeManager = PlayerTradeManager.Instance;
        tradeManager.ResetTrade();
    }

    [Command]
    public void CmdChangeTradeAmount(int resourceType, int amount)
    {
        Player player = playerBehaviour.GetPlayer();
        PlayerTradeManager.Instance.ChangeTradeAmount(player.GetId(), resourceType, amount);

        // Unaccept all verification
        PlayerTradeManager.Instance.offererAccept = false;
        PlayerTradeManager.Instance.receiverAccept = false;
    }
}
