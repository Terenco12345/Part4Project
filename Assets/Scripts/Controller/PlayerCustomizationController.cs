using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCustomizationController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;
    
    public void ChangeSelection(int selection)
    {
        ModelManager.Instance.tileSetSelection = selection;
        ModelManager.Instance.UpdateTiles();
    }
}
