using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VictoryPointManager : NetworkBehaviour
{
    // Singleton
    public static VictoryPointManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SyncVar]
    public int victoryPointWinRequirement = 10;

    [SyncVar]
    public bool hasSomeoneWon = false;

    [SyncVar]
    public string winnerId = "";

    /**
     * Obtain the victory points for a player, given their ID.
     */
    public int GetVictoryPointsForPlayerId(string id)
    {
        BoardGrid boardGrid = GameManager.Instance.GetGame().GetBoardHandler().GetBoardGrid();
        int victoryPoints = 0;
        
        // Building related victory points
        for(int col = 0; col < boardGrid.GetColCount(); col++)
        {
            for(int row = 0; row < boardGrid.GetRowCount(); row++)
            {
                for(int spec = 0; spec < 2; spec++)
                {
                    Vertex vertex = boardGrid.GetVertex(col, row, (BoardGrid.VertexSpecifier) spec);
                    if (vertex != null && vertex.settlement != null)
                    {
                        if (vertex.settlement.ownerId == id)
                        {
                            if (vertex.settlement.isCity)
                            {
                                victoryPoints += 2;
                            }
                            else
                            {
                                victoryPoints += 1;
                            }
                        }
                    }

                }
            }
        }

        // Longest Road related victory points

        // Largest Army related victory points

        return victoryPoints;
    }
}
