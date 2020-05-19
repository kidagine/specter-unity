using UnityEngine;

public class BossUI : MonoBehaviour
{
	[SerializeField] private BossHealthUI _bossHealthUI = default;

	public BossHealthUI BossHealthUI { get { return _bossHealthUI; } private set { } }
}
