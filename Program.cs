// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

// Command Interface
interface ICommand
{
    void Execute();
}

// Rover Class
class Rover
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public string Direction { get; private set; }

    private readonly Grid _grid;

    public Rover(int x, int y, string direction, Grid grid)
    {
        X = x;
        Y = y;
        Direction = direction;
        _grid = grid;
    }

    // Move Rover forward in the direction it is facing
    public void Move()
    {
        int nextX = X, nextY = Y;

        switch (Direction)
        {
            case "N":
                nextY++;
                break;
            case "S":
                nextY--;
                break;
            case "E":
                nextX++;
                break;
            case "W":
                nextX--;
                break;
        }

        if (_grid.IsValidMove(nextX, nextY))
        {
            X = nextX;
            Y = nextY;
        }
        else
        {
            Console.WriteLine("Obstacle detected! Can't move.\n Move away from the Obstacle!");
        }
    }

    // Turn Rover Left
    public void TurnLeft()
    {
        Direction = Direction switch
        {
            "N" => "W",
            "W" => "S",
            "S" => "E",
            "E" => "N",
            _ => Direction
        };
    }

    // Turn Rover Right
    public void TurnRight()
    {
        Direction = Direction switch
        {
            "N" => "E",
            "E" => "S",
            "S" => "W",
            "W" => "N",
            _ => Direction
        };
    }

    public void ReportPosition()
    {
        Console.WriteLine($"Rover is at position ({X}, {Y}) facing {Direction}");
    }
}

// Move Command
class MoveCommand : ICommand
{
    private readonly Rover _rover;

    public MoveCommand(Rover rover)
    {
        _rover = rover;
    }

    public void Execute()
    {
        _rover.Move();
    }
}

// Turn Left Command
class TurnLeftCommand : ICommand
{
    private readonly Rover _rover;

    public TurnLeftCommand(Rover rover)
    {
        _rover = rover;
    }

    public void Execute()
    {
        _rover.TurnLeft();
    }
}

// Turn Right Command
class TurnRightCommand : ICommand
{
    private readonly Rover _rover;

    public TurnRightCommand(Rover rover)
    {
        _rover = rover;
    }

    public void Execute()
    {
        _rover.TurnRight();
    }
}

// Grid Class using Composite Pattern
class Grid
{
    private readonly int _width;
    private readonly int _height;
    private readonly HashSet<(int, int)> _obstacles;

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;
        _obstacles = new HashSet<(int, int)>();
    }

    public void AddObstacle(int x, int y)
    {
        _obstacles.Add((x, y));
    }

    public bool IsValidMove(int x, int y)
    {
        // Check if the move is within bounds and not an obstacle
        return x >= 0 && x < _width && y >= 0 && y < _height && !_obstacles.Contains((x, y));
    }
}

// Command Invoker
class CommandInvoker
{
    private readonly List<ICommand> _commands = new List<ICommand>();

    public void SetCommand(ICommand command)
    {
        _commands.Add(command);
    }

    public void ExecuteCommands()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Initialize the grid and obstacles
        Grid grid = new Grid(10, 10);
        grid.AddObstacle(8, 6);
        grid.AddObstacle(4, 9);

        // Initialize the rover
        Rover rover = new Rover(1, 3, "S", grid);

        // Initialize the command invoker
        CommandInvoker invoker = new CommandInvoker();

        // Define the commands
        invoker.SetCommand(new MoveCommand(rover));
        invoker.SetCommand(new TurnRightCommand(rover));
        invoker.SetCommand(new MoveCommand(rover));
        invoker.SetCommand(new TurnLeftCommand(rover));
        invoker.SetCommand(new MoveCommand(rover));

        // Execute all commands
        invoker.ExecuteCommands();

        // Final position report
        rover.ReportPosition();
    }
}
