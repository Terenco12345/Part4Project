using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerBehaviour : NetworkBehaviour
{
    Player player = null;

    [Header("Colours and Materials")]
    public Color color;
    public Material transparentMaterial; // Used for placement ghost

    [Header("Prefab Definitions")]
    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject cityPrefab;

    List<GameObject> settlements = new List<GameObject>();
    List<GameObject> cities = new List<GameObject>();
    List<GameObject> roads = new List<GameObject>();

    GameObject placementGhost;

    [Header("Display Position")]
    public float yOffset = 0;

    [Header("Controller")]
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
        if (isLocalPlayer && player != null && (player.state == PlayerState.BUILDING || player.state == PlayerState.SETUP))
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

                // Should only be able to place during placement phase on this player's turn.
                if (GameManager.Instance.IsPlayerTurn(player))
                {
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
                                if (Input.GetKeyDown(GameManager.PLACEMENT_KEY))
                                {
                                    playerController.CmdPlaceSettlement(vertex.col, vertex.row, (int)vertex.vertexSpec);
                                }
                            }
                            else if (boardHandler.CanPlaceCity(player, vertex.col, vertex.row, vertex.vertexSpec))
                            {
                                UpdatePlacementGhost(cityPrefab, vertex.transform);
                                if (Input.GetKeyDown(GameManager.PLACEMENT_KEY))
                                {
                                    playerController.CmdPlaceCity(vertex.col, vertex.row, (int)vertex.vertexSpec);
                                }
                            }
                        }

                        // Road
                        if (edge != null)
                        {
                            // Show ghost for road
                            if (boardHandler.CanPlaceRoad(player, edge.col, edge.row, edge.edgeSpec))
                            {
                                UpdatePlacementGhost(roadPrefab, edge.transform);
                                if (Input.GetKeyDown(GameManager.PLACEMENT_KEY))
                                {
                                    playerController.CmdPlaceRoad(edge.col, edge.row, (int)edge.edgeSpec);
                                }
                            }
                        }
                    }
                }
                
                else
                {
                    DestroyPlacementGhost();
                }
            }
        }

        if(player != null)
        {
            int playerIndex = GameManager.Instance.GetGame().GetPlayerIndexById(player.GetId());
            color = GameManager.Instance.playerColors[playerIndex];
            transform.position = GameManager.Instance.playerPositions[playerIndex];
            transform.rotation = Quaternion.Euler(GameManager.Instance.playerRotations[playerIndex]);
        }
    }

    void LateUpdate()
    {
        player = GameManager.Instance.GetPlayerById(netId+"");
        if(player != null)
        {
            float spacing = 0;
            // Render the items in settlements, cities and roads
            for (int i = 0; i < player.storeSettlementNum; i++)
            {
                settlements[i].SetActive(true);
                settlements[i].GetComponent<MeshRenderer>().material.color = color;
                settlements[i].transform.localPosition = new Vector3(spacing, yOffset, 0);
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
                cities[i].GetComponent<MeshRenderer>().material.color = color;
                cities[i].transform.localPosition = new Vector3(spacing, yOffset, 3);
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
                roads[i].GetComponent<MeshRenderer>().material.color = color;
                roads[i].transform.localPosition = new Vector3(spacing, yOffset, 5);
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
