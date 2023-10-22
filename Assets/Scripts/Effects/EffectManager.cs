using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance {get; private set;}

    [SerializeField] ManagedEffect _killParticlesPrefab;
    ObjectPool<ManagedEffect> _killParticlePool;

    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 ParticleManager in the scene");
        Instance = this;
    }


    void Start()
    {
        _killParticlePool = new(
            () => {
                ManagedEffect effect = Instantiate(_killParticlesPrefab);
                effect.OnFinishedPlaying.AddListener(() => {
                    _killParticlePool.Release(effect);
                });
                return effect;
            },
            (ManagedEffect p) => {p.gameObject.SetActive(true);},
            (ManagedEffect p) => {p.gameObject.SetActive(false);},
            defaultCapacity: 30
        );
    }
    public void SpawnKillParticles(Vector3 position){
        ManagedEffect effect = _killParticlePool.Get();
        effect.transform.position = position;
        effect.Play();
        
    }
}
