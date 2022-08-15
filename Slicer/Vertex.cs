using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer
{
    class Vertex
    {
        double x, y, z;
        public int triangle_id;
        public int id;
        public static int count = 0;

        public Vertex() { }
        public Vertex(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vertex(double x, double y, double z, int triangle_id, int id = -1) : this(x, y, z)
        {
            this.triangle_id = triangle_id;
            if (id == -1)
                this.id = count++;
        }

        public bool Equals(Vertex b)
        {
            return (this.X == b.X) && (this.Y == b.Y);
        }

        public static Vertex operator -(Vertex v)
        {
            return new Vertex(-v.X, -v.Y, -v.Z);
        }

        public static Vertex operator +(Vertex a, Vertex b)
        {
            return new Vertex(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static Vertex operator -(Vertex a, Vertex b)
        {
            return a + (-b);
        }

        public static Vertex operator *(double k, Vertex v)
        {
            return new Vertex(k * v.X, k * v.Y, k * v.Z);
        }
        public static Vertex operator *(Vertex v, double k)
        {
            return new Vertex(k * v.X, k * v.Y, k * v.Z);
        }

        public static Vertex operator /(double k, Vertex v)
        {
            return v * (1 / k);
        }

        public static Vertex operator /(Vertex v, double k)
        {
            return v * (1 / k);
        }




        public double X { get { return x; } }
        public double Y { get { return y; } }
        public double Z { get { return z; } }
    }
}
