using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ParticleManager : MonoBehaviour
{
    ManagedParticles _killParticlesPrefab;
    ObjectPool<ManagedParticles> _killParticlePool;

    // Start is called before the first frame update
    void Start()
    {
        _killParticlePool = new(
            () => Instantiate(_killParticlesPrefab),
            (ParticleSystem p) => {p.gameObject.SetActive(true);},
            (ParticleSystem p) => {p.gameObject.SetActive(false);},
            defaultCapacity: 30
        );
    }
    public void SpawnKillParticles(Vector3 position){
        ManagedParticles particles = _killParticlePool.Get();
        particles.transform.position = position;
        particles.Play();
    }
}
