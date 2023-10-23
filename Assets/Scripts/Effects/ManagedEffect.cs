using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ManagedEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _systems;
    [SerializeField] private ManagedAudioClip[] _clips;
    public UnityEvent OnFinishedPlaying;
    float _totalDuration;
    public void Play(){
        StopAllCoroutines();//stop any existing TriggerFinished coroutines in case this particle system is played multiple times without finishing

        OnFinishedPlaying ??= new();//make sure it is something

        _totalDuration = 0f;
        foreach (ParticleSystem system in _systems)
        {
            system.Play();
            if(system.main.duration > _totalDuration)
                _totalDuration = system.main.duration;
        }

        if(_clips.Length > 0){
            float _maxAudioDuration = 
                (from ManagedAudioClip clip in _clips
                select clip.Delay + clip.Clip.length).Max();

            _totalDuration = Mathf.Max(_totalDuration, _maxAudioDuration);

            StartCoroutine(PlayClips());
        }
        
        StartCoroutine(TriggerFinished());
    }
    IEnumerator TriggerFinished(){
        yield return new WaitForSeconds(_totalDuration);
        OnFinishedPlaying.Invoke();
    }
    IEnumerator PlayClips(){
        if(_clips.Length <= 0) yield break;

        float startTime = Time.realtimeSinceStartup;

        IEnumerable<ManagedAudioClip> clipOrder =
            from clip in _clips
            orderby clip.Delay
            select clip;
        
        foreach(ManagedAudioClip clip in clipOrder){
            float progress = Time.realtimeSinceStartup - startTime;

            if(clip.Delay > progress)
                yield return new WaitForSeconds(clip.Delay - progress);
            AudioSource.PlayClipAtPoint(clip.Clip, transform.position.normalized, 10);
            Debug.Log("Playing clip");
        }
    }
}

[Serializable] public struct ManagedAudioClip{
    public AudioClip Clip;
    public float Delay;
}