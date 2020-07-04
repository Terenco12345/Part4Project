using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexRenderer : MonoBehaviour
{
    public GameObject settlementPrefab;
    public GameObject cityPrefab;

    public int col;
    public int row;
    public BoardGrid.VertexSpecifier vertexSpec;

    GameObject settlementObject;
    GameObject cityObject;

    // Start is called before the first frame update
    void Start()
    {
        settlementObject = Instantiate(settlementPrefab, transform);
        cityObject = Instantiate(cityPrefab, transform);
    }

    // Update is called once per frame
    void Update()
    {
        Vertex vertex = GameManager.Instance.GetGame().boardHandler.GetBoardGrid().GetVertex(col, row, vertexSpec);
        if(vertex.settlement != null)
        {
            int playerIndex = GameManager.Instance.GetGame().GetPlayerNumberById(vertex.settlement.ownerId);
            settlementObject.GetComponent<MeshRenderer>().material.color = GameManager.Instance.playerColors[playerIndex];
            cityObject.GetComponent<MeshRenderer>().material.color = GameManager.Instance.playerColors[playerIndex];
            if (vertex.settlement.isCity)
            {
                settlementObject.SetActive(false);
                cityObject.SetActive(true);
            }
            else
            {
                settlementObject.SetActive(true);
                cityObject.SetActive(false);
            }
        } else
        {
            settlementObject.SetActive(false);
            cityObject.SetActive(false);
        }
        
    }
}
