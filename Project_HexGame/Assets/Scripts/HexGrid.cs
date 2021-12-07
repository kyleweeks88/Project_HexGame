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
            hexTileDict[hex.HexCoord] = hex;
        }
    }

    public HexTile GetTileAt(Vector3Int hexCoordinates)
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
        if (hexTileDict.ContainsKey(_hexCoordinates) == false)
            return new List<Vector3Int>();

        if (hexTileNeighborDict.ContainsKey(_hexCoordinates))
            return hexTileNeighborDict[_hexCoordinates];

        hexTileNeighborDict.Add(_hexCoordinates, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(_hexCoordinates.z))
        {
            if(hexTileDict.ContainsKey(_hexCoordinates + direction))
            {
                hexTileNeighborDict[_hexCoordinates].Add(_hexCoordinates + direction);
            }
        }
        return hexTileNeighborDict[_hexCoordinates];
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
