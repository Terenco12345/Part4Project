using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settlement
{
    public string ownerId;
    public bool isCity;

    public Settlement()
    {
        ownerId = "";
        isCity = true;
    }

    public bool Equals(Settlement other)
    {
        if (this.ownerId.Equals(other.ownerId) && this.isCity == other.isCity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
