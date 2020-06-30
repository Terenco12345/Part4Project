using System.Collections.Generic;

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

    public BoardGrid(int rows, int cols)
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

    public Vertex[,,] GetVertices() {
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
    public void CreateFace(int col, int row)
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
}
