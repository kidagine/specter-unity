using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerLevelSystem _playerLevelSystem = default;

	public static GameManager Instance { get; private set; }


	void Awake()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		CheckInstance();
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

	public void GivePlayerExp(int exp)
	{
		_playerLevelSystem.EarnExp(exp);
	}
}
