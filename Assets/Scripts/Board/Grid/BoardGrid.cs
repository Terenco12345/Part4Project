using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
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

    GameObject boardBehaviour;

    GameObject[,] gridFaces;
    GameObject[,,] gridVertices;
    GameObject[,,] gridEdges;

    float gridWidth = 4.82f / 1.1547005f; // Divide width by ratio of hexagon height/width
    float gridHeight = 4.82f;
    Vector3 startPosition = new Vector3(-4.82f * 6f, 0, 4.82f / 1.1547005f * -3);

    public BoardGrid(GameObject boardBehaviour, int rows, int cols)
    {
        this.boardBehaviour = boardBehaviour;

        gridFaces = new GameObject[cols, rows];
        gridVertices = new GameObject[cols, rows, 2];
        gridEdges = new GameObject[cols, rows, 3];

        // Populare the arrays with null
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

    /**
     * Obtain a face from this grid.
     */
    public GameObject GetFace(int col, int row)
    {
        return gridFaces[col, row];
    }

    /**
     * Create a face, and generate all vertices and edges for this face. 
     */
    public void CreateFace(GameObject face, GameObject vertex, GameObject edge, int col, int row)
    {
        if(gridFaces[col, row] != null)
        {
            Object.Destroy(gridFaces[col, row]);
        }
        GameObject faceObject = Object.Instantiate(face, boardBehaviour.transform);
        faceObject.GetComponent<Face>().col = col;
        faceObject.GetComponent<Face>().row = row;
        gridFaces[col, row] = faceObject;

        // Set all vertices for this face
        CreateVertex(vertex, col + 1, row, VertexSpecifier.L); // Top right
        CreateVertex(vertex, col, row, VertexSpecifier.R); // Right
        CreateVertex(vertex, col + 1, row - 1, VertexSpecifier.L); // Bottom Right
        CreateVertex(vertex, col - 1, row, VertexSpecifier.R); // Bottom Left
        CreateVertex(vertex, col, row, VertexSpecifier.L); // Left
        CreateVertex(vertex, col - 1, row + 1, VertexSpecifier.R); // Top Right 

        // Set all edges for this face
        CreateEdge(edge, col, row, EdgeSpecifier.N); // Top Left
        CreateEdge(edge, col, row, EdgeSpecifier.E); // Top Right
        CreateEdge(edge, col, row, EdgeSpecifier.W); // Top Left
        CreateEdge(edge, col, row - 1, EdgeSpecifier.N); // Bottom
        CreateEdge(edge, col - 1, row, EdgeSpecifier.E); // Bottom Left
        CreateEdge(edge, col + 1, row - 1, EdgeSpecifier.W); // Bottom Right
    }

    public void CreateVertex(GameObject vertex, int col, int row, VertexSpecifier vertexSpec)
    {
        if (gridVertices[col, row, (int)vertexSpec] != null)
        {
            Object.Destroy(gridVertices[col, row, (int)vertexSpec]);
        }
        GameObject vertexObject = Object.Instantiate(vertex, boardBehaviour.transform);
        vertexObject.GetComponent<Vertex>().col = col;
        vertexObject.GetComponent<Vertex>().row = row;
        vertexObject.GetComponent<Vertex>().vertexSpec = vertexSpec;

        gridVertices[col, row, (int)vertexSpec] = vertexObject;
    }

    public void CreateEdge(GameObject edge, int col, int row, EdgeSpecifier edgeSpec)
    {
        if (gridEdges[col, row, (int)edgeSpec] != null)
        {
            Object.Destroy(gridEdges[col, row, (int)edgeSpec]);
        }
        GameObject edgeObject = Object.Instantiate(edge, boardBehaviour.transform);
        edgeObject.GetComponent<Edge>().col = col;
        edgeObject.GetComponent<Edge>().row = row;
        edgeObject.GetComponent<Edge>().edgeSpec = edgeSpec;
        
        gridEdges[col, row, (int)edgeSpec] = edgeObject;
    }

    /**
     * Obtain a world coordinate from a face grid coordinate.
     * Each column is shifted up half a hexagon's amount.
     */
    public Vector3 GetPositionOfFaceCoordinate(int col, int row)
    {
        Vector3 pos = new Vector3(col * gridHeight + (row * gridHeight / 2), 0, row * gridWidth) + startPosition;
        return pos;
    }

    /**
     * Obtain a world coordinate for a vertex from a grid coordinate.
     */
    public Vector3 GetPositionOfVertexCoordinate(int col, int row, VertexSpecifier vertexSpec)
    {
        Vector3 pos = GetPositionOfFaceCoordinate(col, row);
        switch (vertexSpec)
        {
            case VertexSpecifier.L:
                pos = pos + new Vector3(-gridHeight/2, 0, 0.28f * gridHeight);
                break;
            case VertexSpecifier.R:
                pos = pos + new Vector3(gridHeight/2, 0, -0.28f * gridHeight);
                break;
        }

        return pos;
    }

    /**
     * Obtains a world coordinate for an edge at a grid coordinate.
     */
    public Vector3 GetPositionOfEdgeCoordinate(int col, int row, EdgeSpecifier edgeSpec)
    {
        Vector3 pos = GetPositionOfFaceCoordinate(col, row);
        switch (edgeSpec)
        {
            case EdgeSpecifier.N:
                pos = pos + new Vector3(gridHeight / 4, 0, gridWidth / 2);
                break;
            case EdgeSpecifier.W:
                pos = pos + new Vector3(-gridHeight / 4, 0, gridWidth / 2);
                break;
            case EdgeSpecifier.E:
                pos = pos + new Vector3(gridHeight / 2, 0, 0);
                break;
        }

        return pos;
    }

    /**
     * Each face, vertex and edge object stored will have their positions be aligned to the grid.
     */
    public void UpdatePositions()
    {
        for (int row = 0; row < gridFaces.GetLength(1); row++)
        {
            for(int col = 0; col < gridFaces.GetLength(0); col++)
            {
                // Faces
                if(gridFaces[col, row] != null)
                {
                    gridFaces[col, row].transform.localPosition = GetPositionOfFaceCoordinate(col, row);
                }

                // Vertices
                if(gridVertices[col, row, (int)VertexSpecifier.L] != null)
                {
                    gridVertices[col, row, (int)VertexSpecifier.L].transform.localPosition = GetPositionOfVertexCoordinate(col, row, VertexSpecifier.L);
                } 
                if (gridVertices[col, row, (int)VertexSpecifier.R] != null)
                {
                    gridVertices[col, row, (int)VertexSpecifier.R].transform.localPosition = GetPositionOfVertexCoordinate(col, row, VertexSpecifier.R);
                }

                // Edges
                if(gridEdges[col, row, (int)EdgeSpecifier.N] != null)
                {
                    gridEdges[col, row, (int)EdgeSpecifier.N].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, EdgeSpecifier.N);
                    gridEdges[col, row, (int)EdgeSpecifier.N].transform.localRotation = Quaternion.Euler(0, 120, 0);
                }
                if (gridEdges[col, row, (int)EdgeSpecifier.E] != null)
                {
                    gridEdges[col, row, (int)EdgeSpecifier.E].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, EdgeSpecifier.E);
                }
                if (gridEdges[col, row, (int)EdgeSpecifier.W] != null)
                {
                    gridEdges[col, row, (int)EdgeSpecifier.W].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, EdgeSpecifier.W);
                    gridEdges[col, row, (int)EdgeSpecifier.W].transform.localRotation = Quaternion.Euler(0, -120, 0);
                }
            }
        }
    }

    /**
     * Gets all faces connected to this vertex.
     * - This will be used for calculating which players will receive what resources.
     */
    public List<GameObject> GetFacesFromVertexCoordinate(int col, int row, VertexSpecifier vertexSpec)
    {
        List<GameObject> faces = new List<GameObject>();
        switch (vertexSpec)
        {
            case VertexSpecifier.L:
                if (gridFaces[col - 1, row + 1] != null) { faces.Add(gridFaces[col - 1, row + 1]); }
                if (gridFaces[col - 1, row] != null) { faces.Add(gridFaces[col - 1, row]); }
                if (gridFaces[col, row] != null) { faces.Add(gridFaces[col, row]); }
                break;
            case VertexSpecifier.R:
                if (gridFaces[col, row] != null) { faces.Add(gridFaces[col, row]); }
                if (gridFaces[col+1, row] != null) { faces.Add(gridFaces[col + 1, row]); }
                if (gridFaces[col+1, row-1] != null) { faces.Add(gridFaces[col + 1, row - 1]); }
                break;
        }

        return faces;
    }

    /**
     * Get all vertices adjacent to a vertex.
     * - This will be used to calculate validity of placement of settlements.
     */
    public List<GameObject> GetAdjacentVerticesFromVertex(int col, int row, VertexSpecifier vertexSpec)
    {
        List<GameObject> vertices = new List<GameObject>();
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
    public List<GameObject> GetAdjacentEdgesFromVertex(int col, int row, VertexSpecifier vertexSpec)
    {
        List<GameObject> edges = new List<GameObject>();
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
    public List<GameObject> GetConnectedEdgesFromEdge(int col, int row, EdgeSpecifier edgeSpec)
    {
        List<GameObject> edges = new List<GameObject>();
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
    public List<GameObject> GetConnectedVerticesFromEdge(int col, int row, EdgeSpecifier edgeSpec)
    {
        List<GameObject> vertices = new List<GameObject>();
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
