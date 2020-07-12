using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeRenderer : MonoBehaviour
{
    public GameObject roadPrefab;

    public int col;
    public int row;
    public BoardGrid.EdgeSpecifier edgeSpec;

    GameObject roadObject;

    // Start is called before the first frame update
    void Start()
    {
        roadObject = Instantiate(roadPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        Edge edge = GameManager.Instance.GetGame().boardHandler.GetBoardGrid().GetEdge(col, row, edgeSpec);
        if (edge.road != null)
        {
            int playerIndex = GameManager.Instance.GetGame().GetPlayerIndexById(edge.road.ownerId);
            roadObject.GetComponent<MeshRenderer>().material.color = GameManager.Instance.playerColors[playerIndex];
            
            roadObject.SetActive(true);
        } else
        {
            roadObject.SetActive(false);
        }
    }
}
