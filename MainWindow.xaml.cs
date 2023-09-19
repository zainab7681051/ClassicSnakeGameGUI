using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ClassicSnakeGameGUI
{

    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValuesToImage = new(){
        {GridValue.Empty, Images.Empty},
        {GridValue.Snake, Images.Body},
        {GridValue.Food, Images.Food}
    };
        private readonly GameState gameState;
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new(rows, cols);
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver) { return; }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.Changedirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.Changedirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.Changedirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.Changedirection(Direction.Down);
                    break;
            }
        }
        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
            }
        }
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new() { Source = Images.Empty };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }
        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValuesToImage[gridVal];
                }
            }
        }
    }
}
//56:00