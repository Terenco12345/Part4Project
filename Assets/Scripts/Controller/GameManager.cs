using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    Game game;
    List<GameObject> players = new List<GameObject>();

    public GameObject playerPrefab;
    public GameObject board;

    public override void OnStartServer()
    {
        game = new Game();
        game.SetupGame();
    }

    public Game GetGame()
    {
        return game;
    }

    public void SetGame(Game game)
    {
        this.game = game;
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

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        Debug.Log("Serializing game");
        List<Player> players = GetGame().GetPlayers();

        // Players
        writer.WriteInt32(players.Count);
        for (int i = 0; i < players.Count; i++)
        {
            players[i].Serialize(writer);
        }

        GetGame().GetBoardHandler().Serialize(writer);

        return true;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        Debug.Log("Deserializing game");
        Game newGame = new Game();

        int playerCount = reader.ReadInt32();
        for(int i = 0; i < playerCount; i++)
        {
            newGame.GetPlayers().Add(Player.Deserialize(reader));
        }
        newGame.SetBoardHandler(BoardHandler.Deserialize(reader));

        SetGame(new Game());
    }
}
