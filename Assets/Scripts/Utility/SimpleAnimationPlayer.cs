using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimationPlayer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _frameDuration = 0.1f;

    public bool Playing = false;
    [SerializeField] private Sprite[] frames;
    private int currentFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        Play();
    }

    public void Play(){
        StopAllCoroutines();
        if(frames.Length <= 0)
            Debug.LogWarning("No animation frames available to play, not starting coroutine");
        else StartCoroutine(PlayAnimation());
    }

    IEnumerator PlayAnimation(){
        while(true){
            if(Playing){
                yield return new WaitForSeconds(_frameDuration);
                currentFrame = (currentFrame + 1) % frames.Length;
                _renderer.sprite = frames[currentFrame];
            }else yield return new WaitUntil(() => Playing);
        }
    }
}
