using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphSearch
{
    /// <summary>
    /// This function will return a list of neighbor tiles as well as determine
    /// a path of how far a unit can move from the selected tile based on movement points.
    /// </summary>
    /// <param name="_hexGrid"></param>
    /// <param name="_startPoint"></param>
    /// <param name="_movementPoints"></param>
    /// <returns></returns>
    public static BFSResult BFSGetRange(HexGrid _hexGrid, Vector3Int _startPoint, int _movementPoints)
    {
        // THIS DICTIONARY NAMING IS A BIT CONFUSING. THE UNIT DOESNT ACTUALLY
        // VISIT THE NODES WERE CHECKING HERE. THIS IS SOMETHING MORE LIKE -
        // checkedNodes OR possibleNodes
        Dictionary<Vector3Int, Vector3Int?> visitedNodes = new Dictionary<Vector3Int, Vector3Int?>();
        Dictionary<Vector3Int, int> costSoFar = new Dictionary<Vector3Int, int>();
        Queue<Vector3Int> nodesToVisitQueue = new Queue<Vector3Int>();

        nodesToVisitQueue.Enqueue(_startPoint);
        costSoFar.Add(_startPoint, 0);
        visitedNodes.Add(_startPoint, null);

        while(nodesToVisitQueue.Count > 0)
        {
            // This will automatically Dequeue the node you started the path from.
            // the node the unity is currently occupying.
            Vector3Int currentNode = nodesToVisitQueue.Dequeue();
            foreach (Vector3Int neighborPos in _hexGrid.GetNeighborsFor(currentNode))
            {
                if (_hexGrid.GetTileAt(neighborPos).IsObstacle()) continue;


                int nodeCost = _hexGrid.GetTileAt(neighborPos).GetCost();
                // Pass in the currentNode as a key to get the int value cost.
                int currentCost = costSoFar[currentNode]; // this will return zero for the first currently occupied tile.
                // Add the two int values to get the total of how much it -
                // will cost to get to the current node in this foreach loop.
                int newCost = currentCost + nodeCost;

                // If this nodes calculated newCost is greater than the Unit's -
                // total points then don't check any more neighbors for it.
                // BUT if there are still movementPoints left...
                if(newCost <= _movementPoints)
                {
                    // if you haven't visted the current node in this loop
                    if(!visitedNodes.ContainsKey(neighborPos))
                    {
                        visitedNodes[neighborPos] = currentNode;
                        costSoFar[neighborPos] = newCost;
                        nodesToVisitQueue.Enqueue(neighborPos);
                    }
                    // if the current node in this loop has been visited BUT...
                    // the current neighbor node in the loop -
                    // will cost less movement points than the others.
                    else if(costSoFar[neighborPos] > newCost)
                    {
                        costSoFar[neighborPos] = newCost;
                        visitedNodes[neighborPos] = currentNode;
                    }
                }
            }
        }
        // Pass in the visitedNodes dictionary we populated above to be -
        // checked and processed inside the struct BFSResult.
        return new BFSResult { visitedNodesDict = visitedNodes };
    }

    public static List<Vector3Int> GeneratePathBFS(Vector3Int _current, Dictionary<Vector3Int, Vector3Int?> _visitedNodesDict)
    {
        // Create a path as a new list that will start at the end node position -
        // that is considered the destination and will work backwards to the start.
        List<Vector3Int> path = new List<Vector3Int>();
        // Add the _current destination to the path.
        path.Add(_current);

        // As long as the _current destination exists within the -
        // _visitedNodesDict arg...
        while(_visitedNodesDict[_current] != null)
        {
            // We're using value because we have a Vector3Int that -
            // we want to return null? I DONT UNDERSTAND THIS PART.
            path.Add(_visitedNodesDict[_current].Value);
            _current = _visitedNodesDict[_current].Value;
        }
        // We reverse the path List so that the _current dest is at -
        // the end of the list.
        path.Reverse();
        // We skip the first index in the list because this is a node -
        // that the unit is already occupying.
        // Once all this is done, return this reversed path list of Vector3Ints -
        // and skip the first index.
        //return path.Skip(1).ToList();
        return path.ToList();
    }
}

public struct BFSResult
{
    public Dictionary<Vector3Int, Vector3Int?> visitedNodesDict;

    /// <summary>
    /// This will generate a path from the start pos to the destination by
    /// checking a list of tiles nodes that are in range of the starting node.
    /// It returns a generated BFS of List<Vector3Int>.
    /// </summary>
    /// <param name="_dest"></param>
    /// <returns></returns>
    public List<Vector3Int> GetPathTo(Vector3Int _dest)
    {
        // if the destination isn't in the checked nodes then it can't -
        // be reached and an empty list will be returned.
        if (visitedNodesDict.ContainsKey(_dest) == false)
            return new List<Vector3Int>(); // empty list for null result

        return GraphSearch.GeneratePathBFS(_dest, visitedNodesDict);
    }

    // Checks if the _pos coords arg are in range of the nodes -
    // we can reach by checking the populated visitedNodesDict.
    // If not we can just cancel the movement attempt.
    public bool IsHexPositionInRange(Vector3Int _pos)
    {
        return visitedNodesDict.ContainsKey(_pos);
    }

    // I DONT UNDERSTAND THIS COROUTINE LOGIC
    public IEnumerable<Vector3Int> GetRangePositions()
        => visitedNodesDict.Keys;
}