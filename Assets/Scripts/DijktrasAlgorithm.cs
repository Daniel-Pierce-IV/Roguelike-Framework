using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implemented with Chebychev's move rules 
public class DijktrasAlgorithm
{
	int mapWidth;
	int mapHeight;
	Node[,] nodes;
	Queue<Node> nodeQueue = new Queue<Node>();

	public class Node
	{
		public Vector2Int position;
		public bool isBlocking;
		public bool isExplored = false;
		public Node parent;
		public int X { get { return position.x; } }
		public int Y { get { return position.y; } }

		public Node(Vector2Int position, bool isBlocking)
		{
			this.position = position;
			this.isBlocking = isBlocking;
		}

		public List<Vector2Int> PositionChain()
		{
			List<Vector2Int> positionChain;

			if (parent != null)
			{
				positionChain = parent.PositionChain();
			}
			else
			{
				positionChain = new List<Vector2Int>();
			}

			positionChain.Add(position);

			return positionChain;
		}
	}

	public void GenerateNodes(RogueTile[,] roguetiles)
	{
		mapWidth = roguetiles.GetLength(0);
		mapHeight = roguetiles.GetLength(1);

		nodes = new Node[mapWidth, mapHeight];

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				nodes[x, y] = new Node(
					new Vector2Int(roguetiles[x, y].x, roguetiles[x, y].y),
					roguetiles[x, y].data.blocksMovement);
			}
		}
	}

	public List<Vector2Int> FindPath(Vector2Int start, Vector2Int end)
	{
		if (start == end) return null;

		nodeQueue.Enqueue(NodeAtPosition(start));

		while (nodeQueue.Count > 0)
		{
			Node node = nodeQueue.Dequeue();
			Node[] neighborNodes = NodesAroundPosition(node.position);
			Node neighborNode;

			for (int i = 0; i < neighborNodes.Length; i++)
			{
				neighborNode = neighborNodes[i];

				if (neighborNode.isExplored || neighborNode.isBlocking)
				{
					continue;
				}

				if (neighborNode.position == end)
				{
					return neighborNode.PositionChain();
				}

				neighborNode.isExplored = true;
				neighborNode.parent = node;
				nodeQueue.Enqueue(neighborNode);
			}
		}

		return null;
	}

	Node NodeAtPosition(Vector2Int position)
	{
		return nodes[position.x, position.y];
	}

	Node NodeAtPosition(int x, int y)
	{
		return nodes[x, y];
	}

	Node[] NodesAroundPosition(Vector2Int position)
	{
		int x = position.x;
		int y = position.y;

		return new Node[] {
			NodeAtPosition(x, y + 1),
			NodeAtPosition(x + 1, y + 1),
			NodeAtPosition(x + 1, y),
			NodeAtPosition(x + 1, y - 1),
			NodeAtPosition(x, y - 1),
			NodeAtPosition(x - 1, y - 1),
			NodeAtPosition(x - 1, y),
			NodeAtPosition(x - 1, y + 1)
		};
	}
}
