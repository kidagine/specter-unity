using UnityEngine;

public class BossUI : MonoBehaviour
{
	[SerializeField] private BossHealthUI _bossHealthUI = default;
	[SerializeField] private BossTitleUI _bossTitleUI = default;
	[SerializeField] private BossVanishedUI _bossVanishedUI = default;

	public BossHealthUI BossHealthUI { get { return _bossHealthUI; } private set { } }
	public BossTitleUI BossTitleUI { get { return _bossTitleUI; } private set { } }
	public BossVanishedUI BossVanishedUI { get { return _bossVanishedUI; } private set { } }
}
