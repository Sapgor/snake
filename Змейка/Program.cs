using System;
using System.Collections.Generic;
using System.Linq;

public class SnakeGame
{
    private int width;
    private int height;
    private List<Point> snake;
    private Point food;
    private Direction direction;

    public SnakeGame(int width, int height)
    {
        this.width = width;
        this.height = height;
        snake = new List<Point> { new Point(width / 2, height / 2) };
        direction = Direction.Right;
        GenerateFood();

    }

    public void Start()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                direction = GetNewDirection(key);
            }

            MoveSnake();

            if (IsSnakeCollidingWithWall() || IsSnakeCollidingWithItself())
            {
                GameOver();
                break;
            }

            if (IsSnakeEatingFood())
            {
                snake.Add(food);
                GenerateFood();
            }

            DrawGame();
            Thread.Sleep(100);
        }
    }

    private void MoveSnake()
    {
        var head = snake.Last();
        var newHead = new Point(head.X, head.Y);

        switch (direction)
        {
            case Direction.Up:
                newHead.Y--;
                break;
            case Direction.Down:
                newHead.Y++;
                break;
            case Direction.Left:
                newHead.X--;
                break;
            case Direction.Right:
                newHead.X++;
                break;
        }

        snake.Add(newHead);
        if (!IsSnakeEatingFood())
        {
            snake.RemoveAt(0);
        }
    }

    private bool IsSnakeEatingFood()
    {
        return snake.Last().X == food.X && snake.Last().Y == food.Y;
    }

    private void GenerateFood()
    {
        var random = new Random();
        food = new Point(random.Next(width), random.Next(height));
    }

    private Direction GetNewDirection(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                return Direction.Up;
            case ConsoleKey.DownArrow:
                return Direction.Down;
            case ConsoleKey.LeftArrow:
                return Direction.Left;
            case ConsoleKey.RightArrow:
                return Direction.Right;
            default:
                return direction;
        }
    }

    private bool IsSnakeCollidingWithWall()
    {
        var head = snake.Last();
        return head.X < 0 || head.X >= width || head.Y < 0 || head.Y >= height;
    }

    private bool IsSnakeCollidingWithItself()
    {
        var head = snake.Last();
        return snake.Count > 1 && snake.Take(snake.Count - 1).Any(segment => segment.X == head.X && segment.Y == head.Y);
    }

    public delegate void ThreadStart();
    private void DrawGame()
    {
        Console.Clear();
        foreach (var segment in snake)
        {
            Console.SetCursorPosition(segment.X, segment.Y);
            Console.Write("*");
        }
        Console.SetCursorPosition(food.X, food.Y);
        Console.Write("@");
    }

    private void GameOver()
    {
        Console.SetCursorPosition(width / 2 - 4, height / 2);
        Console.Write("Game Over!");
    }
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Program
{
    public static void Main()
    {
        var game = new SnakeGame(120, 30);
        game.Start();
    }
}