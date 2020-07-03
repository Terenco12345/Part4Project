using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBehaviour : NetworkBehaviour
{
    Player player = null;

    public Color color;
    public Material transparentMaterial; // Used for placement ghost

    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    List<GameObject> settlements = new List<GameObject>();
    List<GameObject> cities = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    GameObject placementGhost;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            settlements.Add(Instantiate(settlementPrefab, transform));
        }
        for (int i = 0; i < 5; i++)
        {
            cities.Add(Instantiate(cityPrefab, transform));
        }
        for (int i = 0; i < 15; i++)
        {
            roads.Add(Instantiate(roadPrefab, transform));
        }
    }

    public override void OnStartLocalPlayer()
    {
        GameManager.Instance.localPlayer = gameObject;
        playerController.CmdRegisterNewPlayer();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            Ray ray = GameManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool hitSuccess = Physics.Raycast(ray, out hit, 100f);

            if (hitSuccess)
            {
                // Inspection
                if (hit.collider.gameObject.GetComponent<Inspectable>() != null)
                {
                    GameManager.Instance.guiManager.inspectionText.text = hit.collider.gameObject.GetComponent<Inspectable>().inspectionText;
                }
                else
                {
                    GameManager.Instance.guiManager.inspectionText.text = "";
                }

                // Placement
                BoardHandler boardHandler = GameManager.Instance.GetGame().GetBoardHandler();

                VertexRenderer vertex = hit.collider.gameObject.GetComponent<VertexRenderer>();
                EdgeRenderer edge = hit.collider.gameObject.GetComponent<EdgeRenderer>();

                // Show valid placement option if possible
                if (vertex != null || edge != null)
                {
                    // Settlement
                    if (vertex != null)
                    {
                        // Show ghost for settlement
                        if (boardHandler.CanPlaceSettlement(player, vertex.col, vertex.row, vertex.vertexSpec))
                        {
                            UpdatePlacementGhost(settlementPrefab, vertex.transform);
                            playerController.CmdPlaceSettlement(vertex.col, vertex.row, (int)vertex.vertexSpec);
                        }
                        else if (boardHandler.CanPlaceCity(player, vertex.col, vertex.row, vertex.vertexSpec))
                        {
                            UpdatePlacementGhost(cityPrefab, vertex.transform);
                            playerController.CmdPlaceCity(vertex.col, vertex.row, (int)vertex.vertexSpec);
                        }
                    }

                    // Road
                    if (edge != null)
                    {
                        // Show ghost for road
                        if (boardHandler.CanPlaceRoad(player, edge.col, edge.row, edge.edgeSpec))
                        {
                            UpdatePlacementGhost(roadPrefab, edge.transform);
                            playerController.CmdPlaceRoad(vertex.col, vertex.row, (int)edge.edgeSpec);
                        }
                    }
                }
                else
                {
                    DestroyPlacementGhost();
                }
            }
        }
    }

    void LateUpdate()
    {
        player = GameManager.Instance.GetPlayerById(netId+"");
        if(player != null)
        {
            // Update materials to this player's color
            settlementPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
            roadPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);
            cityPrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", color);

            float spacing = 0;
            // Render the items in settlements, cities and roads
            for (int i = 0; i < player.storeSettlementNum; i++)
            {
                settlements[i].SetActive(true);
                settlements[i].transform.localPosition = new Vector3(spacing, 0, 0);
                spacing += 1.4f;
            }
            for(int i = player.storeSettlementNum; i < 5; i++)
            {
                settlements[i].SetActive(false);
            }

            spacing = 0;
            for (int i = 0; i < player.storeCityNum; i++)
            {
                cities[i].SetActive(true);
                cities[i].transform.localPosition = new Vector3(spacing, 0, 3);
                spacing += 1.4f;
            }
            for (int i = player.storeCityNum; i < 5; i++)
            {
                cities[i].SetActive(false);
            }

            spacing = 0;
            for (int i = 0; i < player.storeRoadNum; i++)
            {
                roads[i].SetActive(true);
                roads[i].transform.localPosition = new Vector3(spacing, 0, 5);
                spacing += 0.8f;
            }
            for (int i = player.storeRoadNum; i < 15; i++)
            {
                roads[i].SetActive(false);
            }
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
}
