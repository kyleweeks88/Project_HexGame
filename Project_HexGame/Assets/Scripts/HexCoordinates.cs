using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    //public static float xOffset = 2f, yOffset = 1f, zOffset = 1.8f;
    public static float xOffset = 4f, yOffset = 2f, zOffset = 3.6f;

    internal Vector3Int GetHexCoords() => offsetCoordinates;

    [Header("Offset Coordinates")]
    [SerializeField] private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    /// <summary>
    /// Converts the position of the passed Vector3 to be alligned with -
    /// the whole number grid system of the Hexagon Grid coordinates.
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    public static Vector3Int ConvertPositionToOffset(Vector3 _position)
    {
        int x = Mathf.CeilToInt(_position.x / xOffset);
        int y = Mathf.RoundToInt(_position.y / yOffset);
        int z = Mathf.RoundToInt(_position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
}
