using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    //Layer mask that determines if a game object is an 
    //obstacle or not 
    public LayerMask obstacleMask;
    //The world size of the grid
    public Vector2 gridWorldSize;
    //The radius of the node
    public float nodeRadius;
    //Toggles the cube debug ray on/off
    public bool toggleCubes;
    //Tile prefab used to represent the map
    public GameObject tilePrefab;
    //Standard material for a tile
    public Material standard;
    //Materials to highlight tiles
    public Material highlight;
    public Material attackHighlight;
    //A Node 2D array called grid
    Node[,] grid;

    //The world position of the bottom left part of the grid
    private Vector3 worldBottomLeft;
    //The world position of the node 
    private Vector3 worldPoint;
    //The diameter of the node
    private float nodeDiameter;
    //The size of the grid's x axis
    private int gridSizeX;
    //The size of the grid's y axis
    private int gridSizeY;
    //Determines if an area is walkable or not 
    private bool walkable;

    private void Awake()
    {
        //Calculate the diamater of the node
        nodeDiameter = nodeRadius * 2;

        //Calculate the total amount of nodes that can fit in the grid's x and y axis
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        //Calculate the world position of the bottom left of the grid
        worldBottomLeft = transform.position - Vector3.right *
            gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        CreateGrid();
    }

    public int MaxSize { get { return gridSizeX * gridSizeY; } }

    private void OnDrawGizmos()
    {
        //Draws a wire cube of the total size of the grid
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        //If grid is not null, draw a cube for every node inside the grid
        if (grid != null && toggleCubes)
        {
            foreach (Node node in grid)
            {
                //If the area of the node is walkable, color the cube white
                //If the are of the node is not walkable, color the cube red
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPos, Vector3.one * nodeDiameter);
            }
        }
    }

    private void CreateGrid()
    {
        //Assign the 'gridSizeX' and 'gridSizeY' as the max array values
        //for 'grid'
        grid = new Node[gridSizeX, gridSizeY];

        //For every index inside grid...
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                //Calculate the world position of it 
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.forward * (y * nodeDiameter + nodeRadius);

                //Check if the node is a walkable area or not
                walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask));

                //Create a tile to represent each node
                GameObject tile = Instantiate(tilePrefab);
                tile.transform.position = worldPoint - Vector3.up * 1f;
                tile.transform.localScale = new Vector3(2.2f * tile.transform.localScale.x, 1.5f * tile.transform.localScale.y, 2.2f * tile.transform.localScale.z);

                //Instantiate a node with the values of 'worldPoint' and 'walkable'
                //and assign it to the current index of the grid array
                grid[x, y] = new Node(walkable, worldPoint, tile, x, y, 1);

                if (grid[x, y].ReturnObject() != null)
                {
                    grid[x, y].cost = Mathf.Infinity;
                }

                else
                {
                    grid[x, y].cost = 1;
                }
            }
        }
    }

    public void ClearGrid()
    {
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                n.tile.GetComponent<Renderer>().material = standard;
                n.cost = 1;
            }
        }
    }

    public void HighlightGrid(HashSet<Node> final)
    {
        //Foreach node in final...
        foreach (Node n in final)
        {
            //Set material to highlight material
            n.tile.GetComponent<Renderer>().material = highlight;
        }
    }

    public void UpdateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = grid[x, y].worldPos;
                grid[x, y].walkable = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);

                if (grid[x,y].ReturnObject() != null)
                {
                    grid[x, y].cost = Mathf.Infinity;
                }

                else
                {
                    grid[x, y].cost = 1;
                }
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        //Percentage of the x world position relative to the right side of the grid
        //(e.g., 0% is the far left side of the grid, while 100% is the far right side of the grid)  
        float percentX = Mathf.Clamp01((worldPos.x + gridWorldSize.x / 2) / gridWorldSize.x);
        //Percentage of the y world position relative to the top side of the grid
        //(e.g., 0% is the bottom of the grid, while 100% is the top of the grid)  
        float percentY = Mathf.Clamp01((worldPos.z + gridWorldSize.y / 2) / gridWorldSize.y);

        //x and y position in the grid  
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    //Gets the neighbouring nodes around the current node
    //and return them as a list
    public List<Node> GetNeighbours(Node node, int range)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -range; x <= range; x++)
        {
            if (x == 0)
            {
                for (int y = -range; y <= range; y++)
                {
                    if (y == 0)
                    {
                        continue;
                    }

                    //y position of the neighbour relative to the grid
                    int checkY = node.gridY + y;

                    //If checkX and checkY is within the bounds
                    //of the grid, add to neighbours list
                    if (checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(grid[node.gridX, checkY]);
                    }
                }
            }

            else
            {
                //x position of the neighbour relative to the grid
                int checkX = node.gridX + x;

                //If checkX is within the bounds
                //of the grid, add to neighbours list
                if (checkX >= 0 && checkX < gridSizeX)
                {
                    neighbours.Add(grid[checkX, node.gridY]);
                }
            }

        }

        return neighbours;
    }

    public List<Node> GetSurroundingNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                //x position of the neighbour relative to the grid
                int checkX = node.gridX + x;
                //y position of the neighbour relative to the grid
                int checkY = node.gridY + y;

                //If checkX and checkY is within the bounds
                //of the grid, add to neighbours list
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }
}

