using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryPointManager : MonoBehaviour
{
    public void Start()
    {
        
    }

    /**
     * Obtain the victory points for a player, given their ID.
     */
    public int GetVictoryPointsForPlayerId(string id)
    {
        BoardGrid boardGrid = GetComponent<GameManager>().GetGame().GetBoardHandler().GetBoardGrid();
        int victoryPoints = 0;
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
        return victoryPoints;
    }
}
