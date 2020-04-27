
using System;
using UnityEngine;

public struct MapObjectKey : IEquatable<MapObjectKey>
{
    public string sortingLayer;
    public Vector2Int cell;

    override public bool Equals(object obj)
    {
        return obj is MapObjectKey && Equals((MapObjectKey)obj);
    }

    public bool Equals(MapObjectKey other)
    {
        return sortingLayer == other.sortingLayer && cell == other.cell;
    }

    static public bool operator ==(MapObjectKey object1, MapObjectKey object2)
    {
        return object1.sortingLayer == object2.sortingLayer && object1.cell == object2.cell;
    }

    static public bool operator !=(MapObjectKey object1, MapObjectKey object2)
    {
        return object1.sortingLayer != object2.sortingLayer || object1.cell != object2.cell;
    }

    public override int GetHashCode()
    {
        return sortingLayer.GetHashCode() + cell.GetHashCode();
    }
}