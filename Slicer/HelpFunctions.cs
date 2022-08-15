using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Slicer
{
    static class HelpFunctions
    {
        static bool vertexChecked = false;
        public static void GetLayerVertecies(ref List<Vertex> layer, double z, Triangle[] triangles)
        {
            foreach (Triangle triangle in triangles)
            {
                int oldCount = layer.Count();
                Vertex
                    v1 = EvalVertex(triangle.getV1(), triangle.getV2(), z),
                    v2 = EvalVertex(triangle.getV2(), triangle.getV3(), z),
                    v3 = EvalVertex(triangle.getV3(), triangle.getV1(), z);

                if (v1 != null)
                    layer.Add(v1);
                if (v2 != null)
                    layer.Add(v2);
                if (v3 != null)
                    layer.Add(v3);
            }
        }

        public static Vertex EvalVertex(Vertex a, Vertex b, double z)
        {
            double t;

            if (z >= a.Z && z <= b.Z || z >= b.Z && z <= a.Z)
            {
                if (z == a.Z)
                {
                    if (vertexChecked)
                    {
                        vertexChecked = false;
                        return null;
                    }
                    vertexChecked = true;
                }

                if (b.Z - a.Z != 0)
                    t = (z - a.Z) / (b.Z - a.Z);
                else
                    t = 0;

                return new Vertex((b.X - a.X) * t + a.X, (b.Y - a.Y) * t + a.Y, z, a.id);
            }
            else
                return null;
        }
    }
}
