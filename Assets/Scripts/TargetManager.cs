using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] Target _targetPrefab;
    [SerializeField] float _spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets(){
        while(true){
            Instantiate(_targetPrefab);
            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}
