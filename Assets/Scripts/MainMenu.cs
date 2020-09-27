using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class MainMenu : MonoBehaviour
{
    public NetworkManager networkManager;
    public GameObject mainMenu;

    [Header("Join")]
    public TextMeshProUGUI joinIPText;
    [Header("Error")]
    public TextMeshProUGUI errorText;

    public void HostGame()
    {
        networkManager.StartHost();
    }

    public void JoinGame()
    {
        string ip = joinIPText.text;

        networkManager.networkAddress = ip.Trim((char)8203);
        networkManager.StartClient();
    }

    public void ChangeSelection(Int32 selection)
    {
        GameConfigManager.Instance.ChangeLoadout(selection);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("has quit game");
    }
}
