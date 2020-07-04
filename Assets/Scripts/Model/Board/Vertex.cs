public class Vertex
{
    public Settlement settlement;

    public Vertex()
    {
        settlement = null;
    }

    public bool Equals(Vertex other)
    {
        if (this.settlement == null)
        {
            if (other.settlement == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (this.settlement.Equals(other.settlement))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override string ToString()
    {
        string vertexString = "Vertex: ";
        if (settlement != null)
        {
            vertexString += settlement.ownerId + ", ";
            vertexString += settlement.isCity;
        }
        return vertexString;
    }
}
