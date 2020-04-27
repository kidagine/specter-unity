using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilemapStyx : EditorWindow
{
    private enum SORTING_LAYERS { Background, Midground, Foreground};
    private enum COLLISION_LAYERS { Collision, NoCollision };

    private readonly List<MapObject> _backgroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _midgroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _foregroundNoCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _backgroundCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _midgroundCollisionPalette = new List<MapObject>();
    private readonly List<MapObject> _foregroundCollisionPalette = new List<MapObject>();
    private readonly string _rootPath = "Assets/Editor Default Resources";
    private readonly string _backgroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Background";
    private readonly string _midgroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Midground";
    private readonly string _foregroundNoCollisionPath = "Assets/Editor Default Resources/NoCollision/Foreground";
    private readonly string _backgroundCollisionPath = "Assets/Editor Default Resources/Collision/Background";
    private readonly string _midgroundCollisionPath = "Assets/Editor Default Resources/Collision/Midground";
    private readonly string _foregroundCollisionPath = "Assets/Editor Default Resources/Collision/Foreground";
    private readonly int _maxDrawcountWait = 5;
    private List<MapObject> _currentPalette;
	private GameObject _tilemapStyxRoot;
    private MapObject _currentRoot;
    private MapObject _backgroundNoCollisionRoot;
    private MapObject _midgroundNoCollisionRoot;
    private MapObject _foregroundNoCollisionRoot;
    private MapObject _backgroundCollisionRoot;
    private MapObject _midgroundCollisionRoot;
    private MapObject _foregroundCollisionRoot;
    private Vector2 _cellSize = new Vector2(1.0f, 1.0f);
    private Vector2 tileGridScrollPosition;
    private Color _cellColor;
	private SORTING_LAYERS _sortingLayer;
	private COLLISION_LAYERS _collisionLayer;
    private int _paletteColumnSize = 3;
    private int _tileMinWidth = 350;
    private int _tileMinHeight = 100;
    private int _tileMaxWidth = 1500;
    private int _tileMaxHeight = 800;
    private int _paletteIndex;
    private int _brushSize = 1;
    private static int _drawCountWait;
    private bool _paletteToggle;
	private bool _ruleTileToggle;
	private bool _shortcutToggle;
    private bool _paintMode;
    private bool _ruleTileMode;
    private bool _holdBrush;
    private bool _moveHalf;
    private bool _isHoldingMouse;
    private bool _isPaletteTogglePressed;
    private bool _isRuleTileTogglePressed;
    private bool _isShortcutTogglePressed;


    [MenuItem("Window/Tilemap Styx")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TilemapStyx));
    }

    void Awake()
    {
		_paletteToggle = true;
		_currentPalette = _backgroundNoCollisionPalette;
    }

    void Update()
    {
        Repaint();
    }

    #region OnGui
    void OnGUI()
    {
        DisplayModeToggleSection();
		DisplaySelectedToggleMode();
    }

    private void DisplayModeToggleSection()
    {
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal("box");
        _paletteToggle = GUILayout.Toggle(_paletteToggle, "Palettes", "Button", GUILayout.Height(40f));
        _ruleTileToggle = GUILayout.Toggle(_ruleTileToggle, "Rule Tiles", "Button", GUILayout.Height(40f));
        _shortcutToggle = GUILayout.Toggle(_shortcutToggle, "Settings", "Button", GUILayout.Height(40f));
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

	private void DisplaySelectedToggleMode()
	{
        DisallowUntoggling();
        if (_paletteToggle && !_isPaletteTogglePressed)
        {
            _isPaletteTogglePressed = true;
            _isRuleTileTogglePressed = false;
            _isShortcutTogglePressed = false;
            _ruleTileToggle = false;
            _shortcutToggle = false;
        }
        if (_ruleTileToggle && !_isRuleTileTogglePressed)
        {
            _isRuleTileTogglePressed = true;
            _isPaletteTogglePressed = false;
            _isShortcutTogglePressed = false;
            _paletteToggle = false;
            _shortcutToggle = false;
        }
        if (_shortcutToggle && !_isShortcutTogglePressed)
		{
            _isShortcutTogglePressed = true;
            _isPaletteTogglePressed = false;
            _isRuleTileTogglePressed = false;
            _paletteToggle = false;
            _ruleTileToggle = false;
        }

        if (_paletteToggle)
        {
            DisplayPaintSection();
            DisplayTileGrid();
        }
        if (_ruleTileToggle)
        {
            DisplayRuleTile();
        }
        if (_shortcutToggle)
        {
            DisplayShortcut();
        }
    }

    private void DisallowUntoggling()
    {
        if (_isPaletteTogglePressed)
        {
            _paletteToggle = true;
        }
        if (_isRuleTileTogglePressed)
        {
            _ruleTileToggle = true;
        }
        if (_isShortcutTogglePressed)
        {
            _shortcutToggle = true;
        }
    }

	#region Palette
	private void DisplayPaintSection()
    {
        EditorGUILayout.Space();
        if (_paintMode)
            _paintMode = GUILayout.Toggle(_paintMode, "Stop painting", "Button", GUILayout.Height(60f));
        else
            _paintMode = GUILayout.Toggle(_paintMode, "Start painting", "Button", GUILayout.Height(60f));

        _brushSize = EditorGUILayout.IntSlider("Brush Size:", _brushSize, 1, 10);
        _ruleTileMode = EditorGUILayout.Toggle("Rule tile:", _ruleTileMode);
        _holdBrush = EditorGUILayout.Toggle("Hold brush:", _holdBrush);
        _moveHalf = EditorGUILayout.Toggle("Move half:", _moveHalf);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void DisplayTileGrid()
    {
        EditorGUILayout.Space();
        GUILayout.FlexibleSpace();
        _collisionLayer = (COLLISION_LAYERS)EditorGUILayout.EnumPopup("", _collisionLayer);
        GUILayout.FlexibleSpace();
        _sortingLayer = (SORTING_LAYERS)EditorGUILayout.EnumPopup("", _sortingLayer);

        GUIStyle centerLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
		if (_paletteIndex >= 0 && _paletteIndex <= _currentPalette.Count -1) 
		{
			EditorGUILayout.LabelField(_currentPalette[_paletteIndex].name, centerLabelStyle, GUILayout.ExpandWidth(true));
		}

        if (_currentPalette.Count >= 0)
        {
            GUILayout.BeginVertical();
            tileGridScrollPosition = GUILayout.BeginScrollView(tileGridScrollPosition, false, true, GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(500), GUILayout.ExpandHeight(true));
            List<GUIContent> paletteIcons = new List<GUIContent>();
            if (_collisionLayer == COLLISION_LAYERS.NoCollision)
            {
                switch (_sortingLayer)
                {
                    case SORTING_LAYERS.Background:
                        _currentPalette = _backgroundNoCollisionPalette;
                        break;
                    case SORTING_LAYERS.Midground:
                        _currentPalette = _midgroundNoCollisionPalette;
                        break;
                    case SORTING_LAYERS.Foreground:
                        _currentPalette = _foregroundNoCollisionPalette;
                        break;
                }
            }
            else
            {
                switch (_sortingLayer)
                {
                    case SORTING_LAYERS.Background:
                        _currentPalette = _backgroundCollisionPalette;
                        break;
                    case SORTING_LAYERS.Midground:
                        _currentPalette = _midgroundCollisionPalette;
                        break;
                    case SORTING_LAYERS.Foreground:
                        _currentPalette = _foregroundCollisionPalette;
                        break;
                }
            }

            foreach (MapObject mapObject in _currentPalette)
            {
                {
                    Texture2D texture = AssetPreview.GetAssetPreview(mapObject.gameObject);
                    GUIContent tileGuiContent = new GUIContent() { image = texture, tooltip = mapObject.name };
                    paletteIcons.Add(tileGuiContent);
                }
            }
            _paletteIndex = GUILayout.SelectionGrid(_paletteIndex, paletteIcons.ToArray(), _paletteColumnSize, GUILayout.MinWidth(_tileMinWidth), GUILayout.MaxWidth(_tileMaxWidth), GUILayout.MinHeight(_tileMinHeight), GUILayout.MaxHeight(_tileMaxHeight));
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        else
        {
            GUILayout.BeginVertical("box", GUILayout.MinWidth(350), GUILayout.MaxWidth(1500), GUILayout.ExpandWidth(true), GUILayout.MinHeight(100), GUILayout.MaxHeight(800));
            EditorGUILayout.LabelField("The current palette is empty", centerLabelStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndVertical();
        }
    }
	#endregion

	#region Rule Tile
    private void DisplayRuleTile()
    {
        EditorGUILayout.LabelField("In works.", GUILayout.ExpandWidth(true));
    }
    #endregion
    #region Settings
    private void DisplayShortcut()
    {
        GUIStyle centerLabelStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField("Palette Settings", centerLabelStyle, GUILayout.ExpandWidth(true));
        _paletteColumnSize = EditorGUILayout.IntSlider("Palette column size:", _paletteColumnSize, 1, 10);
        _tileMinWidth = EditorGUILayout.IntSlider("Minimum tile width:", _tileMinWidth, 100, 600);
        _tileMaxWidth = EditorGUILayout.IntSlider("Maximum tile width:", _tileMaxWidth, 600, 1500);
        _tileMinHeight = EditorGUILayout.IntSlider("Minimum tile height:", _tileMinHeight, 100, 600);
        _tileMaxHeight = EditorGUILayout.IntSlider("Maximum tile height:", _tileMaxHeight, 600, 1500);
        EditorGUILayout.Space();
        if (GUILayout.Button("Reset to default", GUILayout.Height(30f)))
        {
            _paletteColumnSize = 3;
            _tileMinWidth = 350;
            _tileMaxWidth = 1500;
            _tileMinHeight = 150;
            _tileMaxHeight = 800;
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        EditorGUILayout.LabelField("Shortcuts", centerLabelStyle, GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("CTRL: Delete", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("A: Go one tile back", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("D: Go one tile ahead", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Q: Decrease brush size by one", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("E: Increase brush size by one", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("Z: Switch between hold brush mode", GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("X: Switch between half move mode", GUILayout.ExpandWidth(true));
    }
    #endregion

    #endregion

    #region Focus And Destroy
    void OnFocus()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        SceneView.duringSceneGui += OnSceneGUI;
        RefreshPalette();
    }

    void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void RefreshPalette()
    {
        _tilemapStyxRoot = null;
        _backgroundNoCollisionPalette.Clear();
        _midgroundNoCollisionPalette.Clear();
        _foregroundNoCollisionPalette.Clear();
        _backgroundCollisionPalette.Clear();
        _midgroundCollisionPalette.Clear();
        _foregroundCollisionPalette.Clear();
        Directory.CreateDirectory(_rootPath);
        Directory.CreateDirectory(_backgroundNoCollisionPath);
        Directory.CreateDirectory(_midgroundNoCollisionPath);
        Directory.CreateDirectory(_foregroundNoCollisionPath);
        Directory.CreateDirectory(_backgroundCollisionPath);
        Directory.CreateDirectory(_midgroundCollisionPath);
        Directory.CreateDirectory(_foregroundCollisionPath);

        string[] root = Directory.GetFiles(_rootPath, "*.prefab");
        foreach (string prefabFile in root)
        {
            _tilemapStyxRoot = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
        }

        string[] backgroundNoCollisionFiles = Directory.GetFiles(_backgroundNoCollisionPath, "*.prefab");
        foreach (string backgroundFile in backgroundNoCollisionFiles)
        {
            _backgroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] midgroundNoCollisionFiles = Directory.GetFiles(_midgroundNoCollisionPath, "*.prefab");
        foreach (string midgroundFile in midgroundNoCollisionFiles)
        {
            _midgroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] foregroundNoCollisionFiles = Directory.GetFiles(_foregroundNoCollisionPath, "*.prefab");
        foreach (string foregroundFile in foregroundNoCollisionFiles)
        {
            _foregroundNoCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(foregroundFile, typeof(MapObject)) as MapObject);
        }

        string[] backgroundCollisionFiles = Directory.GetFiles(_backgroundCollisionPath, "*.prefab");
        foreach (string backgroundFile in backgroundCollisionFiles)
        {
            _backgroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(backgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] midgroundCollisionFiles = Directory.GetFiles(_midgroundCollisionPath, "*.prefab");
        foreach (string midgroundFile in midgroundCollisionFiles)
        {
            _midgroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(midgroundFile, typeof(MapObject)) as MapObject);
        }
        string[] foregroundCollisionFiles = Directory.GetFiles(_foregroundCollisionPath, "*.prefab");
        foreach (string foregroundFile in foregroundCollisionFiles)
        {
            _foregroundCollisionPalette.Add(AssetDatabase.LoadAssetAtPath(foregroundFile, typeof(MapObject)) as MapObject);
        }
    }
    #endregion


    #region OnSceneGUI
    private void OnSceneGUI(SceneView sceneView)
    {
        if (_paintMode)
        {
            Vector2 cellCenter = GetSelectedCell();
            ShowSpritePointer(cellCenter);
            HandleSceneViewInputs(cellCenter);
            sceneView.Repaint();
        }
    }

    private Vector2 GetSelectedCell()
    {
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 mousePosition = (guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z));

        _cellSize = new Vector2(_brushSize, _brushSize);
        Vector2Int cell;
        Vector2 cellCenter;
        if (!_moveHalf)
        {
            cell = new Vector2Int(Mathf.FloorToInt(mousePosition.x / _cellSize.x), Mathf.FloorToInt(mousePosition.y / _cellSize.y));
            cellCenter = cell * _cellSize;
        }
        else
        {
            cell = new Vector2Int(Mathf.FloorToInt(mousePosition.x / 0.5f), Mathf.FloorToInt(mousePosition.y / 0.5f));
            cellCenter = cell * new Vector2(_cellSize.x / 2, _cellSize.y / 2);
        }
        return cellCenter;
    }

    private void ShowSpritePointer(Vector2 cellCenter)
    {
        Vector3 topLeft = cellCenter + Vector2.up * 1;
        Vector3 topRight = cellCenter + Vector2.right * 1 + Vector2.up * 1;
        Vector3 bottomLeft = cellCenter;
        Vector3 bottomRight = cellCenter + Vector2.right * 1;

        Handles.color = _cellColor;
        Vector3[] lines = { topLeft, topRight, topRight, bottomRight, bottomRight, bottomLeft, bottomLeft, topLeft };
        Handles.DrawLines(lines);
    }

    private void HandleSceneViewInputs(Vector2 cellCenter)
    {
        ConsumeClickEvent();
        SwitchBetweenTiles();
        ToggleShortcuts();
        if (Event.current.control)
        {
            _cellColor = Color.red;
        }
        else
        {
            _cellColor = Color.white;
        }
        if (Event.current.type == EventType.MouseDown && Event.current.type == EventType.MouseDown)
        {
            _isHoldingMouse = true;
        }
        else if (Event.current.type == EventType.MouseUp)
        {
            _isHoldingMouse = false;
        }
        if (_paletteIndex < _currentPalette.Count && Event.current.button == 0 && Event.current.type == EventType.MouseDown)
        {
            MapObject mapObject = _currentPalette[_paletteIndex];
            if (Event.current.control)
            {
                RemoveMapObject(cellCenter, mapObject.SortingLayer);
            }
            else
            {
                AddMapObject(cellCenter, mapObject);
            }
        }
        else if (_paletteIndex < _currentPalette.Count && _holdBrush && _isHoldingMouse)
        {
            if (++_drawCountWait > _maxDrawcountWait)
            {
                _drawCountWait = 0;
                MapObject mapObject = _currentPalette[_paletteIndex];
                if (Event.current.control)
                {
                    RemoveMapObject(cellCenter, mapObject.SortingLayer);
                }
                else
                {
                    AddMapObject(cellCenter, mapObject);
                }
            }
        }
    }

    private void ConsumeClickEvent()
    {
        if (Event.current.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(0);
        }
    }

    private void SwitchBetweenTiles()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.D)
        {
			if (_paletteIndex % _paletteColumnSize - 2 == 0)
			{
				tileGridScrollPosition.y += 100;
			}

			if (_paletteIndex != _currentPalette.Count - 1)
            {
				_paletteIndex++;
            }
            else
            {
				tileGridScrollPosition.y = 0;
				_paletteIndex = 0;
            }
		}
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
        {
			if (_paletteIndex % _paletteColumnSize == 0)
			{
				tileGridScrollPosition.y -= 100;
			}

			if (_paletteIndex != 0)
            {
				_paletteIndex--;
			}
			else
            {
				tileGridScrollPosition.y = _currentPalette.Count * 100;
				_paletteIndex = _currentPalette.Count - 1;
            }
		}
    }

    private void ToggleShortcuts()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X)
        {
            _holdBrush = !_holdBrush;
        }
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.V)
        {
            _moveHalf = !_moveHalf;
        }
    }

    private void RemoveMapObject(Vector2 cellCenter, string sortingLayer)
    {
        MapObject previousObject = MapEditorModel.Get(cellCenter, sortingLayer);
        if (previousObject)
        {
            Undo.DestroyObjectImmediate(previousObject.gameObject);
        }
    }

    private void AddMapObject(Vector2 cellCenter, MapObject prefab)
    {
        Debug.Log(prefab);
        RemoveMapObject(cellCenter, prefab.SortingLayer);

        if (_tilemapStyxRoot == null)
        _tilemapStyxRoot = new GameObject("StyxTilemap_STM");

        if (_collisionLayer == COLLISION_LAYERS.NoCollision)
        {
            switch (_sortingLayer)
            {
                case SORTING_LAYERS.Background:
                    if (_backgroundNoCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/BackgroundNoCollision_emptyObject.prefab", typeof(MapObject));
                        _backgroundNoCollisionRoot = (MapObject)PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _backgroundNoCollisionRoot;
                    break;
                case SORTING_LAYERS.Midground:
                    if (_midgroundNoCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject) AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/MidgroundNoCollision_emptyObject.prefab", typeof(MapObject));
                        _midgroundNoCollisionRoot = (MapObject) PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _midgroundNoCollisionRoot;
                    break;
                case SORTING_LAYERS.Foreground:
                    if (_foregroundNoCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject) AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/ForegroundNoCollision_emptyObject.prefab", typeof(MapObject));
                        _foregroundNoCollisionRoot = (MapObject) PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _foregroundNoCollisionRoot;
                    break;
            }
        }
        else
        {
            switch (_sortingLayer)
            {
                case SORTING_LAYERS.Background:
                    if (_backgroundCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject) AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/BackgroundCollision_emptyObject.prefab", typeof(MapObject));
                        _backgroundCollisionRoot = (MapObject) PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _backgroundCollisionRoot;
                    break;
                case SORTING_LAYERS.Midground:
                    if (_midgroundCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject)AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/MidgroundCollision_emptyObject.prefab", typeof(MapObject));
                        _midgroundCollisionRoot = (MapObject)PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _midgroundCollisionRoot;
                    break;
                case SORTING_LAYERS.Foreground:
                    if (_foregroundCollisionRoot == null)
                    {
                        MapObject mapHierarchy = (MapObject) AssetDatabase.LoadAssetAtPath("Assets/Editor Default Resources/Hierarchy/ForegroundCollision_emptyObject.prefab", typeof(MapObject));
                        _foregroundCollisionRoot = (MapObject) PrefabUtility.InstantiatePrefab(mapHierarchy, _tilemapStyxRoot.transform);
                    }
                    _currentRoot = _foregroundCollisionRoot;
                    break;
            }
        }

        MapObject mapObject = PrefabUtility.InstantiatePrefab(prefab, _currentRoot.transform) as MapObject;
        mapObject.transform.position = cellCenter + _cellSize * 0.5f;

        Undo.RegisterCreatedObjectUndo(mapObject, mapObject.name);
    }
    #endregion
}