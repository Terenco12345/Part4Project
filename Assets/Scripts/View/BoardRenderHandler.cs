using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardRenderHandler : MonoBehaviour
{
    public GameObject robberObject;

    public GameObject tilePrefab;
    public GameObject vertexPrefab;
    public GameObject edgePrefab;

    public GameController gameController;

    float gridWidth = 4.82f / 1.1547005f; // Divide width by ratio of hexagon height/width
    float gridHeight = 4.82f;
    Vector3 startPosition = new Vector3(-4.82f * 6f, 0, 4.82f / 1.1547005f * -3);

    GameObject[,] gridFaces;
    GameObject[,,] gridVertices;
    GameObject[,,] gridEdges;

    // Start is called before the first frame update
    void Start()
    {
        Face[,] faces = gameController.GetGame().GetBoardHandler().GetBoardGrid().GetFaces();
        Vertex[,,] vertices = gameController.GetGame().GetBoardHandler().GetBoardGrid().GetVertices();
        Edge[,,] edges = gameController.GetGame().GetBoardHandler().GetBoardGrid().GetEdges();

        gridFaces = new GameObject[faces.GetLength(0), faces.GetLength(1)];
        gridVertices = new GameObject[faces.GetLength(0), faces.GetLength(1), 2];
        gridEdges = new GameObject[faces.GetLength(0), faces.GetLength(1), 3];

        for (int col = 0; col < faces.GetLength(0); col++)
        {
            for (int row = 0; row < faces.GetLength(1); row++)
            {
                if (faces[col, row] != null)
                {
                    GameObject face = Instantiate(tilePrefab, transform);
                    face.GetComponent<TileRenderer>().tile = faces[col, row].tile;
                    gridFaces[col, row] = face;
                }
                
                if(vertices[col, row, (int)BoardGrid.VertexSpecifier.L] != null)
                {
                    gridVertices[col, row, (int)BoardGrid.VertexSpecifier.L] = Instantiate(vertexPrefab, transform);
                }
                if (vertices[col, row, (int)BoardGrid.VertexSpecifier.R] != null)
                {
                    gridVertices[col, row, (int)BoardGrid.VertexSpecifier.R] = Instantiate(vertexPrefab, transform);
                }

                if (edges[col, row, (int)BoardGrid.EdgeSpecifier.W] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.W] = Instantiate(edgePrefab, transform);
                }
                if (edges[col, row, (int)BoardGrid.EdgeSpecifier.N] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.N] = Instantiate(edgePrefab, transform);
                }
                if (edges[col, row, (int)BoardGrid.EdgeSpecifier.E] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.E] = Instantiate(edgePrefab, transform);
                }
            }
        }

        UpdatePositions();
    }

    // Update is called once per frame
    void Update()
    {

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
    public Vector3 GetPositionOfVertexCoordinate(int col, int row, BoardGrid.VertexSpecifier vertexSpec)
    {
        Vector3 pos = GetPositionOfFaceCoordinate(col, row);
        switch (vertexSpec)
        {
            case BoardGrid.VertexSpecifier.L:
                pos = pos + new Vector3(-gridHeight / 2, 0, 0.28f * gridHeight);
                break;
            case BoardGrid.VertexSpecifier.R:
                pos = pos + new Vector3(gridHeight / 2, 0, -0.28f * gridHeight);
                break;
        }

        return pos;
    }

    /**
     * Obtains a world coordinate for an edge at a grid coordinate.
     */
    public Vector3 GetPositionOfEdgeCoordinate(int col, int row, BoardGrid.EdgeSpecifier edgeSpec)
    {
        Vector3 pos = GetPositionOfFaceCoordinate(col, row);
        switch (edgeSpec)
        {
            case BoardGrid.EdgeSpecifier.N:
                pos = pos + new Vector3(gridHeight / 4, 0, gridWidth / 2);
                break;
            case BoardGrid.EdgeSpecifier.W:
                pos = pos + new Vector3(-gridHeight / 4, 0, gridWidth / 2);
                break;
            case BoardGrid.EdgeSpecifier.E:
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
            for (int col = 0; col < gridFaces.GetLength(0); col++)
            {
                // Faces
                if (gridFaces[col, row] != null)
                {
                    gridFaces[col, row].transform.localPosition = GetPositionOfFaceCoordinate(col, row);
                }

                // Vertices
                if (gridVertices[col, row, (int)BoardGrid.VertexSpecifier.L] != null)
                {
                    gridVertices[col, row, (int)BoardGrid.VertexSpecifier.L].transform.localPosition = GetPositionOfVertexCoordinate(col, row, BoardGrid.VertexSpecifier.L);
                }
                if (gridVertices[col, row, (int)BoardGrid.VertexSpecifier.R] != null)
                {
                    gridVertices[col, row, (int)BoardGrid.VertexSpecifier.R].transform.localPosition = GetPositionOfVertexCoordinate(col, row, BoardGrid.VertexSpecifier.R);
                }

                // Edges
                if (gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.N] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.N].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, BoardGrid.EdgeSpecifier.N);
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.N].transform.localRotation = Quaternion.Euler(0, 120, 0);
                }
                if (gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.E] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.E].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, BoardGrid.EdgeSpecifier.E);
                }
                if (gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.W] != null)
                {
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.W].transform.localPosition = GetPositionOfEdgeCoordinate(col, row, BoardGrid.EdgeSpecifier.W);
                    gridEdges[col, row, (int)BoardGrid.EdgeSpecifier.W].transform.localRotation = Quaternion.Euler(0, -120, 0);
                }
            }
        }

        BoardHandler boardHandler = gameController.GetGame().GetBoardHandler();
        Debug.Log(boardHandler.robberCol + ", " + boardHandler.robberRow);
        robberObject.transform.position = gridFaces[boardHandler.robberCol, boardHandler.robberRow].transform.position;
    }
}
