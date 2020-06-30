public class Tile
{
    public int chanceValue; // This is the roll required for this tile to produce resources
    public ResourceType resourceType;
    public Tile()
    {
        chanceValue = 0;
        resourceType = ResourceType.None;
    }
}