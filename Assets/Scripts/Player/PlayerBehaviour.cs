using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    public List<GameObject> settlements = new List<GameObject>();
    public List<GameObject> cities = new List<GameObject>();
    public List<GameObject> roads = new List<GameObject>();

    public GameObject placementGhost;

    // Start is called before the first frame update
    void Start()
    {
        float spacing = 0;

        // Set up settlements, cities and roads
        for(int i = 0; i < 5; i++)
        {
            GameObject settlement = Instantiate(settlementPrefab, transform);
            settlement.transform.localPosition = new Vector3(spacing, 0, 0);
            settlements.Add(settlement);
            spacing += 1.4f;
        }

        spacing = 0;
        for (int i = 0; i < 5; i++)
        {
            GameObject city = Instantiate(cityPrefab, transform);
            city.transform.localPosition = new Vector3(spacing, 0, 3);
            cities.Add(city);
            spacing += 1.4f;
        }

        spacing = 0;
        for (int i = 0; i < 15; i++)
        {
            GameObject road = Instantiate(roadPrefab, transform);
            road.transform.localPosition = new Vector3(spacing, 0, 5);
            roads.Add(road);
            spacing += 0.8f;
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

        float spacing = 0;
        // Render the items in settlements, cities and roads
        for (int i = 0; i < settlements.Count; i++)
        {
            settlements[i].transform.localPosition = new Vector3(spacing, 0, 0);
            spacing += 1.4f;
        }

        spacing = 0;
        for (int i = 0; i < cities.Count; i++)
        {
            cities[i].transform.localPosition = new Vector3(spacing, 0, 3);
            spacing += 1.4f;
        }

        spacing = 0;
        for (int i = 0; i < roads.Count; i++)
        {
            roads[i].transform.localPosition = new Vector3(spacing, 0, 5);
            spacing += 0.8f;
        }

    }

    // Update the inspection placement ghost
    void UpdatePlacementGhost(GameObject prefab, Transform location)
    {
        DestroyPlacementGhost();
        placementGhost = Instantiate(prefab, location);
    }

    // Destroy the current placement ghost
    void DestroyPlacementGhost()
    {
        if (placementGhost != null)
        {
            Destroy(placementGhost);
        }
    }

    /**
     * Can place settlements only when there are no neighbouring settlements, and if connected to a road that player already owns.
     * In setup mode, can place settlements not connected to a road.
     */
    bool CanPlaceSettlement(VertexBehaviour vertex)
    {
        if (vertex.building == null && settlements.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    /**
     * Can only place cities the player has the resources, and only to upgrade an existing settlement.
     */
    bool CanPlaceCity(VertexBehaviour vertex)
    {
        if (vertex.building != null && cities.Count > 0) // If it is this player's settlement
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

    /**
     * Roads must be connected to a settlement or another road.
     */
    bool CanPlaceRoad(EdgeBehaviour edge)
    {
        if (edge.road == null && roads.Count > 0)
        {
            return true;
        } else
        {
            return false;
        }
    }

    /**
     * If there is a key input, move a settlement from player store onto the board.
     */
    void AttemptSettlementPlacement(VertexBehaviour vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(settlements[settlements.Count - 1]);
            settlements.RemoveAt(settlements.Count - 1);
            vertex.building = Instantiate(settlementPrefab, vertex.transform);
        }
    }

    /**
     * If there is a key input, move a city to replace a settlement 
     * (put the settlement in player store, move the city from store to board).
     */
    void AttemptCityPlacement(VertexBehaviour vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            settlements.Add(Instantiate(settlementPrefab, transform));
            Destroy(cities[cities.Count - 1]);
            cities.RemoveAt(cities.Count - 1);
            vertex.building = Instantiate(cityPrefab, vertex.transform);
        }
    }

    void AttemptRoadPlacement(EdgeBehaviour edge)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(roads[roads.Count - 1]);
            roads.RemoveAt(roads.Count - 1);
            edge.road = Instantiate(roadPrefab, edge.transform);
        }
    }
}
