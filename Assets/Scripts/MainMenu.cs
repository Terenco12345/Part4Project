using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;

    [Header("Host")]
    public TextMeshProUGUI hostPortText;
    [Header("Join")]
    public TextMeshProUGUI joinIPText;
    public TextMeshProUGUI joinPortText;
    [Header("Error")]
    public TextMeshProUGUI errorText;
    
    public void HostGame()
    {
        string port = hostPortText.text;

        if(port != "")
        {
            NetworkManager.singleton.networkAddress = "127.0.0.1:" + port;
            NetworkManager.singleton.StartHost();
        } else
        {
            errorText.text = "Please enter a port.";
        }
    }

    public void JoinGame()
    {
        string ip = joinIPText.text;
        string port = joinPortText.text;

        if(ip != "" && port != "")
        {
            NetworkManager.singleton.networkAddress = ip + ":" + port;
            NetworkManager.singleton.StartClient();
        } else
        {
            errorText.text = "Please enter values for IP and Port.";
        }
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
