public class Edge
{
    public Road road;
    
    public Edge()
    {
        road = null;
    }

    public bool Equals(Edge other)
    {
        if (this.road == null)
        {
            if (other.road == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (this.road.Equals(other.road))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
