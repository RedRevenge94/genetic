

using Graph_Coloring.Graph;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Graph_Coloring.Algorithms {
    public class ClassicAlgorithm : BaseAlgorithm {

        public ClassicAlgorithm(GraphModel graphModel) : base(graphModel) {
            selectionThreshold = 0.8f;
            pc = 0.5f;
            pm = 0.02f;
        }
        public ClassicAlgorithm(GraphModel graphModel, int population, float selectionThreshold, float pc, float pm) : base(graphModel) {
            minPopulation = population;
            maxPopulation = population;
            this.selectionThreshold = selectionThreshold;
            this.pc = pc;
            this.pm = pm;
            Start();
        }

        public float pc { get; private set; }
        public float pm { get; private set; }
        public float selectionThreshold { get; private set; }
        private int minPopulation = 2000;
        private int maxPopulation = 2000;

        public int Generation { get; private set; }
        override protected void RunAlgorithm() {

            Init();

            DateTime startDt;
            DateTime endDt;

            while (true) {
                Generation++;
                //Console.WriteLine($"Generation {Generation}");
                {
                    startDt = DateTime.Now;
                    RateChromosomes();
                    endDt = DateTime.Now;
                    //Console.WriteLine($"RateChromosomes zajelo {(endDt - startDt).TotalSeconds} sekund");
                }
                {
                    startDt = DateTime.Now;
                    if (CheckEndOfAlgorithm()) {
                        break;
                    }
                    endDt = DateTime.Now;
                    //Console.WriteLine($"CheckEndOfAlgorithm zajelo {(endDt - startDt).TotalSeconds} sekund");
                }
                {
                    startDt = DateTime.Now;
                    ChromosomesSelection();
                    endDt = DateTime.Now;
                    //Console.WriteLine($"ChromosomesSelection zajelo {(endDt - startDt).TotalSeconds} sekund");
                }
                {
                    startDt = DateTime.Now;
                    Calculate();
                    endDt = DateTime.Now;
                    //Console.WriteLine($"Calculate zajelo {(endDt - startDt).TotalSeconds} sekund");
                }
                {
                    startDt = DateTime.Now;
                    CreateNewPopulation();
                    endDt = DateTime.Now;
                    //Console.WriteLine($"CreateNewPopulation zajelo {(endDt - startDt).TotalSeconds} sekund");
                }
            }

            GetTheBestChromosome();
        }


        Random rnd;

        private int chromosomeSize;
        private int populationCount;
        private List<bool[]> population;
        private void Init() {

            rnd = new Random();
            populationCount = rnd.Next(minPopulation, maxPopulation);

            population = new List<bool[]>();
            populationRating = new int[populationCount];

            chromosomeSize = 3 * graphModel.vertexes.Count;

            for (int i = 0; i < populationCount; i++) {

                bool[] newChromosome = new bool[chromosomeSize];

                for (int j = 0; j < chromosomeSize; j++) {

                    int rndValue = rnd.Next(0, 2);

                    if (rndValue == 1) {
                        newChromosome[j] = true;
                    } else {
                        newChromosome[j] = false;
                    }
                }

                population.Add(newChromosome);
            }

        }

        private int indexOfMaxScoreChromosome;
        private int maxScore;
        private int prevMaxScore;
        private int[] populationRating;
        private void RateChromosomes() {

            prevMaxScore = maxScore;
            maxScore = 0;

            for (int i = 0; i < populationCount; i++) {

                ChangeGraphColor(population[i]);
                int scoreForColorsCorrect = 70;
                int scoreForCountOfColors = 30;

                for (int j = 0; j < graphModel.vertexes.Count; j++) {


                    if (!CheckCorrect(graphModel.vertexes[j])) {
                        scoreForColorsCorrect -= 70 / graphModel.vertexes.Count;
                    }

                    if (scoreForColorsCorrect < 0) {
                        scoreForColorsCorrect = 0;
                        break;
                    }
                }

                int countOfColors = GetCountOfColors();
                scoreForCountOfColors -= countOfColors * 30 / graphModel.vertexes.Count;

                populationRating[i] = scoreForColorsCorrect + scoreForCountOfColors;
                if (populationRating[i] <= 0) {
                    populationRating[i] = 1;
                }

                if (populationRating[i] > maxScore) {
                    maxScore = populationRating[i];
                    indexOfMaxScoreChromosome = i;
                }

            }

        }

        private bool waitingForResult;
        private int maxGeneralScore;
        private int numberOfTheSameResults;
        private bool CheckEndOfAlgorithm() {

            if (!waitingForResult) {
                if (maxScore > maxGeneralScore) {
                    maxGeneralScore = maxScore;
                    numberOfTheSameResults = 0;
                } else {
                    numberOfTheSameResults++;
                    if (numberOfTheSameResults > (400 * 150) / populationCount) {
                        waitingForResult = true;
                        numberOfTheSameResults = 0;
                    }
                }
            } else {
                if (maxScore == maxGeneralScore) {
                    return true;
                } /*else {
                    numberOfTheSameResults++;
                    if (numberOfTheSameResults > (400 * 15000) / populationCount) {
                        return true;
                    }
            }*/
            }

            return false;
        }

        private int ratingSum;
        private List<bool[]> parentPopulation;
        private List<bool[]> newPopulation;
        private void ChromosomesSelection() {

            ratingSum = 0;
            parentPopulation = new List<bool[]>();
            newPopulation = new List<bool[]>();
            bool[] choosedChromosomes = new bool[populationCount];

            foreach (var value in populationRating) {
                ratingSum += value;
            }

            int numberOfParents = (int)(populationCount * selectionThreshold);

            for (int i = 0; i < numberOfParents; i++) {

                while (true) {
                    int randomValue = rnd.Next(1, ratingSum);

                    int index = GetIndexForSelection(randomValue);
                    if (!choosedChromosomes[index]) {
                        choosedChromosomes[index] = true;
                        parentPopulation.Add(population[index]);
                        break;
                    }
                }
            }

            newPopulation = population;

            int j = 0;
            for (int i = 0; i < populationCount; i++) {
                newPopulation[i] = parentPopulation[j];

                if (++j >= parentPopulation.Count) {
                    j = 0;
                }
            }

        }

        private int GetIndexForSelection(int value) {

            int sum = 0;
            sum += populationRating[i];
            for (int i = 0; i < populationCount; i++) {

                if (value < sum) {
                    return i;
                }
            }
            return -1;
        }

        private List<int> locusPossibleValue;
        private void Calculate() {

            Dictionary<int, int> pairs = new Dictionary<int, int>();

            List<int> chromosomesIndexesList = new List<int>();

            for (int i = 0; i < populationCount; i++) {
                chromosomesIndexesList.Add(i);
            }

            for (int i = 0; i < chromosomesIndexesList.Count; i++) {
                if (!pairs.ContainsKey(i) && !pairs.ContainsValue(i)) {

                    while (true) {
                        int pairChromosome = rnd.Next(0, chromosomesIndexesList.Count - 1);

                        if (i != pairChromosome && !pairs.ContainsKey(chromosomesIndexesList[pairChromosome])
                            && !pairs.ContainsValue(chromosomesIndexesList[pairChromosome])) {
                            pairs.Add(i, chromosomesIndexesList[pairChromosome]);
                            chromosomesIndexesList.Remove(pairChromosome);
                            chromosomesIndexesList.Remove(i);
                            break;
                        }

                    }

                }
            }


            //krzyzowanie
            foreach (var pair in pairs) {

                if (((float)rnd.Next(0, 100) / 100) < pc) {

                    if (locusPossibleValue == null) {
                        locusPossibleValue = new List<int>();

                        int j = 0;
                        for (int i = 0; i < chromosomeSize; i++) {
                            ++j;
                            if (j == 3) {
                                locusPossibleValue.Add(i);
                                j = 0;
                            }
                        }
                    }

                    int locus = locusPossibleValue[rnd.Next(1, locusPossibleValue.Count - 1)];

                    bool[] firstParent = newPopulation[pair.Key];
                    bool[] secondParent = newPopulation[pair.Value];

                    for (int i = 0; i < chromosomeSize; i++) {

                        if (i < locus) {
                            newPopulation[pair.Key][i] = firstParent[i];
                            newPopulation[pair.Value][i] = secondParent[i];
                        } else {
                            newPopulation[pair.Key][i] = secondParent[i];
                            newPopulation[pair.Value][i] = firstParent[i];
                        }

                    }
                }
            }

            //mutacje
            for (int i = 0; i < populationCount; i++) {
                if (((float)rnd.Next(0, 100) / 100) < pm) {

                    int genIndex = rnd.Next(1, chromosomeSize - 1);
                    newPopulation[i][genIndex] = !newPopulation[i][genIndex];
                }
            }
        }

        private void CreateNewPopulation() {
            population = newPopulation;
        }

        public int MaxResult { get; private set; }
        public int AvgResult { get; private set; }
        public int MinResult { get; private set; }
        private void GetTheBestChromosome() {

            bool[] theBestChromosome = population[indexOfMaxScoreChromosome];
            ChangeGraphColor(theBestChromosome);
            MaxResult = maxScore;

            AvgResult = 0;
            MinResult = 100;

            foreach (var value in populationRating) {
                AvgResult += value;
                if (MinResult > value) {
                    MinResult = value;
                }
            }
            AvgResult = AvgResult / populationCount;

            Console.WriteLine($"Zdobyto {maxScore} punktów.");
        }

        private void ChangeGraphColor(bool[] chromosome) {
            for (int i = 0; i < graphModel.vertexes.Count; i++) {

                bool[] colorInformation = new bool[3];
                colorInformation[0] = chromosome[0 + i * 3];
                colorInformation[1] = chromosome[1 + i * 3];
                colorInformation[2] = chromosome[2 + i * 3];

                BitArray arr = new BitArray(colorInformation);
                byte[] data = new byte[1];
                arr.CopyTo(data, 0);
                graphModel.vertexes[i].ColorId = data[0];

            }
        }

    }
}
