using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private List<Vector2Int> path = new List<Vector2Int>();
    private Vector2Int start = new Vector2Int(0, 1);
    private Vector2Int goal = new Vector2Int(4, 4);
    private Vector2Int next;
    private Vector2Int current;

    private int width = 5;
    private int height = 5;
    private float obstacleProbability = 0.3f;

    private Vector2Int position;

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private int[,] grid = new int[,] //Grid setup and obstacle management
    {
        { 0, 1, 0, 0, 0 },
        { 0, 1, 0, 1, 0 },
        { 0, 0, 0, 1, 0 },
        { 0, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0 }
    };

    private void Start()
    {
        GenerateRandomGrid(width, height, obstacleProbability);
        AddObstacle(position);
        FindPath(start, goal);
    }

    void GenerateRandomGrid(int width, int height, float obstacleProbability)
    {
        grid = new int[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == start.x && y == start.y)
                {
                    grid[x, y] = 0;
                    continue;
                }
                else if (x == goal.x && y == goal.y)
                {
                    grid[x, y] = 0;
                    continue;
                }

                float distanceToStart = Vector2Int.Distance(new Vector2Int(x, y), start);
                float distanceToGoal = Vector2Int.Distance(new Vector2Int(x, y), goal);
                float localProbability = obstacleProbability;

                if (distanceToStart < 2 || distanceToGoal < 2)
                {
                    localProbability *= 0.3f;
                }

                if (Random.value < localProbability)
                {
                    grid[x, y] = 1;
                }
                else
                {
                    grid[x, y] = 0;
                }
            }
        }
    }

    public void AddObstacle(Vector2Int position)
    {
        if (position.x >= 0 && position.x < grid.GetLength(0) && position.y >= 0 && position.y < grid.GetLength(1))
        {
            if (position == start || position == goal)
            {
                //do nothing
            }
            else
            {
                grid[position.x, position.y] = 1;
            }
        }
        else
        {
            Debug.LogWarning("Position is out of bounds: " + position);
        }
    }

    private void OnDrawGizmos()
    {
        float cellSize = 1f;

        // Draw grid cells
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                Vector3 cellPosition = new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.color = grid[x, y] == 1 ? Color.black : Color.white;
                Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
            }
        }

        // Draw path
        foreach (var step in path)
        {
            Vector3 cellPosition = new Vector3(step.x * cellSize, 0, step.y * cellSize);
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(cellPosition, new Vector3(cellSize, 0.1f, cellSize));
        }

        // Draw start and goal
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector3(start.x * cellSize, 0, start.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(goal.x * cellSize, 0, goal.y * cellSize), new Vector3(cellSize, 0.1f, cellSize));
    }

    private bool IsInBounds(Vector2Int point)
    {
        return point.x >= 0 && point.x < grid.GetLength(1) && point.y >= 0 && point.y < grid.GetLength(0);
    }

    private void FindPath(Vector2Int start, Vector2Int goal) //Pathfinding
    {
        Queue<Vector2Int> frontier = new Queue<Vector2Int>();
        frontier.Enqueue(start);

        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[start] = start;

        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
            {
                break;
            }

            foreach (Vector2Int direction in directions)
            {
                next = current + direction;

                if (IsInBounds(next) && grid[next.y, next.x] == 0 && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom[next] = current;
                }
            }
        }

        if (!cameFrom.ContainsKey(goal))
        {
            Debug.Log("Path not found.");
            return;
        }

        // Trace path from goal to start
        Vector2Int step = goal;
        while (step != start)
        {
            path.Add(step);
            step = cameFrom[step];
        }
        path.Add(start);
        path.Reverse();
    }
}
