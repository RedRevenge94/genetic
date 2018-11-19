using Graph_Coloring.Graph;
using Microsoft.Msagl.Drawing;
using System;

namespace Graph_Coloring.Algorithms {
    public class FirstAlgorithm : BaseAlgorithm {

        public FirstAlgorithm(GraphModel graphModel) : base(graphModel) { }

        override protected void RunAlgorithm() {

            foreach (Vertex vertex in graphModel.vertexes) {

                foreach (Vertex verNeighbor in graphModel.GetNeighborVertex(vertex)) {

                    if (vertex.ColorId == verNeighbor.ColorId) {
                        verNeighbor.ColorId++;
                        //jesli sasiad nizszy to zmien siebie
                        /*if (Int32.Parse(vertex.Name) > Int32.Parse(verNeighbor.Name)) {
                            vertex.ColorId++;
                        } else {
                            verNeighbor.ColorId++;
                        }*/
                    }

                }
            }
        }
    }
}
