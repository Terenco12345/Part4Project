using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinDisplayBehaviour : MonoBehaviour
{
    public Text winDisplayText;

    // Update is called once per frame
    void Update()
    {
        winDisplayText.text = "Player "+GameManager.Instance.GetGame().GetPlayerIndexById(VictoryPointManager.Instance.winnerId) + " has won!";
    }
}
