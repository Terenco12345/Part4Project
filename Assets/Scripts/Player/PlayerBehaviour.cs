using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    public Stack<GameObject> settlements = new Stack<GameObject>();
    public Stack<GameObject> cities = new Stack<GameObject>();
    public Stack<GameObject> roads = new Stack<GameObject>();

    public GameObject placementGhost;

    // Start is called before the first frame update
    void Start()
    {
        // Set up settlements, cities and roads
        for(int i = 0; i < settlements.Count; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSuccess = Physics.Raycast(ray, out hit, 100f);

        if (hitSuccess)
        {
            // Inspection
            if (hit.collider.gameObject.GetComponent<Inspectable>() != null)
            {
                GameManager.Instance.inspectionText.text = hit.collider.gameObject.GetComponent<Inspectable>().getInspectionText();
            }
            else
            {
                GameManager.Instance.inspectionText.text = "";
            }

            // Placement
            VertexBehaviour vertex = hit.collider.gameObject.GetComponent<VertexBehaviour>();
            EdgeBehaviour edge = hit.collider.gameObject.GetComponent<EdgeBehaviour>();

            // Show valid placement option if possible
            if (vertex != null || edge != null)
            {
                // Settlement
                if (vertex != null)
                {
                    // Show ghost for settlement
                    if (CanPlaceSettlement(vertex))
                    {
                        UpdatePlacementGhost(settlementPrefab, vertex.transform);
                        AttemptSettlementPlacement(vertex);
                    } else if (CanPlaceCity(vertex))
                    {
                        UpdatePlacementGhost(cityPrefab, vertex.transform);
                        AttemptCityPlacement(vertex);
                    }
                }

                // Road
                if (edge != null)
                {
                    // Show ghost for road
                    if(CanPlaceRoad(edge))
                    {
                        UpdatePlacementGhost(roadPrefab, edge.transform);
                        AttemptRoadPlacement(edge);
                    }
                }
            } else
            {
                DestroyPlacementGhost();
            }
        }
    }

    void UpdatePlacementGhost(GameObject prefab, Transform location)
    {
        DestroyPlacementGhost();
        placementGhost = Instantiate(prefab, location);
    }

    void DestroyPlacementGhost()
    {
        if (placementGhost != null)
        {
            Destroy(placementGhost);
        }
    }

    bool CanPlaceSettlement(VertexBehaviour vertex)
    {
        if (vertex.building == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    bool CanPlaceCity(VertexBehaviour vertex)
    {
        if (vertex.building != null) // If it is this player's settlement
        {
            if (vertex.building.GetComponent<SettlementBehaviour>() != null)
            {
                return true;
            } else
            {
                return false;
            }
            
        } else
        {
            return false;
        }
    }

    bool CanPlaceRoad(EdgeBehaviour edge)
    {
        if (edge.road == null)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void AttemptSettlementPlacement(VertexBehaviour vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            vertex.building = Instantiate(settlementPrefab, vertex.transform);
        }
    }

    void AttemptCityPlacement(VertexBehaviour vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(vertex.building != null)
            {
                Destroy(vertex.building);
            }
            vertex.building = Instantiate(cityPrefab, vertex.transform);
        }
    }

    void AttemptRoadPlacement(EdgeBehaviour edge)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            edge.road = Instantiate(roadPrefab, edge.transform);
        }
    }
}
