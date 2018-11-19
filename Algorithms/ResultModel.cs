using System;
using System.Collections.Generic;
using System.IO;

namespace Graph_Coloring.Algorithms {
    public class ResultModel {

        public int Id;
        public int Population;
        public int Generatoins;
        public int MaxResult;
        public int MinResult;
        public int AvgResult;
        public int Colors;
        public int Errors;
        public double ExecutionTime;
        public float SelectionThreshold;
        public float PC;
        public float PM;

        public static void SaveResults(List<ResultModel> results) {
            string resultCsvContent = "Id;Populacja;Generacje;Prog selekcji;PC;PM;Czas [s];Max punkty;Min punkty;Srednia punkty;Kolory;Bledy\n";
            foreach (var result in results) {
                resultCsvContent += $"{result.Id};{result.Population};{result.Generatoins};{result.SelectionThreshold};" +
                    $"{result.PC};{result.PM};{result.ExecutionTime};{result.MaxResult};{result.MinResult};{result.AvgResult};" +
                    $"{result.Colors};{result.Errors}\n";
            }

            File.WriteAllText($"data/{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.csv", resultCsvContent);
        }

    }
}
