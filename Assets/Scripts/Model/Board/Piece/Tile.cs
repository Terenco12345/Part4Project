public class Tile
{
    public string ownerId;
    public int chanceValue; // This is the roll required for this tile to produce resources
    public ResourceType resourceType;
    public Tile()
    {
        ownerId = "";
        chanceValue = 0;
        resourceType = ResourceType.None;
    }

    public bool Equals(Tile other)
    {
        if(this.ownerId.Equals(other.ownerId) && this.chanceValue == other.chanceValue && this.resourceType == other.resourceType)
        {
            return true;
        } else
        {
            return false;
        }
    }
}