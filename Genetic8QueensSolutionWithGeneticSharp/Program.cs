using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using Graph_Coloring.Graph;

namespace Genetic8QueensSolutionWithGeneticSharp {
    class Program {
        static void Main(string[] args) {
            //

            GraphModel graphModel = new GraphModel("data/data_5.csv");


            int vezirSayisi = graphModel.vertexes.Count;

            var selection = new EliteSelection();
            var crossover = new TwoPointCrossover();
            var mutation = new ReverseSequenceMutation();
            var fitness = new MyProblemFitness(graphModel);
            var chromosome = new MyProblemChromosome(vezirSayisi);
            var population = new Population(5000, 5500, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation) {
                //Termination = new FitnessThresholdTermination(vezirSayisi * (vezirSayisi - 1) / 2)
                Termination = new FitnessThresholdTermination(100)
            };

            int index = 0;

            ga.GenerationRan += delegate {
                var bestChromosome = ga.Population.BestChromosome;

                Console.Write("Index: " + index);
                Console.Write(", Fitness: {0}", bestChromosome.Fitness);

                Console.Write(", Genes: {0}", string.Join("-", bestChromosome.GetGenes()));

                bestChromosome.GetGenes();
                Console.Write($", Ilość kolorow: {MyProblemFitness.GetCountOfColors(bestChromosome.GetGenes())}");

                Console.WriteLine();

                index++;
            };

            Console.WriteLine("GA running...");

            ga.Start();

            Console.WriteLine("Best solution found has {0} fitness.", ga.BestChromosome.Fitness);

            Console.Read();
        }
    }

    public class MyProblemFitness : IFitness {

        GraphModel graphModel;

        public MyProblemFitness(GraphModel gm) {
            graphModel = gm;
        }

        public double Evaluate(IChromosome chromosome) {
            var genes = chromosome.GetGenes();

            double result = 99;

            for (int i = 0; i < genes.Length; i++) {

                List<Vertex> neiVertexes = graphModel.GetNeighborVertex(graphModel.vertexes[i]);

                foreach (Vertex vertex in neiVertexes) {
                    if ((int)genes[i].Value == (int)genes[Int32.Parse(vertex.Name) - 1].Value) {
                        //Powtorzenie koloru
                        result -= 5;
                    }
                }

            }

            result -= GetCountOfColors(genes) * 5;

            return result;
        }

        public static int GetCountOfColors(Gene[] genes) {

            List<int> colors = new List<int>();
            for (int i = 0; i < genes.Length; i++) {

                if (colors.Contains((int)genes[i].Value) == false) {
                    colors.Add((int)genes[i].Value);
                }
            }

            return colors.Count;
        }

    }

    public class MyProblemChromosome : ChromosomeBase {
        public MyProblemChromosome(int length) : base(length) {
            CreateGenes();
        }

        public override Gene GenerateGene(int geneIndex) {
            var rnd = RandomizationProvider.Current;

            return new Gene(rnd.GetInt(0, 7));
        }

        public override IChromosome CreateNew() {
            return new MyProblemChromosome(Length);
        }
    }
}
