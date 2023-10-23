using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Manages showing on-screen effects to the user
/// </summary>
public class EffectManager : MonoBehaviour
{
    /// <summary>
    /// The currently active instance of the object.
    /// </summary>
    public static EffectManager Instance {get; private set;}

    /// <summary>
    /// The prefab for the particles spawned when a fly is killed
    /// </summary>
    [SerializeField] ManagedEffect _killParticlesPrefab;

    /// <summary>
    /// An object-pool for performantly spawning the particles.
    /// </summary>
    ObjectPool<ManagedEffect> _killParticlePool;

    /// <summary>
    /// The prefab for the particles spawned when the user clicks
    /// </summary>
    [SerializeField] ManagedEffect _swatParticlesPrefab;

    /// <summary>
    /// An object-pool for performantly spawning the particles.
    /// </summary>
    ObjectPool<ManagedEffect> _swatParticlePool;

    void Awake(){
        //this uses singleton design pattern for easy-access from other scripts while allowing me to change values in the editor
        if(Instance != null)
            Debug.LogWarning("More than 1 ParticleManager in the scene");//warn the user if there is more than 1 active instance which could result in unintentional behavour
        Instance = this;//replace the old instance regardless of the above if statement
    }


    void Start()
    {
        _killParticlePool = new(
            () => {
                ManagedEffect effect = Instantiate(_killParticlesPrefab);
                effect.OnFinishedPlaying.AddListener(() => {
                    _killParticlePool.Release(effect);//add the object back to the pool when the effect stops
                });
                return effect;
            },
            (ManagedEffect p) => {p.gameObject.SetActive(true);},
            (ManagedEffect p) => {p.gameObject.SetActive(false);},
            defaultCapacity: 10
        );//create the kill particle pool using lambdas for all the required methods

        _swatParticlePool = new(
            () => {
                ManagedEffect effect = Instantiate(_swatParticlesPrefab);
                effect.OnFinishedPlaying.AddListener(() => {
                    _swatParticlePool.Release(effect);
                });
                return effect;
            },
            (ManagedEffect p) => {p.gameObject.SetActive(true);},
            (ManagedEffect p) => {p.gameObject.SetActive(false);},
            defaultCapacity: 20
        );//do the same for the swat particles, but with a higher default capacity becuase they wll be spawned more frequently
    }

    public void SpawnKillParticles(Vector3 position){
        ManagedEffect effect = _killParticlePool.Get();//get an object from the pool
        effect.transform.position = position;//set its position to the desired position
        effect.Play();//let it play
    }

    public void SpawnSwatParticles(Vector3 position){
        ManagedEffect effect = _swatParticlePool.Get();//see above
        effect.transform.position = position;
        effect.Play();
    }
}
