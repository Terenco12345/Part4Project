using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static KeyCode PLACEMENT_KEY = KeyCode.Mouse0;

    public Color[] playerColors;
    public Vector3[] playerPositions;
    public Vector3[] playerRotations;

    public Game game = new Game();
    public GameObject localPlayer = null;

    public GameObject playerPrefab;
    public GameObject board;
    public Camera mainCamera;
    public GUIManager guiManager;

    // Singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public override void OnStartServer()
    {
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

    public GameObject GetLocalPlayer()
    {
        return localPlayer;
    }

    public Player GetPlayerById(string id)
    {
        foreach(Player player in game.players)
        {
            if (player.GetId().Equals(id))
            {
                return player;
            }
        }

        return null;
    }

    public override bool OnSerialize(NetworkWriter writer, bool initialState)
    {
        Debug.Log("Serializing...");
        if (initialState)
        {
            ListPlayerReaderWriter.WriteListPlayer(writer, game.players);
            BoardHandlerReaderWriter.WriteBoard(writer, game.boardHandler);
            return true;
        }

        bool wroteSyncVar = false;
        if((base.syncVarDirtyBits & 1u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            ListPlayerReaderWriter.WriteListPlayer(writer, game.players);
        }

        if ((base.syncVarDirtyBits & 2u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            BoardHandlerReaderWriter.WriteBoard(writer, game.boardHandler);
        }
        if (!wroteSyncVar)
        {
            // write zero dirty bits if nothing changed
            writer.WritePackedUInt64(0u);
        }
        PrintBoardToConsole();
        return wroteSyncVar;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        Debug.Log("Deserializing...");
        game = new Game();
        if (initialState)
        {
            game.players = ListPlayerReaderWriter.ReadListPlayer(reader);
            game.boardHandler = BoardHandlerReaderWriter.ReadBoard(reader);
            return;
        }

        ulong dirtyBits = reader.ReadPackedUInt64();
        if ((dirtyBits & 1u) != 0u)
        {
            game.players = ListPlayerReaderWriter.ReadListPlayer(reader);
        }
        if ((dirtyBits & 2u) != 0u)
        {
            game.boardHandler = BoardHandlerReaderWriter.ReadBoard(reader);
        }
        PrintBoardToConsole();
    }

    public void PrintBoardToConsole()
    {
        Debug.Log(game.boardHandler.GetBoardGrid().ToString());
    }
}