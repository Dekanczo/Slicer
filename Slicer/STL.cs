using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Slicer
{
    class STL
    {
        static Regex numericTemplate = new Regex(@"(-?\d+\.?\d*)");
        
        Triangle[] triangles;
        Vertex[] vertices;

        public STL(string input)
        {
            String stlText = new StreamReader(input).ReadToEnd();
            Triangle.count = 0;
            Vertex.count = 0;
            ASCIIProtocol(stlText);
        }

        void ASCIIProtocol(String stlText)
        {
            string[] stlByLine = stlText.Split(new char[] { '\n' });
            int triangleId;
            int triangleNum = (stlByLine.Length - 2) / 7;
            triangles = new Triangle[triangleNum];
            vertices = new Vertex[triangleNum * 3];

            for (int k = 1, vrtxCount = 0; k < stlByLine.Length - 2; k += 7, vrtxCount += 3)
            {
                triangleId = (k - 1) / 7;

                triangles[triangleId] = new Triangle(ReadVertex(stlByLine[k + 2]), ReadVertex(stlByLine[k + 3]), ReadVertex(stlByLine[k + 4]), ReadVertex(stlByLine[k]));

                vertices[vrtxCount] = triangles[triangleId].getV1(); ;
                vertices[vrtxCount + 1] = triangles[triangleId].getV2();
                vertices[vrtxCount + 2] = triangles[triangleId].getV3();
            }
        }

        Vertex ReadVertex(string line)
        {
            MatchCollection matches;
            matches = numericTemplate.Matches(line);
            return new Vertex(
                Convert.ToDouble(matches[0].Value),
                Convert.ToDouble(matches[1].Value),
                Convert.ToDouble(matches[2].Value)
            );
        }

        public ref readonly Vertex GetVertex(int i)
        {
            return ref vertices[i];
        }
        public ref readonly Triangle[] getTriangles()
        {
            return ref triangles;
        }
    }
}
