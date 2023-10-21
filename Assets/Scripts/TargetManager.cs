using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] Target _targetPrefab;

    [SerializeField] float _spawnIntervalMultiplier;
    [SerializeField] float _baseSpawnInterval;
    float _currentSpawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpawnInterval = _baseSpawnInterval;
        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets(){
        while(true){
            Instantiate(_targetPrefab);
            yield return new WaitForSeconds(_currentSpawnInterval);

            //last spawn interval becuase that is the "deltaTime" between this method getting called, so this makes the scaling consistent
            _currentSpawnInterval *= Mathf.Pow(_spawnIntervalMultiplier, _currentSpawnInterval);
        }
    }
}
