using System;
using System.Collections.Generic;
using System.Threading.Channels;
using ClassicSnakeGameGUI.src;

namespace ClassicSnakeGameGUI.srs;

public class GameState
{
    public int Rows { get; }
    public int Cols { get; }
    public GridValue[,] Grid { get; }
    public Direction Dir { get; private set; }
    public readonly LinkedList<Position> snakePositions = new();
    private readonly Random random = new();
    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new GridValue[rows, cols];
        Dir = Direction.Right;
        AddSnake();
        AddFood();
    }
    private void AddSnake()
    {
        int r = Rows / 2;
        for (int c = 1; c <= 3; c++)
        {
            Grid[r, c] = GridValue.Snake;
            snakePositions.AddFirst(new Position(r, c));
        }
    }
    private IEnumerable<Position> EmptyPositions()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                yield return new Position(r, c);
            }
        }
    }
    private void AddFood()
    {
        List<Position> empty = new(EmptyPositions());
        if (empty.Count == 0)
        {
            return;
        }
        Position pos = empty[random.Next(empty.Count)];
        Grid[pos.Row, pos.Col] = GridValue.Food;
    }
    public Position HeadPosition()
    {
        return snakePositions.First.Value;
    }
    public Position TailPosition()
    {
        return snakePositions.Last.Value;
    }
    public IEnumerable<Position> SnakePositions()
    {
        return snakePositions;
    }
    private void AddHead(Position pos)
    {
        snakePositions.AddFirst(pos);
        Grid[pos.Row, pos.Col] = GridValue.Snake;
    }
    private void RemoveTail()
    {
        Position tail = snakePositions.Last.Value;
        Grid[tail.Row, tail.Col] = GridValue.Empty;
        snakePositions.RemoveLast();
    }
    public void Changedirection(Direction dir)
    {
        Dir = dir;
    }

}
