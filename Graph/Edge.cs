namespace Graph_Coloring.Graph {
    public class Edge {

        public Edge(int id, int source, int target) {
            this.id = id;
            this.source = source;
            this.target = target;
        }

        public int id { get; private set; }
        public int source { get; private set; }
        public int target { get; private set; }

    }
}
