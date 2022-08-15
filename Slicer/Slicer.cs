using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Slicer
{
    class Slicer
    {
        public STL stl;

        public Slicer(string stlPath)
        {
            stl = new STL(stlPath);
        }

        public List<Vertex> GetLayer(double z)
        {
            List<Vertex> layer = new List<Vertex>();
            HelpFunctions.GetLayerVertecies(ref layer, z, stl.getTriangles());

            Comparer comparer = new Comparer();
            layer = layer.OrderBy<Vertex, Vertex>(key => key, comparer).ToList();

            return layer;
        }

        public class Comparer : IComparer<Vertex>
        {
            public int Compare(Vertex a, Vertex b)
            {

                double
                    dx = Math.Abs(a.X - b.X),
                    dy = Math.Abs(a.Y - b.Y),
                    e = Math.Pow(10, -6);

                if (Math.Abs(a.X - b.X) < Math.Pow(10, -13))
                {
                    if (a.Y == b.Y)
                        return 0;
                    else
                        return Math.Sign(a.Y - b.Y);
                }
                else
                    return Math.Sign(a.X - b.X);


            }
        }

        public ValueTuple<List<List<Vertex>>, int[][]> GetConturs(double z)
        {
            List<List<Vertex>> conturs = new List<List<Vertex>>();
            Func<Vertex, int> vComp = new Func<Vertex, int>(v => v.id);

            int count = 0;
            int targetId;
            int conturNum = -1;

            List<Vertex> layer = GetLayer(z);

            if (layer.Count() == 0)
                return (null, null);

            bool check = true;
            int t = 0;
            while (check && t < layer.Count / 2)
            {
                check = (layer[2*t].X == layer[2 * t + 1].X) && (layer[2*t].Y == layer[2 * t + 1].Y);
                t++;
            }

            int[] chain = new int[layer.Count() / 2 + 300];
            bool[] checkedVertecies = new bool[layer.Count()];
            count = 0;
            conturNum = 0;
            int firstId = -1;
            while (firstId + count < layer.Count() / 2)
            {
                targetId = 0;
                while (checkedVertecies[targetId])
                    targetId++;

                firstId += count + 1;
                chain[firstId] = layer[targetId].triangle_id;
                conturs.Add(new List<Vertex>());
                conturs[conturNum].Add(layer[targetId]);
                int i = firstId + 1;
                count = 0;
                do
                {

                    if (targetId % 2 == 0)
                        targetId++;
                    else
                        targetId--;

                    chain[i] = layer[targetId].triangle_id;
                    checkedVertecies[targetId] = true;

                    int k = 0;
                    while (k < layer.Count())
                    {
                        if (k == targetId || checkedVertecies[k])
                        {
                            k++;
                            continue;
                        }
                        if (layer[k].triangle_id == chain[i])
                            break;

                        k++;
                    }

                    count++;
                    targetId = k;
                    i++;

                    conturs[conturNum].Add(layer[targetId]);
                    checkedVertecies[targetId] = true;

                } while (chain[i - 1] != chain[firstId]);

                checkedVertecies[firstId] = true;
                conturNum += 1;
            }

            int[][] intendStatus = new int[conturNum][];
            for (int i = 0; i < conturNum; i++)
                intendStatus[i] = new int[2];
            
            for (int i = 0; i < conturNum; i++)
            {
                intendStatus[i][0] = i;

                for (int j = 0; j < conturNum; j++)
                    if (i != j)
                        intendStatus[j][1] += Convert.ToInt32(CheckOnIntend2(conturs[i], conturs[j])); ;
            }

            Comparer<int> comparer = Comparer<int>.Default;
            Array.Sort<int[]>(intendStatus, (x, y) => comparer.Compare(x[1], y[1]));

            return new ValueTuple<List<List<Vertex>>, int[][]>(conturs, intendStatus);
        }

        bool CheckOnIntend2(List<Vertex> contur1Source, List<Vertex> contur2Source)
        {
            bool c = false;
            for (int i = 0, j = contur1Source.Count - 1; i < contur1Source.Count; j = i++)
            {
                double
                    x = contur2Source[0].X,
                    y = contur2Source[0].Y;
                var p = contur1Source;

                if ((((p[i].Y <= y) && (y < p[j].Y)) || ((p[j].Y <= y) && (y < p[i].Y))) &&
                  (((p[j].Y - p[i].Y) != 0) && (x > ((p[j].X - p[i].X) * (y - p[i].Y) / (p[j].Y - p[i].Y) + p[i].X))))
                    c = !c;
            }
            return c;
        }



    }
}

