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
}
