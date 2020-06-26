using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public Material transparentMaterial; // Used for placement ghost

    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    public List<GameObject> settlements = new List<GameObject>();
    public List<GameObject> cities = new List<GameObject>();
    public List<GameObject> roads = new List<GameObject>();

    public List<GameObject> placedSettlements = new List<GameObject>();
    public List<GameObject> placedCities = new List<GameObject>();
    public List<GameObject> placedRoads = new List<GameObject>();

    GameObject placementGhost;

    // Cards
    List<ResourceType> resources = new List<ResourceType>();
    List<DevelopmentCardType> developmentCards = new List<DevelopmentCardType>();

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
        // Update materials to this player's color
        settlementPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
        roadPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
        cityPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);

        Ray ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSuccess = Physics.Raycast(ray, out hit, 100f);

        if (hitSuccess)
        {
            // Inspection
            if (hit.collider.gameObject.GetComponent<Inspectable>() != null)
            {
                GameManager.Instance.guiManager.inspectionText.text = hit.collider.gameObject.GetComponent<Inspectable>().getInspectionText();
            }
            else
            {
                GameManager.Instance.guiManager.inspectionText.text = "";
            }

            // Placement
            Vertex vertex = hit.collider.gameObject.GetComponent<Vertex>();
            Edge edge = hit.collider.gameObject.GetComponent<Edge>();

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
        
        // Create a transparent material
        Material ghostMaterial = new Material(transparentMaterial);
        Color ghostColor = color;
        ghostColor.a = 0.3f;

        ghostMaterial.SetColor("_Color", ghostColor);

        placementGhost.GetComponent<Renderer>().sharedMaterial = ghostMaterial;
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
    bool CanPlaceSettlement(Vertex vertex)
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
    bool CanPlaceCity(Vertex vertex)
    {
        if (vertex.building != null && cities.Count > 0) // If it is this player's settlement
        {
            if (vertex.building.GetComponent<Settlement>() != null)
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
    bool CanPlaceRoad(Edge edge)
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
    void AttemptSettlementPlacement(Vertex vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(settlements[settlements.Count - 1]);
            settlements.RemoveAt(settlements.Count - 1);

            vertex.building = Instantiate(settlementPrefab, vertex.transform);
            vertex.building.GetComponent<Settlement>().owner = this;
            placedSettlements.Add(vertex.building);
        }
    }

    /**
     * If there is a key input, move a city to replace a settlement 
     * (put the settlement in player store, move the city from store to board).
     */
    void AttemptCityPlacement(Vertex vertex)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            settlements.Add(Instantiate(settlementPrefab, transform));
            Destroy(cities[cities.Count - 1]);
            cities.RemoveAt(cities.Count - 1);
            placedSettlements.Remove(vertex.building);
            Destroy(vertex.building);

            vertex.building = Instantiate(cityPrefab, vertex.transform);
            vertex.building.GetComponent<City>().owner = this;
            placedCities.Add(vertex.building);
        }
    }

    void AttemptRoadPlacement(Edge edge)
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Destroy(roads[roads.Count - 1]);
            roads.RemoveAt(roads.Count - 1);

            edge.road = Instantiate(roadPrefab, edge.transform);
            edge.road.GetComponent<Road>().owner = this;
            placedRoads.Add(edge.road);
        }
    }

    /**
     * Reset this player's resources.
     */
    public void ResetResources()
    {
        resources.Clear();
    }

    public void AddResource(ResourceType type, int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            resources.Add(type);
        }
    }

    /**
     * Give this player a certain amount of resources.
     */
    public void AddResources(int lumber, int wool, int grain, int brick, int ore)
    {
        for(int i = 0; i < lumber; i++)
        {
            resources.Add(ResourceType.Lumber);
        }
        for (int i = 0; i < wool; i++)
        {
            resources.Add(ResourceType.Wool);
        }
        for (int i = 0; i < grain; i++)
        {
            resources.Add(ResourceType.Grain);
        }
        for (int i = 0; i < brick; i++)
        {
            resources.Add(ResourceType.Brick);
        }
        for (int i = 0; i < ore; i++)
        {
            resources.Add(ResourceType.Ore);
        }
    }

    /**
     * Remove resources from this player.
     */
    public void RemoveResources(int lumber, int wool, int grain, int brick, int ore)
    {
        // Check if there are enough resources for this action to take place.
        if (GetResourceCount(ResourceType.Lumber) >= lumber 
            && GetResourceCount(ResourceType.Wool) >= wool 
            && GetResourceCount(ResourceType.Grain) > grain 
            && GetResourceCount(ResourceType.Brick) > brick 
            && GetResourceCount(ResourceType.Ore) > ore)
        {
            return;
        }

        // Remove resources
        for (int i = 0; i < lumber; i++)
        {
            resources.Remove(ResourceType.Lumber);
        }
        for (int i = 0; i < wool; i++)
        {
            resources.Remove(ResourceType.Wool);
        }
        for (int i = 0; i < grain; i++)
        {
            resources.Remove(ResourceType.Grain);
        }
        for (int i = 0; i < brick; i++)
        {
            resources.Remove(ResourceType.Brick);
        }
        for (int i = 0; i < ore; i++)
        {
            resources.Remove(ResourceType.Ore);
        }
    }

    /**
     * Get the amount for a certain type of resource owned by this player.
     */
    public int GetResourceCount(ResourceType type)
    {
        int count = 0;
        for(int i = 0; i < resources.Count; i++)
        {
            if(resources[i] == type)
            {
                count++;
            }
        }
        return count;
    }
}
