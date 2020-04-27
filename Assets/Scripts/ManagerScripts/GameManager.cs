using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _player = default;

	public Player Player { get { return _player; } private set { } }
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
}
