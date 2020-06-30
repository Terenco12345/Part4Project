using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    public bool isCity;
    public Player owner;

    public Settlement()
    {
        owner = null;
        isCity = true;
    }
}
