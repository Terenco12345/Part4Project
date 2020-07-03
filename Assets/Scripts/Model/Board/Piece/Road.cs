public class Road
{
    public string ownerId;
 
    public Road()
    {
        ownerId = "";
    }

    public bool Equals(Road other)
    {
        if (this.ownerId.Equals(other.ownerId))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
