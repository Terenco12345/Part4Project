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
    public EnvironmentBehaviour environment;

    [Header("Tile Sets")]
    public List<TileRenderer> tileSets;
    public int tileSetSelection;

    [Header("Environments")]
    public List<GameObject> environments;
    public int environmentSelection;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void UpdateTiles()
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

    public void UpdateEnvironment()
    {
        environment.SwapEnvironment(environments[environmentSelection]);
    }

    public void ToggleTilesetSelection()
    {
        PlayerCustomizationController playerCustomizationController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerCustomizationController>();
        if (tileSetSelection + 1 >= tileSets.Count)
        {
            playerCustomizationController.ChangeTilesetSelection(0);
        }
        else
        {
            playerCustomizationController.ChangeTilesetSelection(tileSetSelection + 1);
        }
    }

    public void ToggleEnviromentSelection()
    {
        PlayerCustomizationController playerCustomizationController = GameManager.Instance.GetLocalPlayer().GetComponent<PlayerCustomizationController>();
        if (environmentSelection + 1 >= environments.Count)
        {
            playerCustomizationController.ChangeEnvironmentSelection(0);
        }
        else
        {
            playerCustomizationController.ChangeEnvironmentSelection(environmentSelection + 1);
        }
    }
}
