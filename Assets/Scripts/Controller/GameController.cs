using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Game game;

    // Start is called before the first frame update
    void Start()
    {
        game = new Game();
    }

    public Game GetGame()
    {
        return game;
    }
}
