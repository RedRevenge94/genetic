using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Msagl.Drawing;

namespace Graph_Coloring.Graph {
    public class GraphModel {

        string path;
        public List<Vertex> vertexes;
        public List<Edge> edges { get; private set; }

        public GraphModel(int vertexCount) {

            vertexes = new List<Vertex>();
            edges = new List<Edge>();

            ParseCsv(ReadInCSV(vertexCount));
        }

        public GraphModel(string path) {

            vertexes = new List<Vertex>();
            edges = new List<Edge>();
            this.path = path;

            ParseCsv(ReadInCSV(path));
        }

        public Microsoft.Msagl.Drawing.Graph GetGraph() {
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            foreach (Edge edge in edges) {
                graph.AddEdge(edge.source.ToString(), edge.target.ToString());
            }

            foreach (Vertex vertex in vertexes) {
                Node n = graph.FindNode($"{vertex.Name}");

                if (n != null) {
                    n.Attr.FillColor = vertex.color;


                    if (vertex.IsCorrect == false) {
                        n.Attr.LineWidth = 3;
                        n.Attr.Color = Color.Red;
                    }


                    n.Attr.Shape = Shape.Circle;
                }
            }

            return graph;
        }

        Dictionary<string, List<Vertex>> neighborDictionary;
        public List<Vertex> GetNeighborVertex(Vertex vertex) {

            if (neighborDictionary != null) {
                if (neighborDictionary.ContainsKey(vertex.Name)) {
                    return neighborDictionary[vertex.Name];
                } else {
                    List<Vertex> neighbourVertex = new List<Vertex>();

                    foreach (Edge edge in edges.FindAll(x => x.source.ToString() == vertex.Name)) {
                        neighbourVertex.Add(vertexes.Find(x => x.Name == edge.target.ToString()));
                    }

                    foreach (Edge edge in edges.FindAll(x => x.target.ToString() == vertex.Name)) {

                        neighbourVertex.Add(vertexes.Find(x => x.Name == edge.source.ToString()));
                    }

                    neighborDictionary.Add(vertex.Name, neighbourVertex);
                    return GetNeighborVertex(vertex);
                }
            }
            neighborDictionary = new Dictionary<string, List<Vertex>>();
            return GetNeighborVertex(vertex);
        }

        private void ParseCsv(List<string> csvContent) {
            for (int i = 1; i <= Int32.Parse(csvContent[0]); i++) {
                string[] values = csvContent[i].Split(';');
                AddEdge(Int32.Parse(values[0]), Int32.Parse(values[1]), Int32.Parse(values[2]));

                if (vertexes.Find(x => x.Name == values[1]) == null) {
                    AddVertex($"{values[1]}");
                }
                if (vertexes.Find(x => x.Name == values[2]) == null) {
                    AddVertex($"{values[2]}");
                }
            }
        }

        private void AddVertex(string name) {
            vertexes.Add(new Vertex(name));
        }

        private void AddEdge(int id, int sourceVertex, int targetVertex) {
            edges.Add(new Edge(id, sourceVertex, targetVertex));
        }

        private static List<string> ReadInCSV(int vertexCount) {
            List<string> result = new List<string>();

            Random rd = new Random();

            int rowNumberGlobal = 0;
            result.Add($"0");

            for (int i = 0; i < vertexCount; i++) {

                int rowNumber = rd.Next(1, 4);

                for (int j = 0; j < rowNumber; j++) {
                    int source = i;
                    int target = rd.Next(1, vertexCount);

                    while (target == source) {
                        target = rd.Next(1, vertexCount);
                    }
                    result.Add($"{i};{source};{target}");
                    rowNumberGlobal++;
                }

            }
            result[0] = $"{rowNumberGlobal}";

            foreach (string line in result) {
                Console.WriteLine(line);
            }

            return result;
        }

        private static List<string> ReadInCSV(string absolutePath) {
            List<string> result = new List<string>();
            string value;
            using (TextReader fileReader = File.OpenText(absolutePath)) {
                var csv = new CsvReader(fileReader);
                csv.Configuration.HasHeaderRecord = false;
                while (csv.Read()) {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++) {
                        result.Add(value);
                    }
                }
            }
            return result;
        }

    }
}
