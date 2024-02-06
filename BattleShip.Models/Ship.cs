public class Ship
{
    public readonly int HORIZONTAL = 0;
    public readonly int VERTICAL = 1;

    public int Size { get; set; }
    public int Orientation { get; set; }

    public Ship(int size, int orientation)
    {
        Size = size;
        Orientation = orientation;
    }
}