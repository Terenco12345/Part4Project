using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : NetworkBehaviour
{
    public static ModelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public BoardBehaviour board;
    public List<TileRenderer> tileSets;

    [SyncVar]
    public int tileSetSelection;
    int previousTileSetSelection;

    // Start is called before the first frame update
    void Start()
    {
        previousTileSetSelection = tileSetSelection;
    }

    // Update is called once per frame
    void Update()
    {
        if(board.gridFaces != null)
        {
            // Check if selection changed
            if(tileSetSelection != previousTileSetSelection)
            {
                UpdateTiles();
            }
        }
        previousTileSetSelection = tileSetSelection;
    }

    void UpdateTiles()
    {
        foreach (GameObject tileObject in board.gridFaces)
        {
            if (tileObject != null)
            {
                TileRenderer tileRenderer = tileObject.GetComponent<TileRenderer>();

                if (tileRenderer != null)
                {
                    tileRenderer.noneTile = tileSets[tileSetSelection].noneTile;
                    tileRenderer.forestTile = tileSets[tileSetSelection].forestTile;
                    tileRenderer.hillTile = tileSets[tileSetSelection].hillTile;
                    tileRenderer.meadowTile = tileSets[tileSetSelection].meadowTile;
                    tileRenderer.mountainTile = tileSets[tileSetSelection].mountainTile;
                    tileRenderer.fieldTile = tileSets[tileSetSelection].fieldTile;
                    tileRenderer.desertTile = tileSets[tileSetSelection].desertTile;

                    tileRenderer.UpdateTileObject();
                }
            }
        }
    }

    public void ToggleSelection()
    {
        PlayerCustomizationController playerCustomizationController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerCustomizationController>();
        if (tileSetSelection + 1 >= tileSets.Count)
        {
            playerCustomizationController.CmdChangeSelection(0);
        } else
        {
            playerCustomizationController.CmdChangeSelection(tileSetSelection + 1);
        }
    }
}
