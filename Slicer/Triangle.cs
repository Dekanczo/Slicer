using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slicer
{
    class Triangle
    {
        Vertex v1, v2, v3;
        Vertex n;
        int id;
        public int Id { get { return id; } }
        public static int count = 0;

        public Triangle(Vertex v1, Vertex v2, Vertex v3, Vertex n)
        {
            id = count;
            this.v1 = v1;
            this.v1.id = id;
            this.v2 = v2;
            this.v2.id = id;
            this.v3 = v3;
            this.v3.id = id;
            this.n = n;
            count++;
        }
        public ref readonly Vertex getV1()
        {
            return ref v1;
        }

        public ref readonly Vertex getV2()
        {
            return ref v2;
        }

        public ref readonly Vertex getV3()
        {
            return ref v3;
        }
        public ref readonly Vertex getN()
        {
            return ref n;
        }

    }
}
