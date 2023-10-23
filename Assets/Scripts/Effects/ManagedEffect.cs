using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An effect triggered by <c>EffectManager</c>
/// </summary>
public class ManagedEffect : MonoBehaviour
{
    /// <summary>
    /// All the particle systems to play
    /// </summary>
    [SerializeField] private ParticleSystem[] _systems;

    /// <summary>
    /// All the audio clips to play
    /// </summary>
    [SerializeField] private ManagedAudioClip[] _clips;

    /// <summary>
    /// A callback that is triggered when everything stops playing
    /// </summary>
    public UnityEvent OnFinishedPlaying;//Used unityevent becuase I can drag and drop things in the editor
    float _totalDuration;//the current duration of the effect, using a private var becuase coroutines don't support arguments
    
    /// <summary>
    /// Plays all the clips and particles
    /// </summary>
    public void Play(){
        StopAllCoroutines();//stop any existing TriggerFinished coroutines in case this particle system is played multiple times without finishing

        OnFinishedPlaying ??= new();//make sure it is something otheriwse I will get a NullReferenceException

        _totalDuration = 0f;//reset the total durration becuase it persists between calls
        foreach (ParticleSystem system in _systems)
        {
            system.Play();//play all the systems
            if(system.main.duration > _totalDuration)//find the maximum duration of all the systems
                _totalDuration = system.main.duration;
        }

        if(_clips.Length > 0){//only do the LINQ if there is somethign to do it on to avoid an Exception
            float _maxAudioDuration = 
                (from ManagedAudioClip clip in _clips
                select clip.Delay + clip.Clip.length).Max();

            _totalDuration = Mathf.Max(_totalDuration, _maxAudioDuration);//set _totalDuration to the longest clip/particle system

            StartCoroutine(PlayClips());//play all the clips with their respective delays
        }
        
        StartCoroutine(TriggerFinished());//start waiting to send the finished callback
    }
    IEnumerator TriggerFinished(){
        yield return new WaitForSeconds(_totalDuration);//wait for the duration
        OnFinishedPlaying.Invoke();//send the callback
    }
    IEnumerator PlayClips(){
        if(_clips.Length <= 0) yield break;//dont execute if there is nothing to do

        float startTime = Time.realtimeSinceStartup;

        IEnumerable<ManagedAudioClip> clipOrder =
            from clip in _clips
            orderby clip.Delay
            select clip;//sort the clips by their delay
        
        foreach(ManagedAudioClip clip in clipOrder){//play clips in that order
            float progress = Time.realtimeSinceStartup - startTime;

            if(clip.Delay > progress)//make sure each clip is played delay seconds after this method is called
                yield return new WaitForSeconds(clip.Delay - progress);

            AudioSource.PlayClipAtPoint(clip.Clip, transform.position.normalized, 10);//play the clips spatially where the gameobject is
        }
    }
}

[Serializable] public struct ManagedAudioClip{
    public AudioClip Clip;
    public float Delay;
}