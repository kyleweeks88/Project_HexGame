using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, HexTile> hexTileDict = new Dictionary<Vector3Int, HexTile>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighborDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    private void Start()
    {
        foreach (HexTile hex in FindObjectsOfType<HexTile>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }
    }

    public HexTile GetHexTileAt(Vector3Int hexCoordinates)
    {
        HexTile result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    /// <summary>
    /// This function is used to find the neighbors of the HexTile coordinates 
    /// passed to it as a parameter.
    /// </summary>
    /// <param name="_hexCoordinates"></param>
    /// <returns></returns>
    public List<Vector3Int> GetNeighborsFor(Vector3Int _hexCoordinates)
    {
        // Check if the _hexCoordinates exist in the dictionary which
        // contains all of the hextiles in the scene.
        if (hexTileDict.ContainsKey(_hexCoordinates) == false)
            // If the _hexCoordinates don't exist, return a new empty list.
            return new List<Vector3Int>();

        // Check if the _hexCoordinates exist in this neighbors dictionary
        // then returned the cached value.
        if (hexTileNeighborDict.ContainsKey(_hexCoordinates))
            return hexTileNeighborDict[_hexCoordinates];


        // If the _hexCoordinates passes the above checks then -
        // add the _hexCoordinates and create a new List<Vector3Int>
        hexTileNeighborDict.Add(_hexCoordinates, new List<Vector3Int>());

        // Check each direction in the List<Vector3Int> returned from -
        // the function GetDirectionList with the passed _hexCoordindates.z
        foreach (Vector3Int direction in Direction.GetDirectionList(_hexCoordinates.z))
        {
            // Check the dictionary that contains all the hextiles for the currently
            // passed _hexTileCoordinates with the added offset of each direction 
            // from the List<Vector3Int> returned from GetDirectionFromList.
            if(hexTileDict.ContainsKey(_hexCoordinates + direction))
            {
                // If any hex coord + direction offset is found, populate the -
                // neighbor dictionary by passing _hexCoordinates as the Key and -
                // adding the hextile + direction offset to the list created above.
                hexTileNeighborDict[_hexCoordinates].Add(_hexCoordinates + direction);
            }
        }

        // You then return this with the newly created and populated list of -
        // neighbors that was created above... WHEW!
        return hexTileNeighborDict[_hexCoordinates];
    }

    /// <summary>
    /// Takes the world position of the passed Vector3 and converts it
    /// to fit the offset coordinates of the Hex Grid. Then returns the 
    /// closest Hex Tile position to the passed Vector3.
    /// </summary>
    /// <param name="_worldPos"></param>
    /// <returns></returns>
    public Vector3Int GetClosestHexCoords(Vector3 _worldPos)
    {
        _worldPos.y = 0;
        return HexCoordinates.ConvertPositionToOffset(_worldPos);
    }
}

public static class Direction
{
    public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1,0,1), // N1
        new Vector3Int(0,0,1), // N2
        new Vector3Int(1,0,0), // E
        new Vector3Int(0,0,-1), // S2
        new Vector3Int(-1,0,-1), // S1
        new Vector3Int(-1,0,0), // W
    };
    
    public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>
    {
        new Vector3Int(0,0,1), // N1
        new Vector3Int(1,0,1), // N2
        new Vector3Int(1,0,0), // E
        new Vector3Int(1,0,-1), // S2
        new Vector3Int(0,0,-1), // S1
        new Vector3Int(-1,0,0), // W
    };

    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
}
