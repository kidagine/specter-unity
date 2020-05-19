using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private StatsUI _statsUI = default;
    [SerializeField] private InventoryUI _inventoryUI = default;
    [SerializeField] private DeathUI _deathUI = default;
    [SerializeField] private DialogueUI _dialogueUI = default;
	[SerializeField] private InteractUI _interactUI = default;
	[SerializeField] private FadeDarkUI _fadeDarkUI = default;


	public StatsUI StatsUI { get { return _statsUI; } private set {} }
    public InventoryUI InventoryUI { get { return _inventoryUI; } private set {} }
    public DeathUI DeathUI { get { return _deathUI; } private set {} }
    public DialogueUI DialogueUI { get { return _dialogueUI; } private set { } }
	public InteractUI InteractUI { get { return _interactUI; } private set { } }
	public FadeDarkUI FadeDarkUI { get { return _fadeDarkUI; } private set { } }
}