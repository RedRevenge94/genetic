using Microsoft.Msagl.Drawing;

namespace Graph_Coloring.Graph {
    public class Vertex {

        public string Name { get; private set; }

        private int colorId;
        public int ColorId {
            set {
                colorId = value;
                color = SetColor(value);
            }
            get {
                return colorId;
            }
        }

        public Color color { get; private set; }

        public bool IsCorrect;

        public Vertex(string Name) {
            this.Name = Name;
            ColorId = 0;
            IsCorrect = true;
        }

        private Color SetColor(int colorId) {

            switch (colorId) {
                case 0: {
                        return Color.White;
                    }
                case 1: {
                        return Color.Green;
                    }
                case 2: {
                        return Color.Blue;
                    }
                case 3: {
                        return Color.Yellow;
                    }
                case 4: {
                        return Color.Violet;
                    }
                case 5: {
                        return Color.Orange;
                    }
                case 6: {
                        return Color.Purple;
                    }
                case 7: {
                        return Color.Brown;
                    }
            }

            return Color.Pink;
        }
    }
}
