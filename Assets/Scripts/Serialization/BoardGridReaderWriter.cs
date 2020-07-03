using UnityEngine;
using System.Collections;
using Mirror;

public static class BoardGridReaderWriter
{
    public static void WriteBoard(this NetworkWriter writer, BoardGrid boardGrid)
    {
        writer.WriteInt32(boardGrid.GetColCount());
        writer.WriteInt32(boardGrid.GetRowCount());
        for (int col = 0; col < boardGrid.GetColCount(); col++)
        {
            for (int row = 0; row < boardGrid.GetRowCount(); row++)
            {
                // Serialize col, row face
                Face face = boardGrid.GetFace(col, row);

                if (face == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (face.tile == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(face.tile.ownerId);
                    writer.WriteInt32((int)face.tile.resourceType);
                    writer.WriteInt32(face.tile.chanceValue);
                }

                // Serialize col, row vertices L and R
                Vertex vertexL = boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.L);
                Vertex vertexR = boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.R);

                if (vertexL == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (vertexL.settlement == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(vertexL.settlement.ownerId);
                    writer.WriteBoolean(vertexL.settlement.isCity);
                }

                if (vertexR == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (vertexR.settlement == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(vertexR.settlement.ownerId);
                    writer.WriteBoolean(vertexR.settlement.isCity);
                }

                // Serialize col, row edges W, N and E
                Edge edgeW = boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.W);
                Edge edgeN = boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.N);
                Edge edgeE = boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.E);

                if (edgeW == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (edgeW.road == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(edgeW.road.ownerId);
                }

                if (edgeN == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (edgeN.road == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(edgeN.road.ownerId);
                }

                if (edgeE == null)
                {
                    writer.WriteBoolean(false);
                }
                else if (edgeE.road == null)
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(false);
                }
                else
                {
                    writer.WriteBoolean(true);
                    writer.WriteBoolean(true);
                    writer.WriteString(edgeE.road.ownerId);
                }
            }
        }
    }

    public static BoardGrid ReadBoard(NetworkReader reader)
    {
        int cols = reader.ReadInt32();
        int rows = reader.ReadInt32();
        BoardGrid boardGrid = new BoardGrid(cols, rows);
        for (int col = 0; col < cols; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                // Face exists?
                if (reader.ReadBoolean())
                {
                    // Face col, row
                    boardGrid.CreateFace(col, row);
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetFace(col, row).tile = new Tile();
                        boardGrid.GetFace(col, row).tile.ownerId = reader.ReadString();
                        boardGrid.GetFace(col, row).tile.resourceType = (ResourceType)reader.ReadInt32();
                        boardGrid.GetFace(col, row).tile.chanceValue = reader.ReadInt32();
                    }
                }

                // Vertex L exists?
                if (reader.ReadBoolean())
                {
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.L).settlement = new Settlement();
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.L).settlement.ownerId = reader.ReadString();
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.L).settlement.isCity = reader.ReadBoolean();
                    }
                }

                // Vertex R exists?
                if (reader.ReadBoolean())
                {
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.R).settlement = new Settlement();
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.R).settlement.ownerId = reader.ReadString();
                        boardGrid.GetVertex(col, row, BoardGrid.VertexSpecifier.R).settlement.isCity = reader.ReadBoolean();
                    }
                }

                // Edge W exists?
                if (reader.ReadBoolean())
                {
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.W).road = new Road();
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.W).road.ownerId = reader.ReadString();
                    }
                }

                // Edge N exists?
                if (reader.ReadBoolean())
                {
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.N).road = new Road();
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.N).road.ownerId = reader.ReadString();
                    }
                }

                // Edge E exists?
                if (reader.ReadBoolean())
                {
                    if (reader.ReadBoolean())
                    {
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.E).road = new Road();
                        boardGrid.GetEdge(col, row, BoardGrid.EdgeSpecifier.E).road.ownerId = reader.ReadString();
                    }
                }
            }
        }

        return boardGrid;
    }
}