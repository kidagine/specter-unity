#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

public static class MapEditorModel
{
    private readonly static Dictionary<MapObjectKey, MapObject> map = new Dictionary<MapObjectKey, MapObject>();

    public static void Register(MapObject mapObject)
    {
        map.Add(GetKey(mapObject), mapObject);
    }

    public static void Remove(MapObject mapObject)
    {
        map.Remove(GetKey(mapObject));
    }

    public static void Remove(Vector2 position, string sortingLayer)
    {
        map.Remove(new MapObjectKey()
        {
            cell = GetCell(position),
            sortingLayer = sortingLayer
        });
    }

    public static MapObject Get(Vector2 position, string sortingLayer)
    {
        MapObject mapObject = null;
        MapObjectKey key = new MapObjectKey()
        {
            cell = GetCell(position),
            sortingLayer = sortingLayer
        };

        map.TryGetValue(key,
                        out mapObject);

        return mapObject;
    }

    private static MapObjectKey GetKey(MapObject mapObject)
    {
        return new MapObjectKey()
        {
            cell = GetCell(mapObject.transform.position),
            sortingLayer = mapObject.SortingLayer
        };
    }

    public readonly static Vector2 cellSize = new Vector2(1f, 1f);

    public static Vector2Int GetCell(Vector3 coords)
    {
        return new Vector2Int(Mathf.FloorToInt(coords.x / cellSize.x), Mathf.FloorToInt(coords.y / cellSize.y));
    }
}
#endif