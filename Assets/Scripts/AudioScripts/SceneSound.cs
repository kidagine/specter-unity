using UnityEngine;

[RequireComponent(typeof(EntityAudio))]
public class SceneSound : MonoBehaviour
{
    [SerializeField] private EntityAudio _areaAudio = default;
    private bool _isLooping;


    void Start()
    {
        _areaAudio.Play("TheRockBottomIntroMusic");
    }

    void Update()
    {
        if (!_areaAudio.IsPlaying("TheRockBottomIntroMusic") && !_isLooping)
        {
            _isLooping = true;
            _areaAudio.Play("TheRockBottomLoopMusic");
        }
    }
}
