using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/**
 * This game manager holds player and game information.
 */
public class GameManager : NetworkBehaviour
{
    public static KeyCode PLACEMENT_KEY = KeyCode.Mouse0;

    public Color[] playerColors;
    public Vector3[] playerPositions;
    public Vector3[] playerRotations;

    public Game game = new Game();
    public GameObject localPlayer = null;

    public int recentRoll = 0;
    public int turnIndex = 0;
    public int turnCount = 0;

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

    public int GetTurnCycle()
    {
        return (turnCount/ game.players.Count)+1;
    }

    /**
     * Obtains next number in the cycle of turns. E.g. If player 3 ends their turn, 
     * player 4 would go next if there are 4 or more players, or return to player 1 otherwise.
     */
    public int GetNextTurnIndex()
    {
        int nextTurn = 0;
        if(turnIndex >= game.players.Count-1)
        {
            nextTurn = 0;
        } else
        {
            nextTurn = turnIndex + 1;
        }
        Debug.Log(nextTurn);
        return nextTurn;
    }

    /**
     * Obtain current local player
     */
    public GameObject GetLocalPlayer()
    {
        return localPlayer;
    }

    /**
     * Is it this player's turn?
     */
    public bool IsPlayerTurn(Player player)
    {
        if (GetGame().players.Contains(player))
        {
            if (GetTurnCycle() == 2)
            {
                return GetGame().GetPlayerIndexById(player.GetId()) == GetGame().players.Count - 1 - (turnIndex);

            }
            else
            {
                return GetGame().GetPlayerIndexById(player.GetId()) == turnIndex;
            }
        }
        return false;
    }

    /**
     * Obtain a player through their ID
     */
    public Player GetPlayerById(string id)
    {
        foreach (Player player in game.players)
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
            writer.WriteInt32(recentRoll);
            return true;
        }

        bool wroteSyncVar = false;
        if ((base.syncVarDirtyBits & 1u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            ListPlayerReaderWriter.WriteListPlayer(writer, game.players); // Player list
        }

        if ((base.syncVarDirtyBits & 2u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            BoardHandlerReaderWriter.WriteBoard(writer, game.boardHandler); // Game board
        }

        if ((base.syncVarDirtyBits & 4u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.WriteInt32(recentRoll); // Recent roll
        }

        if ((base.syncVarDirtyBits & 8u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.WriteInt32(turnIndex); // Turn index
        }

        if ((base.syncVarDirtyBits & 16u) != 0u)
        {
            if (!wroteSyncVar)
            {
                writer.WritePackedUInt64(base.syncVarDirtyBits);
                wroteSyncVar = true;
            }
            writer.WriteInt32(turnCount); // Turn count
        }

        if (!wroteSyncVar)
        {
            // write zero dirty bits if nothing changed
            writer.WritePackedUInt64(0u);
        }
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
            recentRoll = reader.ReadInt32();
            return;
        }

        ulong dirtyBits = reader.ReadPackedUInt64();
        if ((dirtyBits & 1u) != 0u)
        {
            game.players = ListPlayerReaderWriter.ReadListPlayer(reader); // Player list
        }
        if ((dirtyBits & 2u) != 0u)
        {
            game.boardHandler = BoardHandlerReaderWriter.ReadBoard(reader); // Game board
        }
        if ((dirtyBits & 4u) != 0u)
        {
            recentRoll = reader.ReadInt32(); // Recent dice roll
        }
        if ((dirtyBits & 8u) != 0u)
        {
            turnIndex = reader.ReadInt32(); // Turn index
        }
        if ((dirtyBits & 16u) != 0u)
        {
            turnCount = reader.ReadInt32(); // Turn count
        }
    }

    public void PrintBoardToConsole()
    {
        Debug.Log(game.boardHandler.GetBoardGrid().ToString());
    }
}