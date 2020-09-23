using System.Collections.Generic;
using Mirror;


public class Game
{
    public static readonly int DEFAULT_BOARD_GRID_COLS = 10;
    public static readonly int DEFAULT_BOARD_GRID_ROWS = 10;

    public static readonly int DEFAULT_FOREST_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_HILL_TILE_AMOUNT = 3;
    public static readonly int DEFAULT_MEADOW_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_MOUNTAIN_TILE_AMOUNT = 3;
    public static readonly int DEFAULT_FIELD_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_DESERT_TILE_AMOUNT = 1;

    public BoardHandler boardHandler;
    public List<Player> players;

    public Game()
    {
        boardHandler = new BoardHandler();
        players = new List<Player>();

        boardHandler.CreateSettlersBoard();
    }

    public void SetupGame()
    {
        boardHandler.CreateTiles();
        if (GameConfigManager.Instance.resourceLoadout == null || GameConfigManager.Instance.resourceLoadout.Count == 0)
        {
            boardHandler.SetupTileResourceTypesRandom();
        }
        else
        {
            boardHandler.SetupTileResourceTypes(GameConfigManager.Instance.resourceLoadout);
        }
        
        boardHandler.SetupChanceTokens();
        boardHandler.SetupRobberAtDesert();
    }

    public BoardHandler GetBoardHandler()
    {
        return boardHandler;
    }

    public void SetBoardHandler(BoardHandler boardHandler)
    {
        this.boardHandler = boardHandler;
    }

    /**
     * Return the player's number based on their ID. This is used to determine player order, and their color.
     */
    public int GetPlayerIndexById(string id)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (players[i].GetId().Equals(id))
            {
                return i;
            }
        }
        return -1;
    }
    
    /**
     * Return a player in a given position.
     */
    public Player GetPlayerAtIndex(int i)
    {
        return players[i];
    }
}