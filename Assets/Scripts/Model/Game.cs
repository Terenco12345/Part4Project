using System.Collections.Generic;

public class Game
{
    public static readonly int DEFAULT_FOREST_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_HILL_TILE_AMOUNT = 3;
    public static readonly int DEFAULT_MEADOW_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_MOUNTAIN_TILE_AMOUNT = 3;
    public static readonly int DEFAULT_FIELD_TILE_AMOUNT = 4;
    public static readonly int DEFAULT_DESERT_TILE_AMOUNT = 1;

    BoardHandler boardHandler;
    List<Player> players;

    public Game()
    {
        boardHandler = new BoardHandler();
        players = new List<Player>();

        boardHandler.CreateSettlersBoard();
        boardHandler.CreateTiles();
        boardHandler.SetupTileResourceTypes();
        boardHandler.SetupChanceTokens();
        boardHandler.SetupRobberAtDesert();
    }

    public BoardHandler GetBoardHandler()
    {
        return boardHandler;
    }

    public List<Player> GetPlayers()
    {
        return players;
    }
}
