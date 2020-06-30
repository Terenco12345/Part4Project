using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    Game game;
    List<GameObject> players = new List<GameObject>();

    public GameObject playerPrefab;

    // Start is called before the first frame update
    public override void OnStartServer()
    {
        Debug.Log("Game manager started.");
        game = new Game();
    }

    public Game GetGame()
    {
        return game;
    }

    public void AddPlayer(GameObject playerObject)
    {
        players.Add(playerObject);

        Player player = new Player("Red");
        playerObject.GetComponent<PlayerBehaviour>().SetPlayer(player);
        game.GetPlayers().Add(player);
        
    }

    public Player GetLocalPlayer()
    {
        return null;
    }
}
