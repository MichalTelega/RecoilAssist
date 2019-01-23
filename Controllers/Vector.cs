namespace RecoilAssist.Controllers
{
    public struct Vector
    {
        public int x;
        public int y;

        public static readonly Vector Zero = new Vector(0,0);

        public Vector(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.x + b.x , a.y + b.y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.x - b.x, a.y - b.y);
        }

        public override string ToString() => $"<{x}, {y}>";
    }
}
