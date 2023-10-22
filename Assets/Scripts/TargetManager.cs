using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance {get; private set;}

    private const float TARGET_SPAWN_RADIUS = 30f;

    [SerializeField] Target _flyPrefab;
    [SerializeField] float _swarmSpawnIntervalMultiplier;
    [SerializeField] float _baseSwarmSpawnInterval;
    [SerializeField] VariedValue _swarmMemberCount;
    [SerializeField] VariedValue _swarmMemberTimeOffset;
    [SerializeField] float _swarmDegreesVaraition = 30f;
   
    float _currentSpawnInterval;

    ObjectPool<Target> _targetPool;

    
    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 targetmanager in scene!");
        Instance = this;
    }

    void Start()
    {
        _targetPool = new(PoolCreateTarget, PoolOnGetTarget, PoolOnReturnTarget, defaultCapacity: 50);
        _currentSpawnInterval = _baseSwarmSpawnInterval;
        StartCoroutine(SpawnSwarms());
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Mouse1))
            Statistics.OnShot();
    }

    Target PoolCreateTarget(){
        Target t = Instantiate(_flyPrefab);
        t.OnSpawn();
        return t;
    }
    void PoolOnGetTarget(Target t){
        t.gameObject.SetActive(true);
        t.OnSpawn();
    }
    void PoolOnReturnTarget(Target t){
        t.gameObject.SetActive(false);
    }
    
    public void ReturnTarget(Target t){
        _targetPool.Release(t);
    }

    IEnumerator SpawnSwarms(){
        while(true){
            yield return SpawnSwarm();
            
            yield return new WaitForSeconds(_currentSpawnInterval);

            //last spawn interval becuase that is the "deltaTime" between this method getting called, so this makes the scaling consistent
            _currentSpawnInterval *= Mathf.Pow(_swarmSpawnIntervalMultiplier, _currentSpawnInterval);
        }
    }

    IEnumerator SpawnSwarm(){

        Debug.Log("Spawning swarm");
        float baseDegrees = Random.Range(0f, 360f);
        int swarmCount = (int)_swarmMemberCount.GetValue();

        VariedValue angles = new(baseDegrees, _swarmDegreesVaraition);

        for (int i = 0; i < swarmCount; i++)
        {
            float angle = (angles.GetValue() % 360 + 360) % 360;//true modulus
            Debug.Log("Spawning fly at angle: " + angle.ToString());

            Vector3 position = DegreesToCirclePoint(angle, TARGET_SPAWN_RADIUS);
            Debug.Log("Spawning fly at position: " + position.ToString());

            Target fly = _targetPool.Get();
            fly.transform.position = position;
            Statistics.OnSpawnTarget();

            yield return new WaitForSeconds(_swarmMemberTimeOffset.GetValue());
        }
    }

    public static Vector2 DegreesToCirclePoint(float degrees, float radius = 1){
        return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad)) * radius;
    }
}

[Serializable]
struct VariedValue{
    public float BaseValue;
    public float Variation;

    public VariedValue(float baseValue, float variation){
        BaseValue = baseValue;
        Variation = variation;
    }

    public float GetValue(){
        return BaseValue + Random.Range(-Variation, Variation);
    }
}