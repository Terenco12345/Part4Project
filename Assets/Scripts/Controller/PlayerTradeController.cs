using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Mirror;

public class PlayerTradeController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;

    [Command]
    public void CmdChangeTradeAmount(int resourceType, int amount)
    {
        Player player = playerBehaviour.GetPlayer();
        PlayerTradeManager.Instance.ChangeTradeAmount(player.GetId(), resourceType, amount);
    }
}
