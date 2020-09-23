using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using static ResourceType;

/**
 * This game manager holds player and game information.
 */
public class GameConfigManager : NetworkBehaviour
{
    // Singleton
    public static GameConfigManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public List<ResourceType>[] resourceLoadouts = new List<ResourceType>[] { 
        null,
        new List<ResourceType> { 
            Ore, Wool, Grain, 
            Lumber, Brick, Brick, Lumber, 
            Lumber, Grain, Desert, Ore, Grain, 
            Brick, Wool, Lumber, Wool,
            Wool, Ore, Grain
        },
        new List<ResourceType> {
            Ore, Ore, Ore,
            Ore, Ore, Ore, Ore,
            Ore, Ore, Desert, Ore, Ore,
            Ore, Ore, Ore, Ore,
            Ore, Ore, Ore
        },
        new List<ResourceType> {
            Grain, Lumber, Grain,
            Lumber, Wool, Wool, Lumber,
            Grain, Ore, Desert, Ore, Grain,
            Lumber, Wool, Wool, Lumber,
            Grain, Lumber, Grain
        },
    };

    public List<ResourceType> resourceLoadout = null;

    public void ChangeLoadout(int loadout)
    {
        resourceLoadout = resourceLoadouts[loadout];
    }

}