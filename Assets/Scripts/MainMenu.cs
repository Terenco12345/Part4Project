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
    public TelepathyTransport telepathyTransport;
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
        try
        {
            string port = hostPortText.text;
            telepathyTransport.port = ushort.Parse(port.Trim((char)8203));

            networkManager.StartHost();
        }
        catch (Exception e)
        {
            errorText.text = "Hosting failed! Please enter a valid port.";
        }
    }

    public void JoinGame()
    {
        try
        {
            string ip = joinIPText.text;
            string port = joinPortText.text;

            networkManager.networkAddress = ip.Trim((char)8203);
            telepathyTransport.port = ushort.Parse(port.Trim((char)8203));

            networkManager.StartClient();
        }
        catch (Exception e)
        {
            errorText.text = "Joining failed! Please enter a valid IP and port.";
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
