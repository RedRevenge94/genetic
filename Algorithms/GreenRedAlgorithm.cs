using Graph_Coloring.Graph;
using Microsoft.Msagl.Drawing;

namespace Graph_Coloring.Algorithms {
    public class GreenRedAlgorithm : BaseAlgorithm {

        public GreenRedAlgorithm(GraphModel graphModel) : base(graphModel) { }

        override protected void RunAlgorithm() {

            for (int i = 0; i < graphModel.vertexes.Count; i++) {

                if (i % 2 == 0) {
                    graphModel.vertexes[i].ColorId = 1;
                } else {
                    graphModel.vertexes[i].ColorId = 2;
                }

            }

        }

    }
}
