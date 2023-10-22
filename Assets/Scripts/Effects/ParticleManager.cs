using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance {get; private set;}

    [SerializeField] ManagedParticles _killParticlesPrefab;
    ObjectPool<ManagedParticles> _killParticlePool;

    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 ParticleManager in the scene");
        Instance = this;
    }


    void Start()
    {
        _killParticlePool = new(
            () => Instantiate(_killParticlesPrefab),
            (ManagedParticles p) => {p.gameObject.SetActive(true);},
            (ManagedParticles p) => {p.gameObject.SetActive(false);},
            defaultCapacity: 30
        );
    }
    public void SpawnKillParticles(Vector3 position){
        ManagedParticles particles = _killParticlePool.Get();
        particles.transform.position = position;
        particles.Play();
        particles.OnFinishedPlaying.AddListener(() => {
            _killParticlePool.Release(particles);
        });
    }
}
