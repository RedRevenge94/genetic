using Graph_Coloring.Graph;
using System;
using System.Collections.Generic;

namespace Graph_Coloring.Algorithms {
    public class BaseAlgorithm {

        protected GraphModel graphModel;

        public BaseAlgorithm(GraphModel graphModel) {
            this.graphModel = graphModel;
        }

        public double executionTime { get; private set; }
        public void Start() {
            DateTime startDt = DateTime.Now;
            RunAlgorithm();
            CheckCorrect();

            DateTime endDt = DateTime.Now;

            executionTime = (endDt - startDt).TotalSeconds;
            Console.WriteLine($"Znalezienie rozwiazania zajelo {executionTime} sekund");
        }

        virtual protected void RunAlgorithm() { }

        public int Colors { get; private set; }
        public int Errors { get; private set; }
        protected void CheckCorrect() {
            foreach (Vertex vertex in graphModel.vertexes) {

                foreach (Vertex verNeighbor in graphModel.GetNeighborVertex(vertex)) {

                    if (vertex.ColorId == verNeighbor.ColorId) {
                        vertex.IsCorrect = false;
                        verNeighbor.IsCorrect = false;
                    }

                }
            }

            Colors = GetCountOfColors();
            Errors = graphModel.vertexes.FindAll(x => x.IsCorrect == false).Count;

            Console.WriteLine("Zakończono poszukiwania rozwiazania");
            Console.WriteLine($"Użyto {Colors} różnych kolorów i popelniono" +
                $" {Errors} błędów powtórzeń.");

        }

        ///Do poprawy metoda bo wolno działa
        protected bool CheckCorrect(Vertex vertex) {
            foreach (Vertex verNeighbor in graphModel.GetNeighborVertex(vertex)) {
                if (vertex.ColorId == verNeighbor.ColorId) {
                    return false;
                }
            }
            return true;
        }

        protected int GetCountOfColors() {

            Dictionary<int, int> countOfColorsDictionary = new Dictionary<int, int>();
            int countOfColors = 0;

            foreach (Vertex vertex in graphModel.vertexes) {

                // zliczanie ilosci wystapien kolorow ewentualnie na przyszlosc
                if (!countOfColorsDictionary.ContainsKey(vertex.ColorId)) {
                    countOfColorsDictionary.Add(vertex.ColorId, 1);
                    countOfColors++;
                } else {
                    countOfColorsDictionary[vertex.ColorId]++;
                }

            }
            return countOfColors;
        }

    }
}
