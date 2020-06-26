using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }

    public GUIManager guiManager;

    // Components
    public Camera mainCamera;
    public Board board;

    // Players
    public Player[] players;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        board.SetupBoardGrid();
        board.RandomizeTileTypes();
        board.SetupChanceTokens();
        board.MoveRobberToDesert();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * This represents the action of rolling the dice in Catan (the core game loop)
     */
    public void RollDice()
    {
        int diceOne = Random.Range(1, 7);
        int diceTwo = Random.Range(1, 7);

        int totalRoll = diceOne + diceTwo;

        guiManager.rollText.text = "You rolled a " + totalRoll;

        ProduceResources(totalRoll);
    }

    /**
     * This method will produce resources from tiles of a specific roll.
     * Each player with settlements/cities connected to these tiles will receive the resource
     * on the tile.
     */
    public void ProduceResources(int roll)
    {
        foreach(Player player in players)
        {
            foreach(GameObject cityObject in player.placedCities)
            {
                Vertex vertex = cityObject.transform.parent.GetComponent<Vertex>();
                List<GameObject> adjacentTiles = board.boardGrid.GetFacesFromVertexCoordinate(vertex.col, vertex.row, vertex.vertexSpec);
                foreach (GameObject tileObject in adjacentTiles)
                {
                    ResourceType tileResource = tileObject.GetComponent<Tile>().resourceType;
                    int rollRequirement = tileObject.GetComponent<Tile>().chanceTokenValue;

                    if (rollRequirement == roll)
                    {
                        player.AddResource(tileResource, 2);
                    }
                }
            }
            foreach (GameObject settlementObject in player.placedSettlements)
            {
                Vertex vertex = settlementObject.transform.parent.GetComponent<Vertex>();
                List<GameObject> adjacentTiles = board.boardGrid.GetFacesFromVertexCoordinate(vertex.col, vertex.row, vertex.vertexSpec);
                foreach (GameObject tileObject in adjacentTiles)
                {
                    ResourceType tileResource = tileObject.GetComponent<Tile>().resourceType;
                    int rollRequirement = tileObject.GetComponent<Tile>().chanceTokenValue;

                    if (rollRequirement == roll)
                    {
                        player.AddResource(tileResource, 1);
                    }
                }
            }
        }
    }
}
