using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCustomizationController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;
    
    [Command]
    public void CmdChangeSelection(int selection)
    {
        ModelManager.Instance.tileSetSelection = selection;
    }
}
