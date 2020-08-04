using System;
using System.Collections.Generic;
using Mirror;

/**
 * This class represents all grid related logic for the Catan game board.
 * 
 * Represents a hexagonal grid of hexagons, with their vertices and edges.
 * Grid is flat side top and bottom.
 * 
 *      ----
 *    /      \
 *    \      /
 *      ----
 * 
 * -x to +x = top row to bottom row
 * -z to +z = left col to right col
 * 
 * Heavily inspired by the hexagon system described in this paper.
 * http://www-cs-students.stanford.edu/~amitp/game-programming/grids/
 */
public class BoardGrid
{
    public enum VertexSpecifier
    {
        L = 0,
        R = 1
    }

    public enum EdgeSpecifier
    {
        N = 0,
        W = 1,
        E = 2
    }

    Face[,] gridFaces;
    Vertex[,,] gridVertices;
    Edge[,,] gridEdges;

    public BoardGrid(int cols, int rows)
    {
        gridFaces = new Face[cols, rows];
        gridVertices = new Vertex[cols, rows, 2];
        gridEdges = new Edge[cols, rows, 3];

        // Populate the grid with null
        for (int row = 0; row < gridFaces.GetLength(1); row++)
        {
            for (int col = 0; col < gridFaces.GetLength(0); col++)
            {
                // Faces
                gridFaces[col, row] = null;

                // Vertices
                gridVertices[col, row, (int)VertexSpecifier.L] = null;
                gridVertices[col, row, (int)VertexSpecifier.R] = null;

                // Edges
                gridEdges[col, row, (int)EdgeSpecifier.N] = null;
                gridEdges[col, row, (int)EdgeSpecifier.E] = null;
                gridEdges[col, row, (int)EdgeSpecifier.W] = null;
            }
        }
    }

    public int GetColCount()
    {
        return gridFaces.GetLength(0);
    }

    public int GetRowCount()
    {
        return gridFaces.GetLength(1);
    }

    public Face[,] GetFaces()
    {
        return gridFaces;
    }

    public Edge[,,] GetEdges()
    {
        return gridEdges;
    }

    public Vertex[,,] GetVertices()
    {
        return gridVertices;
    }

    /**
     * Obtain a face from this grid.
     */
    public Face GetFace(int col, int row)
    {
        return gridFaces[col, row];
    }

    /**
     * Obtain a vertex from this grid.
     */
    public Vertex GetVertex(int col, int row, VertexSpecifier vertexSpec)
    {
        return gridVertices[col, row, (int)vertexSpec];
    }

    /**
     * Obtain an edge from this grid.
     */
    public Edge GetEdge(int col, int row, EdgeSpecifier edgeSpec)
    {
        return gridEdges[col, row, (int)edgeSpec];
    }

    public int[] GetEdgeCoordinates(Edge edge)
    {
        for (int col = 0; col < gridEdges.GetLength(0); col++) 
        { 
            for(int row = 0; row < gridEdges.GetLength(1); row++) 
            {
                for(int edgeSpec = 0; edgeSpec < gridEdges.GetLength(2); edgeSpec++)
                {
                    if(gridEdges[col, row, edgeSpec] == edge)
                    {
                        return new int[] { col, row, edgeSpec };
                    }    
                }
            }
        }

        return null;
    }

    /**
     * Get faces as a list
     */
    public List<Face> GetFacesAsList()
    {
        List<Face> faces = new List<Face>();

        // Store all of the board grid's items in a single dimensional array here.
        for (int col = 0; col < gridFaces.GetLength(0); col++)
        {
            for (int row = 0; row < gridFaces.GetLength(1); row++)
            {
                Face face = GetFace(col, row);
                if (face != null)
                {
                    faces.Add(face);
                }
            }
        }

        return faces;
    }

    /**
     * Get vertices as a list
     */
    public List<Vertex> GetVerticesAsList()
    {
        List<Vertex> vertices = new List<Vertex>();

        // Store all of the board grid's items in a single dimensional array here.
        for (int col = 0; col < gridVertices.GetLength(0); col++)
        {
            for (int row = 0; row < gridVertices.GetLength(1); row++)
            {
                Vertex vertexL = GetVertex(col, row, VertexSpecifier.L);
                if (vertexL != null)
                {
                    vertices.Add(vertexL);
                }

                Vertex vertexR = GetVertex(col, row, VertexSpecifier.R);
                if (vertexR != null)
                {
                    vertices.Add(vertexR);
                }
            }
        }

        return vertices;
    }

    /**
     * Get edges as a list
     */
    public List<Edge> GetEdgesAsList()
    {
        List<Edge> edges = new List<Edge>();

        // Store all of the board grid's items in a single dimensional array here.
        for (int col = 0; col < gridEdges.GetLength(0); col++)
        {
            for (int row = 0; row < gridEdges.GetLength(1); row++)
            {
                Edge edgeW = GetEdge(col, row, EdgeSpecifier.W);
                if (edgeW != null)
                {
                    edges.Add(edgeW);
                }

                Edge edgeN = GetEdge(col, row, EdgeSpecifier.N);
                if (edgeN != null)
                {
                    edges.Add(edgeN);
                }

                Edge edgeE = GetEdge(col, row, EdgeSpecifier.E);
                if (edgeE != null)
                {
                    edges.Add(edgeE);
                }
            }
        }

        return edges;
    }

    /**
     * Create a face, and generate all vertices and edges for this face. 
     */
    public void CreateFaceWithEdgesAndVertices(int col, int row)
    {
        gridFaces[col, row] = new Face();

        // Set all vertices for this face
        gridVertices[col + 1, row, (int)VertexSpecifier.L] = new Vertex(); // Top right
        gridVertices[col, row, (int)VertexSpecifier.R] = new Vertex(); // Right
        gridVertices[col + 1, row - 1, (int)VertexSpecifier.L] = new Vertex(); // Bottom Right
        gridVertices[col - 1, row, (int)VertexSpecifier.R] = new Vertex(); // Bottom Left
        gridVertices[col, row, (int)VertexSpecifier.L] = new Vertex(); // Left
        gridVertices[col - 1, row + 1, (int)VertexSpecifier.R] = new Vertex(); // Top Right 

        // Set all edges for this face
        gridEdges[col, row, (int)EdgeSpecifier.N] = new Edge(); // Top Left
        gridEdges[col, row, (int)EdgeSpecifier.E] = new Edge(); // Top Right
        gridEdges[col, row, (int)EdgeSpecifier.W] = new Edge(); // Top Left
        gridEdges[col, row - 1, (int)EdgeSpecifier.N] = new Edge(); // Bottom
        gridEdges[col - 1, row, (int)EdgeSpecifier.E] = new Edge(); // Bottom Left
        gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W] = new Edge(); // Bottom Right
    }

    public void CreateFace(int col, int row)
    {
        gridFaces[col, row] = new Face();
    }

    /**
     * Create a vertex
     */
    public void CreateVertex(int col, int row, VertexSpecifier vertexSpecifier)
    {
        gridVertices[col, row, (int)vertexSpecifier] = new Vertex();
    }

    /**
     * Create an edge
     */
    public void CreateEdge(int col, int row, EdgeSpecifier edgeSpecifier)
    {
        gridEdges[col, row, (int)edgeSpecifier] = new Edge();
    }

    /**
     * Get vertices connected to face.
     */
    public List<Vertex> GetVerticesFromFace(int col, int row)
    {
        List<Vertex> vertexList = new List<Vertex>();

        vertexList.Add(gridVertices[col + 1, row, (int)VertexSpecifier.L]); // Top right
        vertexList.Add(gridVertices[col, row, (int)VertexSpecifier.R]); // Right
        vertexList.Add(gridVertices[col + 1, row - 1, (int)VertexSpecifier.L]); // Bottom Right
        vertexList.Add(gridVertices[col - 1, row, (int)VertexSpecifier.R]); // Bottom Left
        vertexList.Add(gridVertices[col, row, (int)VertexSpecifier.L]); // Left
        vertexList.Add(gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]); // Top Right 

        return vertexList;
    }

    /**
     * Gets all faces connected to this vertex.
     * - This will be used for calculating which players will receive what resources.
     */
    public List<Face> GetFacesFromVertexCoordinate(int col, int row, VertexSpecifier vertexSpec)
    {
        List<Face> faces = new List<Face>();
        switch (vertexSpec)
        {
            case VertexSpecifier.L:
                if (gridFaces[col - 1, row + 1] != null) { faces.Add(gridFaces[col - 1, row + 1]); }
                if (gridFaces[col - 1, row] != null) { faces.Add(gridFaces[col - 1, row]); }
                if (gridFaces[col, row] != null) { faces.Add(gridFaces[col, row]); }
                break;
            case VertexSpecifier.R:
                if (gridFaces[col, row] != null) { faces.Add(gridFaces[col, row]); }
                if (gridFaces[col + 1, row] != null) { faces.Add(gridFaces[col + 1, row]); }
                if (gridFaces[col + 1, row - 1] != null) { faces.Add(gridFaces[col + 1, row - 1]); }
                break;
        }

        return faces;
    }

    /**
     * Get all vertices adjacent to a vertex.
     * - This will be used to calculate validity of placement of settlements.
     */
    public List<Vertex> GetAdjacentVerticesFromVertex(int col, int row, VertexSpecifier vertexSpec)
    {
        List<Vertex> vertices = new List<Vertex>();
        switch (vertexSpec)
        {
            case VertexSpecifier.L:
                if (gridVertices[col - 2, row + 1, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col - 2, row + 1, (int)VertexSpecifier.R]); }
                if (gridVertices[col - 1, row + 1, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]); }
                if (gridVertices[col - 1, row, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col - 1, row, (int)VertexSpecifier.R]); }
                break;
            case VertexSpecifier.R:
                if (gridVertices[col + 1, row, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col + 1, row, (int)VertexSpecifier.L]); }
                if (gridVertices[col + 1, row - 1, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col + 1, row - 1, (int)VertexSpecifier.L]); }
                if (gridVertices[col + 2, row - 1, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col + 2, row - 1, (int)VertexSpecifier.L]); }
                break;
        }
        return vertices;
    }

    /**
     * Get all edges connected to a vertex.
     * - This will be used to calculate validity of placement of settlements.
     */
    public List<Edge> GetAdjacentEdgesFromVertex(int col, int row, VertexSpecifier vertexSpec)
    {
        List<Edge> edges = new List<Edge>();
        switch (vertexSpec)
        {
            case VertexSpecifier.L:
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col - 1, row, (int)EdgeSpecifier.N]); } // Lower Left
                if (gridEdges[col, row, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.W]); } // Top
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col - 1, row, (int)EdgeSpecifier.E]); } // Lower Right
                break;
            case VertexSpecifier.R:
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W]); }
                if (gridEdges[col, row, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.E]); }
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col + 1, row - 1, (int)EdgeSpecifier.N]); }
                break;
        }
        return edges;
    }

    /**
     * Return a vertex that is between two edges, if the edges are adjacent.
     */
    public Vertex GetVertexBetweenTwoConnectedEdges(int col, int row, EdgeSpecifier edgeSpecifier, int col2, int row2, EdgeSpecifier edgeSpecifier2)
    {
        Edge edge2 = gridEdges[col2, row2, (int)edgeSpecifier2];

        switch (edgeSpecifier)
        {
            case EdgeSpecifier.W:
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.N] == edge2) { return gridVertices[col, row, (int)VertexSpecifier.L]; } // Upper Left
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.E] == edge2) { return gridVertices[col, row, (int)VertexSpecifier.L]; } // Lower Left
                if (gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E] == edge2) { return gridVertices[col-1, row+1, (int)VertexSpecifier.R]; } // Upper Right
                if (gridEdges[col, row, (int)EdgeSpecifier.N] == edge2) { return gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]; } // Lower Right
                break;
            case EdgeSpecifier.N:
                if (gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E] == edge2) { return gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]; } // Upper Left
                if (gridEdges[col, row, (int)EdgeSpecifier.W] == edge2) { return gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]; } // Lower Left
                if (gridEdges[col + 1, row, (int)EdgeSpecifier.W] == edge2) { return gridVertices[col + 1, row, (int)VertexSpecifier.L]; } // Upper Right
                if (gridEdges[col, row, (int)EdgeSpecifier.E] == edge2) { return gridVertices[col + 1, row, (int)VertexSpecifier.L]; } // Lower Right
                break;
            case EdgeSpecifier.E:
                if (gridEdges[col + 1, row, (int)EdgeSpecifier.W] == edge2) { return gridVertices[col + 1, row, (int)VertexSpecifier.L]; } // Upper Left
                if (gridEdges[col, row, (int)EdgeSpecifier.N] == edge2) { return gridVertices[col + 1, row, (int)VertexSpecifier.L]; } // Lower Left
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.N] == edge2) { return gridVertices[col, row, (int)VertexSpecifier.R]; } // Upper Right
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W] == edge2) { return gridVertices[col, row, (int)VertexSpecifier.R]; } // Lower Right
                break;
        }
        return null;
    }

    /**
     * Get all edges connected to an edge.
     * - This will be used to calculate validity of placement of roads.
     */
    public List<Edge> GetConnectedEdgesFromEdge(int col, int row, EdgeSpecifier edgeSpec)
    {
        List<Edge> edges = new List<Edge>();
        switch (edgeSpec)
        {
            case EdgeSpecifier.W:
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col - 1, row, (int)EdgeSpecifier.N]); } // Upper Left
                if (gridEdges[col - 1, row, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col - 1, row, (int)EdgeSpecifier.E]); } // Lower Left
                if (gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E]); } // Upper Right
                if (gridEdges[col, row, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.N]); } // Lower Right
                break;
            case EdgeSpecifier.N:
                if (gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col - 1, row + 1, (int)EdgeSpecifier.E]); } // Upper Left
                if (gridEdges[col, row, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.W]); } // Lower Left
                if (gridEdges[col + 1, row, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col + 1, row, (int)EdgeSpecifier.W]); } // Upper Right
                if (gridEdges[col, row, (int)EdgeSpecifier.E] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.E]); } // Lower Right
                break;
            case EdgeSpecifier.E:
                if (gridEdges[col + 1, row, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col + 1, row, (int)EdgeSpecifier.W]); } // Upper Left
                if (gridEdges[col, row, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col, row, (int)EdgeSpecifier.N]); } // Lower Left
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.N] != null) { edges.Add(gridEdges[col + 1, row - 1, (int)EdgeSpecifier.N]); } // Upper Right
                if (gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W] != null) { edges.Add(gridEdges[col + 1, row - 1, (int)EdgeSpecifier.W]); } // Lower Right
                break;
        }
        return edges;
    }

    /**
     * Get all edges connected to a vertex.
     * - This will be used to calculate validity of placement of roads.
     */
    public List<Vertex> GetConnectedVerticesFromEdge(int col, int row, EdgeSpecifier edgeSpec)
    {
        List<Vertex> vertices = new List<Vertex>();
        switch (edgeSpec)
        {
            case EdgeSpecifier.W:
                if (gridVertices[col, row, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col, row, (int)VertexSpecifier.L]); }
                if (gridVertices[col - 1, row + 1, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]); }
                break;
            case EdgeSpecifier.N:
                if (gridVertices[col - 1, row + 1, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col - 1, row + 1, (int)VertexSpecifier.R]); }
                if (gridVertices[col + 1, row, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col + 1, row, (int)VertexSpecifier.L]); }
                break;
            case EdgeSpecifier.E:
                if (gridVertices[col + 1, row, (int)VertexSpecifier.L] != null) { vertices.Add(gridVertices[col + 1, row, (int)VertexSpecifier.L]); }
                if (gridVertices[col, row, (int)VertexSpecifier.R] != null) { vertices.Add(gridVertices[col, row, (int)VertexSpecifier.R]); }
                break;
        }
        return vertices;
    }

    public int[] GetVertexPosition(Vertex vertex)
    {
        for(int col = 0; col < gridVertices.GetLength(0); col++)
        {
            for(int row = 0; row < gridVertices.GetLength(1); row++)
            {
                for(int spec = 0; spec < gridVertices.GetLength(2); spec++)
                {
                    if(gridVertices[col, row, spec] == vertex)
                    {
                        return new int[] { col, row, spec };
                    }
                }
            }
        }
        return null;
    }

    public bool Equals(BoardGrid other)
    {
        for (int col = 0; col < this.GetColCount(); col++)
        {
            for (int row = 0; row < this.GetRowCount(); row++)
            {
                // This' properties
                Face face = this.GetFace(col, row);

                Vertex vertexL = this.GetVertex(col, row, VertexSpecifier.L);
                Vertex vertexR = this.GetVertex(col, row, VertexSpecifier.R);

                Edge edgeW = this.GetEdge(col, row, EdgeSpecifier.W);
                Edge edgeN = this.GetEdge(col, row, EdgeSpecifier.N);
                Edge edgeE = this.GetEdge(col, row, EdgeSpecifier.E);

                // Other's properties
                Face faceOther = other.GetFace(col, row);

                Vertex vertexLOther = other.GetVertex(col, row, VertexSpecifier.L);
                Vertex vertexROther = other.GetVertex(col, row, VertexSpecifier.R);

                Edge edgeWOther = other.GetEdge(col, row, EdgeSpecifier.W);
                Edge edgeNOther = other.GetEdge(col, row, EdgeSpecifier.N);
                Edge edgeEOther = other.GetEdge(col, row, EdgeSpecifier.E);

                // Comparing face
                if (face == null)
                {
                    if (faceOther != null) { return false; }
                }
                else
                {
                    if (!face.Equals(faceOther)) { return false; }
                }

                // Comparing vertices
                if (vertexL == null)
                {
                    if (vertexLOther != null) { return false; }
                }
                else
                {
                    if (!vertexL.Equals(vertexLOther)) { return false; }
                }
                if (vertexR == null)
                {
                    if (vertexROther != null) { return false; }
                }
                else
                {
                    if (!vertexR.Equals(vertexROther)) { return false; }
                }

                // Comparing edges
                if (edgeW == null)
                {
                    if (edgeWOther != null) { return false; }
                }
                else
                {
                    if (!edgeW.Equals(edgeWOther)) { return false; }
                }

                if (edgeN == null)
                {
                    if (edgeNOther != null) { return false; }
                }
                else
                {
                    if (!edgeN.Equals(edgeNOther)) { return false; }
                }

                if (edgeE == null)
                {
                    if (edgeNOther != null) { return false; }
                }
                else
                {
                    if (!edgeE.Equals(edgeEOther)) { return false; }
                }
            }
        }

        return true;
    }
    
    public override string ToString()
    {
        string boardString = "";
        for (int col = 0; col < this.GetColCount(); col++)
        {
            for (int row = 0; row < this.GetRowCount(); row++)
            {
                // This' properties
                Face face = this.GetFace(col, row);

                Vertex vertexL = this.GetVertex(col, row, VertexSpecifier.L);
                Vertex vertexR = this.GetVertex(col, row, VertexSpecifier.R);

                Edge edgeW = this.GetEdge(col, row, EdgeSpecifier.W);
                Edge edgeN = this.GetEdge(col, row, EdgeSpecifier.N);
                Edge edgeE = this.GetEdge(col, row, EdgeSpecifier.E);

                // Face exists?
                if (face != null)
                {
                    // Face col, row
                    boardString += "( " + col + "," + row + " ) " + face.ToString();
                    boardString += "\n";
                }

                // Vertex L exists?
                if (vertexL != null)
                {
                    boardString += "( " + col + "," + row + ", L " + " ) " + vertexL.ToString();
                    boardString += "\n";
                }

                // Vertex R exists?
                if (vertexR != null)
                {
                    boardString += "( " + col + "," + row + ", R " + " ) " + vertexR.ToString();
                    boardString += "\n";
                }

                // Edge W exists?
                if (edgeW != null)
                {
                    boardString += "( " + col + "," + row + ", W " + " ) " + edgeW.ToString();
                    boardString += "\n";
                }

                // Edge N exists?
                if (edgeN != null)
                {
                    boardString += "( " + col + "," + row + ", N " + " ) " + edgeN.ToString();
                    boardString += "\n";
                }

                // Edge E exists?
                if (edgeE != null)
                {
                    boardString += "( " + col + "," + row + ", E " + " ) " + edgeE.ToString();
                    boardString += "\n";
                }
            }
        }

        return boardString;
    }
}
