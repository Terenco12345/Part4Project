public class Face
{
    public Tile tile;

    public Face()
    {
        tile = null;
    }

    public bool Equals(Face other)
    {
        if(this.tile == null)
        {
            if(other.tile == null)
            {
                return true;
            } else
            {
                return false;
            }
        }

        if (this.tile.Equals(other.tile))
        {
            return true;
        } else
        {
            return false;
        }
    }

    public override string ToString()
    {
        string faceString = "Face: ";
        if(tile != null)
        {
            faceString += tile.resourceType.ToString() + ", ";
            faceString += tile.chanceValue.ToString();
        }
        return faceString;
    }
}
