using Mapsui;
using Mapsui.Tiling;
using MapsuiMAUIMemoryLeakTest.MemoryToolkit;
using Map = Mapsui.Map;

namespace MapsuiMAUIMemoryLeakTest
{
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();

            MapControl.Map = new Map();
            MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
        }

        protected override void OnDisappearing()
        {
            VisualLeakCheckQueue.Enqueue(this);
            base.OnDisappearing();
            MapControl.Dispose();
            VisualLeakCheckQueue.Monitor();
        }
    }
}
