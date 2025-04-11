using System;
using System.Windows;
using System.Windows.Controls;
using TPW.Presentation.ViewModel;

namespace TPW.Presentation.View
{
    public partial class MainWindow : Window
    {
        private BallViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BallCountTextBox.Text, out int count) && count > 0)
            {
                _viewModel?.Dispose();

                _viewModel = new BallViewModel(count);
                DataContext = _viewModel;

                BallCanvas.Children.Clear();
                foreach (var ellipse in _viewModel.Balls)
                {
                    BallCanvas.Children.Add(ellipse);
                }
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę kulek (większą od zera).", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _viewModel?.Dispose();
        }
    }
}
