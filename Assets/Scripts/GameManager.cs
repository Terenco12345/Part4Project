using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardBehaviour board;

    // Start is called before the first frame update
    void Start()
    {
        board.SetupBoard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
