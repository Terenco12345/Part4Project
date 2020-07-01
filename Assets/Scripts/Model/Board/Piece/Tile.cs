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
}