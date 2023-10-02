using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// the food sometimes is generated on the block that belongs to the snake thus causing a bug that makes the food disapear and the game continues without the food
//add different grid sizes
//add ai solving
namespace ClassicSnakeGameGUI
{

    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridValuesToImage = new(){
        {GridValue.Empty, Images.Empty},
        {GridValue.Snake, Images.Body},
        {GridValue.Food, Images.Food}
        };
        private readonly Dictionary<Direction, int> dirToRotation = new(){
            {Direction.Up, 0},
            {Direction.Right, 90},
            {Direction.Down,180},
            {Direction.Left,270}
        };
        private GameState gameState;
        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private bool gameRunning;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new(rows, cols);
        }
        public async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await showGameOver();
            gameState = new GameState(rows, cols);
        }
        private async Task ShowCountDown()
        {
            for (int i = 3; i > 0; i--)
            {
                Overlaytext.Text = i.ToString();
                await Task.Delay(500);
            }
        }
        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;
            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }
        private async Task DrawDeadSnake()
        {
            List<Position> positions = new(gameState.SnakePositions());
            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }
        private async Task showGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            Overlaytext.Text = "PRESS ANY KEY TO START";
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }
            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;

            }
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
                    Image image = new()
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }
        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
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
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }
    }
}