using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _npcBoxCollider = default;
    [SerializeField] private Dialogue _dialogue = default;
    private readonly int _yPromptOffset = 1;

    public Vector2 GetDialoguePromptPosition()
    {
        Vector2 dialoguePromptPosition = new Vector2(transform.position.x, (_npcBoxCollider.bounds.extents.y * 2) + _yPromptOffset);
        return dialoguePromptPosition;
    }

    public Dialogue GetDialogue()
    {
        return _dialogue;
    }
}
