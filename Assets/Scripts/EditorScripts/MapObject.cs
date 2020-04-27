#if UNITY_EDITOR
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class MapObject : MonoBehaviour
{
    [Tooltip("Fill this if the SpriteRenderer is in a child, leave empty otherwise.")]
    [SerializeField] private SpriteRenderer spriteRenderer = default;

    public string SortingLayer
    {
        get
        {
            return (spriteRenderer ? spriteRenderer : GetComponent<SpriteRenderer>()).sortingLayerName;
        }
    }

    private void Start()
    {
        MapEditorModel.Register(this);
    }

    private void OnDestroy()
    {
        MapEditorModel.Remove(this);
    }
}

public class MapObjectProcess : IProcessSceneWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnProcessScene(UnityEngine.SceneManagement.Scene scene, BuildReport report)
    {
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            foreach (MapObject mapObject in go.GetComponentsInChildren<MapObject>())
                Object.DestroyImmediate(mapObject);
        }
    }
}
#endif