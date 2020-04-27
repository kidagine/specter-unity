#if (UNITY_EDITOR) 
using TMPro;
using UnityEngine;

public class GameDebugger : MonoBehaviour
{
    [SerializeField] private Canvas _canvas = default;
    [SerializeField] private TextMeshProUGUI _slowdownText = default;
    [SerializeField] private TextMeshProUGUI _currentAreaText = default;
    [SerializeField] private TextMeshProUGUI _nextAreaText = default;
    private bool _isDebuggerActive;


    void Update()
    {
        SetDebugger();
        CheckDebuggerControls();
    }

    private void SetDebugger()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _isDebuggerActive = !_isDebuggerActive;
            if (_isDebuggerActive)
            {
                _canvas.enabled = true;
            }
            else
            {
                _canvas.enabled = false;
            }
        }
    }

    private void CheckDebuggerControls()
    {
        if (_isDebuggerActive)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Slowdown();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                NextCheckpoint();
            }
        }
    }

    private void Slowdown()
    {
        if (Time.timeScale <= 0.1f)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale -= 0.1f;
        }
        _slowdownText.text = Time.timeScale.ToString("F1");
    }

    private void NextCheckpoint()
    {
        CheckpointManager.Instance.RespawnToNextCheckpoint();
        UpdateCheckpoint();
    }

    private void UpdateCheckpoint()
    {
        _currentAreaText.text = CheckpointManager.Instance.GetCurrentCheckpoint().AreaName;
        _nextAreaText.text = CheckpointManager.Instance.GetNextCheckpoint().AreaName;
    }

    private void OnEnable()
    {
        CheckpointManager.Instance.CheckpointEvent += UpdateCheckpoint;
    }

    private void OnDisable()
    {
        CheckpointManager.Instance.CheckpointEvent -= UpdateCheckpoint;
    }
}
#endif