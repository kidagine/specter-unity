using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    [SerializeField] private GameObject _dialogueObject = default;
    [SerializeField] private GameObject _dialoguePrompt = default;
    [SerializeField] private GameObject _dialoguePane = default;
    [SerializeField] private Transform _promptPoint = default;


    public void SetPrompt(bool state, Vector2 promptPosition = default)
    {
        if (state)
        {
            _dialogueObject.SetActive(true);
            PositionPrompt(promptPosition);
        }
        else
        {
            _dialogueObject.SetActive(false);
        }
    }

    private void PositionPrompt(Vector2 promptPosition)
    {
        Vector2 promptPositionWorldSpace = _camera.WorldToScreenPoint(promptPosition);
        _promptPoint.position = promptPositionWorldSpace;
    }

    public void StartDialogue()
    {
        _dialoguePane.SetActive(true);
        _dialoguePrompt.SetActive(false);
    }
}
