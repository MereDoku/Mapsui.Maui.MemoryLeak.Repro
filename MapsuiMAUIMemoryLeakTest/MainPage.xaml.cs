namespace MapsuiMAUIMemoryLeakTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnOpenMapClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(MapPage));
        }
    }
}
