using Graph_Coloring.Graph;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Graph_Coloring.Algorithms;
using System.Collections.Generic;
using System;

class ViewerSample {
    public static void Main() {

        bool isDataRegisterMode = false;

        GraphModel graphModel = new GraphModel(1);
        List<ResultModel> results = new List<ResultModel>();

        if (isDataRegisterMode) {

            int[] populations = new int[] { 150, 300, 600 };
            float[] pc = new float[] { 0.5f };
            float[] pm = new float[] { 0.05f };
            float[] selectionThresholds = new float[] { 0.7f };

            for (int i = 0; i < populations.Length; i++) {

                for (int j = 0; j < selectionThresholds.Length; j++) {

                    for (int k = 0; k < pc.Length; k++) {

                        for (int l = 0; l < pm.Length; l++) {
                            graphModel = new GraphModel("data/data.csv");

                            ClassicAlgorithm testAlgorithm = new ClassicAlgorithm(graphModel, populations[i], selectionThresholds[j], pc[k], pm[l]);

                            results.Add(new ResultModel {
                                Id = i + 1,
                                Population = populations[i],
                                Generatoins = testAlgorithm.Generation,
                                MaxResult = testAlgorithm.MaxResult,
                                AvgResult = testAlgorithm.AvgResult,
                                MinResult = testAlgorithm.MinResult,
                                Colors = testAlgorithm.Colors,
                                Errors = testAlgorithm.Errors,
                                ExecutionTime = testAlgorithm.executionTime,
                                SelectionThreshold = testAlgorithm.selectionThreshold,
                                PC = testAlgorithm.pc,
                                PM = testAlgorithm.pm
                            });
                        }
                    }
                }
            }

            ResultModel.SaveResults(results);
        } else {
            //graphModel = new GraphModel(14);
            graphModel = new GraphModel("data/data_5.csv");
            ClassicAlgorithm testAlgorithm = new ClassicAlgorithm(graphModel, 150, 0.8f, 0.6f, 0.02f);
        }

        #region Rysuj wykres
        Form form = new Form();
        GViewer viewer = new GViewer();

        viewer.Graph = graphModel.GetGraph();

        form.SuspendLayout();
        viewer.Dock = DockStyle.Fill;
        form.Controls.Add(viewer);
        form.ResumeLayout();
        form.Size = new System.Drawing.Size(1024, 768);
        ///show the form
        form.ShowDialog();

        #endregion
    }
}