using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerCustomizationController : NetworkBehaviour
{
    public PlayerBehaviour playerBehaviour;
    
    public void ChangeTilesetSelection(int selection)
    {
        ModelManager.Instance.tileSetSelection = selection;
        ModelManager.Instance.UpdateTiles();
    }

    public void ChangeEnvironmentSelection(int selection)
    {
        ModelManager.Instance.environmentSelection = selection;
        ModelManager.Instance.UpdateEnvironment();
    }
}
