using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
	public static CheckpointManager Instance { get; private set; }
	[SerializeField] private Transform _playerTransform = default;
	[SerializeField] private Checkpoint[] _checkpoints = default;
	[HideInInspector] public event Action CheckpointEvent;
	private Vector2 _lastStoredCheckpointPosition;
	private int _nextCheckpointIndex;
	private int _currentCheckpointIndex;


	void Awake()
	{
		CheckInstance();
		Respawn();
	}

	private void CheckInstance()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	private void Respawn()
	{
		_lastStoredCheckpointPosition = GlobalSettings._lastStoredCheckpointPosition;
		if (_lastStoredCheckpointPosition != Vector2.zero)
		_playerTransform.position = _lastStoredCheckpointPosition;
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void StoreCheckpoint(Checkpoint checkpoint)
	{
		_currentCheckpointIndex = checkpoint.CheckpointIndex;
		if (_currentCheckpointIndex == _checkpoints.Length - 1)
		{
			_nextCheckpointIndex = 0;
		}
		else
		{
			int nextCheckpointIndex = checkpoint.CheckpointIndex + 1;
			_nextCheckpointIndex = nextCheckpointIndex;
		}
		GlobalSettings._lastStoredCheckpointPosition = checkpoint.transform.position;
		CheckpointEvent();
	}

	public void RespawnToNextCheckpoint()
	{
		_playerTransform.position = _checkpoints[_nextCheckpointIndex].GetCheckpointPosition();
		if (_nextCheckpointIndex == _checkpoints.Length)
		{
			_currentCheckpointIndex = 0;
			_nextCheckpointIndex = 1;
		}
		else
		{
			_currentCheckpointIndex = _nextCheckpointIndex;
			_nextCheckpointIndex++;
			if (_nextCheckpointIndex == _checkpoints.Length)
			{
				_nextCheckpointIndex = 0;
			}
		}
	}

	public Checkpoint GetCurrentCheckpoint()
	{
		return _checkpoints[_currentCheckpointIndex];
	}

	public Checkpoint GetNextCheckpoint()
	{
		return _checkpoints[_nextCheckpointIndex];
	}
}