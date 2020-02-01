using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implemented with Chebychev distance
public class DijkstrasAlgorithm
{
	int mapWidth;
	int mapHeight;

	// Encourage cardinal paths
	float moveCostCardinal = 1f;
	float moveCostDiagonal = 1.0001f; 

	Node[,] nodes;
	List<Node> nodesToExplore = new List<Node>();

	public class Node
	{
		public Vector2Int position;
		public float cost = float.PositiveInfinity;
		public bool isBlocked;
		public bool isTempBlocked;
		public bool isExplored = false;
		public Node parent;
		public int X { get { return position.x; } }
		public int Y { get { return position.y; } }

		public Node(Vector2Int position, bool isBlocked)
		{
			this.position = position;
			this.isBlocked = isBlocked;
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

		public void Refresh()
		{
			cost = float.PositiveInfinity;
			isTempBlocked = false;
			isExplored = false;
			parent = null;
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

	public List<Vector2Int> FindPath(
		Vector2Int start,
		Vector2Int end,
		List<Vector2Int> tempBlockedPositions = null)
	{
		if (start == end) return null;

		RefreshNodes();
		UpdateBlockedNodes(tempBlockedPositions);
		SetupStartNode(start);
		SetupEndNode(end);

		while (nodesToExplore.Count > 0)
		{
			Node node = ExploreNextLowestNode();
			Node[] neighborNodes = NodesAroundPosition(node.position);

			for (int i = 0; i < neighborNodes.Length; i++)
			{
				if (neighborNodes[i].isExplored ||
					neighborNodes[i].isBlocked ||
					neighborNodes[i].isTempBlocked)
				{
					continue;
				}

				// If i is even, we're looking in a cardinal direction
				float curCost = i % 2 == 0 ? moveCostCardinal : moveCostDiagonal;

				if (node.cost + curCost < neighborNodes[i].cost)
				{
					neighborNodes[i].cost = node.cost + curCost;
					neighborNodes[i].parent = node;
				}

				if (neighborNodes[i].position == end)
				{
					return neighborNodes[i].PositionChain();
				}

				if (!nodesToExplore.Contains(neighborNodes[i]))
				{
					nodesToExplore.Add(neighborNodes[i]);
				}
			}
		}

		return null;
	}

	void RefreshNodes()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				nodes[x, y].Refresh();
			}
		}

		nodesToExplore.Clear();
	}

	void UpdateBlockedNodes(List<Vector2Int> tempBlockedPositions)
	{
		if (tempBlockedPositions != null)
		{
			foreach (var position in tempBlockedPositions)
			{
				NodeAtPosition(position).isTempBlocked = true;
			}
		}
	}

	void SetupStartNode(Vector2Int start)
	{
		// The start node costs nothing to move to,
		// and it does not block itself
		nodesToExplore.Add(NodeAtPosition(start));
		nodesToExplore[0].cost = 0;
		nodesToExplore[0].isTempBlocked = false;
	}

	void SetupEndNode(Vector2Int end)
	{
		// The end node does not block the path
		NodeAtPosition(end).isTempBlocked = false;
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
			NodeAtPosition(x, y + 1),     // up
			NodeAtPosition(x + 1, y + 1), // up right
			NodeAtPosition(x + 1, y),     // right
			NodeAtPosition(x + 1, y - 1), // down right
			NodeAtPosition(x, y - 1),     // down
			NodeAtPosition(x - 1, y - 1), // down left
			NodeAtPosition(x - 1, y),     // left
			NodeAtPosition(x - 1, y + 1)  // up left
		};
	}

	Node ExploreNextLowestNode()
	{
		Node lowestNode = nodesToExplore[0];

		foreach (var node in nodesToExplore)
		{
			if (node.cost < lowestNode.cost)
			{
				lowestNode = node;
			}
		}

		nodesToExplore.Remove(lowestNode);
		lowestNode.isExplored = true;

		return lowestNode;
	}
}
