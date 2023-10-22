using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TargetManager : MonoBehaviour
{
    private const float TARGET_SPAWN_RADIUS = 30f;

    [SerializeField] Target _targetPrefab;

    [SerializeField] float _spawnIntervalMultiplier;
    [SerializeField] float _baseSpawnInterval;
    float _currentSpawnInterval;

    ObjectPool<Target> _targetPool;

    // Start is called before the first frame update
    void Start()
    {
        _targetPool = new(PoolCreateTarget, PoolOnGetTarget, PoolOnReturnTarget, defaultCapacity: 50);
        _currentSpawnInterval = _baseSpawnInterval;
        StartCoroutine(SpawnTargets());
    }

    Target PoolCreateTarget(){
        return Instantiate(_targetPrefab);
    }
    void PoolOnGetTarget(Target t){
        t.gameObject.SetActive(true);
    }
    void PoolOnReturnTarget(Target t){
        t.gameObject.SetActive(false);
    }
    IEnumerator SpawnTargets(){
        while(true){
            Target target = _targetPool.Get();

            target.transform.position = GenerateRandomPositionAroundCircle(TARGET_SPAWN_RADIUS);
            
            yield return new WaitForSeconds(_currentSpawnInterval);

            //last spawn interval becuase that is the "deltaTime" between this method getting called, so this makes the scaling consistent
            _currentSpawnInterval *= Mathf.Pow(_spawnIntervalMultiplier, _currentSpawnInterval);
        }
    }

    public static Vector2 GenerateRandomPositionAroundCircle(float radius = 1){
        float degrees = Random.Range(0, 360);

        return new Vector2(Mathf.Sin(degrees), Mathf.Cos(degrees)) * radius;
    }
}
