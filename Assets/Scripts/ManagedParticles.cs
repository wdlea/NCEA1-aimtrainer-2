using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ManagedParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _systems;

    public UnityEvent OnFinishedPlaying;
    float _totalDuration;
    public void Play(){
        OnFinishedPlaying ??= new();//make sure it is something

        _totalDuration = 0f;
        foreach (ParticleSystem system in _systems)
        {
            system.Play();
            if(system.main.duration > _totalDuration)
                _totalDuration = system.main.duration;

            StopAllCoroutines();//stop any existing TriggerFinished coroutines in case this particle system is played multiple times without finishing
            StartCoroutine(TriggerFinished());
        }
    }
    IEnumerator TriggerFinished(){
        yield return new WaitForSeconds(_totalDuration);
        OnFinishedPlaying.Invoke();
    }
}
