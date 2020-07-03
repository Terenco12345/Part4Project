using System.Collections.Generic;
using Mirror;

public class Player
{
    string id;

    public int storeSettlementNum;
    public int storeCityNum;
    public int storeRoadNum;

    public List<ResourceType> resources;
    public List<DevelopmentCardType> developmentCards;

    public int freeSettlements;
    public int freeRoads;

    public Player(string id)
    {
        this.id = id;

        storeSettlementNum = 5;
        storeCityNum = 5;
        storeRoadNum = 15;
        
        resources = new List<ResourceType>();
        developmentCards = new List<DevelopmentCardType>();

        freeSettlements = 0;
        freeRoads = 0;
    }

    public string GetId()
    {
        return id;
    }

    public List<ResourceType> GetResources()
    {
        return resources;
    }

    public List<DevelopmentCardType> GetDevelopmentCards()
    {
        return developmentCards;
    }

    /**
     * Reset this player's resources.
     */
    public void ResetResources()
    {
        resources.Clear();
    }

    /**
     * Add a resource to this player
     */
    public void AddResource(ResourceType type, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            resources.Add(type);
        }
    }

    /**
     * Give this player a certain amount of resources.
     */
    public void AddResources(int lumber, int wool, int grain, int brick, int ore)
    {
        for (int i = 0; i < lumber; i++)
        {
            resources.Add(ResourceType.Lumber);
        }
        for (int i = 0; i < wool; i++)
        {
            resources.Add(ResourceType.Wool);
        }
        for (int i = 0; i < grain; i++)
        {
            resources.Add(ResourceType.Grain);
        }
        for (int i = 0; i < brick; i++)
        {
            resources.Add(ResourceType.Brick);
        }
        for (int i = 0; i < ore; i++)
        {
            resources.Add(ResourceType.Ore);
        }
    }

    /**
     * Remove resources from this player.
     */
    public void RemoveResources(int lumber, int wool, int grain, int brick, int ore)
    {
        if (!CanAffordResourceTransaction(lumber, wool, grain, brick, ore))
        {
            return; // This should throw an exception instead.
        }

        // Remove resources
        for (int i = 0; i < lumber; i++)
        {
            resources.Remove(ResourceType.Lumber);
        }
        for (int i = 0; i < wool; i++)
        {
            resources.Remove(ResourceType.Wool);
        }
        for (int i = 0; i < grain; i++)
        {
            resources.Remove(ResourceType.Grain);
        }
        for (int i = 0; i < brick; i++)
        {
            resources.Remove(ResourceType.Brick);
        }
        for (int i = 0; i < ore; i++)
        {
            resources.Remove(ResourceType.Ore);
        }
    }

    /**
     * Get the amount for a certain type of resource owned by this player.
     */
    public int GetResourceCount(ResourceType type)
    {
        int count = 0;
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i] == type)
            {
                count++;
            }
        }
        return count;
    }

    /**
     * Check if player can player afford a transaction of resources.
     */
    public bool CanAffordResourceTransaction(int lumber, int wool, int grain, int brick, int ore)
    {
        // Check if there are enough resources for this action to take place.
        if (GetResourceCount(ResourceType.Lumber) >= lumber
            && GetResourceCount(ResourceType.Wool) >= wool
            && GetResourceCount(ResourceType.Grain) >= grain
            && GetResourceCount(ResourceType.Brick) >= brick
            && GetResourceCount(ResourceType.Ore) >= ore)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}