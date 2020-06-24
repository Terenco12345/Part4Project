using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance { get; private set; }
    
    // Components
    public Camera mainCamera;
    public BoardBehaviour board;

    // UI
    public Text inspectionText;

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
}
