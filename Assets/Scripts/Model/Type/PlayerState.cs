using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    SETUP, // First two turns are setup turns
    ROLLING, // Player before rolling dice 
    ROBBING, // Player in the middle of robbing another player
    TRADING, // Player in the trading phase
    BUILDING, // Player in the building phase
}