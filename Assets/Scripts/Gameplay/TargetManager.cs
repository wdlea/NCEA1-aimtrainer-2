using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

/// <summary>
/// Handles pooling and spawnign the targets
/// </summary>
public class TargetManager : MonoBehaviour
{
    /// <summary>
    /// The currently active instance
    /// </summary>
    public static TargetManager Instance {get; private set;}

    /// <summary>
    /// The radius at which the targets spawn at
    /// </summary>
    private const float TARGET_SPAWN_RADIUS = 30f;


    /// <summary>
    /// The prefab for the target
    /// </summary>
    [SerializeField] Target _flyPrefab;

    //Math stuff for spawning characteristics
    [SerializeField] float _swarmSpawnIntervalMultiplier;
    [SerializeField] float _baseSwarmSpawnInterval;
    [SerializeField] VariedValue _swarmMemberCount;
    [SerializeField] VariedValue _swarmMemberTimeOffset;
    [SerializeField] float _swarmDegreesVaraition = 30f;

    Camera _mainCamera;

    /// <summary>
    /// All the audio clips that play when the user clicks
    /// </summary>
    [SerializeField] AudioClip[] _swatClips;
   
    /// <summary>
    /// The current interval between spawns
    /// </summary>
    float _currentSpawnInterval;


    /// <summary>
    /// The pool of targets
    /// </summary>
    ObjectPool<Target> _targetPool;

    
    void Awake(){
        //singleton pattern
        if(Instance != null)
            Debug.LogWarning("More than 1 targetmanager in scene!");//warn user if there is more than 1
        Instance = this;//regardlessly of ^, override the current one
    }

    void Start()
    {
        _mainCamera = Camera.main;//cache the main camera to avoid c++ call
    
        _targetPool = new(PoolCreateTarget, PoolOnGetTarget, PoolOnReturnTarget, defaultCapacity: 50);//instantiate the pool
        _currentSpawnInterval = _baseSwarmSpawnInterval;//set the initial spawn interval
        StartCoroutine(SpawnSwarms());//start the spawning coroutine
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){//if the user clicked
            Statistics.OnShot();//update stats


            //find out where they clicked
            Ray cursorRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 cursorGamePosition = new Vector3(cursorRay.origin.x, cursorRay.origin.y);//orthogrpahic projection so I can just set Z to the Z I want

            //do some effects at that point to give them some feedback
            ScreenShaker.Instance.CurrentShakeIntensity = 1f;
            AudioSource.PlayClipAtPoint(
                _swatClips[Random.Range(0, _swatClips.Length)],
                cursorGamePosition.normalized,
                100
            );
            EffectManager.Instance.SpawnSwatParticles(cursorGamePosition);
        }  
    }

    //pool-specific methods
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


    /// <summary>
    /// Spawns the targets
    /// </summary>
    /// <returns>The coroutine</returns>
    IEnumerator SpawnSwarms(){
        while(true){
            yield return SpawnSwarm();//spawn a group of targets
            
            yield return new WaitForSeconds(_currentSpawnInterval);//wait a while

            //make the interval between spawns smaller
            //last spawn interval becuase that is the "deltaTime" between this method getting called, so this makes the scaling consistent
            _currentSpawnInterval *= Mathf.Pow(_swarmSpawnIntervalMultiplier, _currentSpawnInterval);
        }
    }


    /// <summary>
    /// Spawns a single swarm
    /// </summary>
    /// <returns>The coroutine</returns>
    IEnumerator SpawnSwarm(){
        //choose some random values
        float baseDegrees = Random.Range(0f, 360f);
        int swarmCount = (int)_swarmMemberCount.GetValue();

        //instantiate the angle "generator"
        VariedValue angles = new(baseDegrees, _swarmDegreesVaraition);

        for (int i = 0; i < swarmCount; i++)//spawn the random amount of flys
        {
            //choose a random angle in the same general direction
            float angle = (angles.GetValue() % 360 + 360) % 360;//true modulus

            //calculate the point on TARGET_SPAWN_RADIUS at that angle
            Vector3 position = DegreesToCirclePoint(angle, TARGET_SPAWN_RADIUS);

            //spawn a fly there
            Target fly = _targetPool.Get();
            fly.transform.position = position;
            Statistics.OnSpawnTarget();

            //wait a random interval to spread the flies out
            yield return new WaitForSeconds(_swarmMemberTimeOffset.GetValue());
        }
    }

    /// <summary>
    /// Converts a nangle to a point on a circle
    /// </summary>
    /// <param name="degrees">The angle in degrees.</param>
    /// <param name="radius">The radius of the circle</param>
    /// <returns>The point.</returns>
    public static Vector2 DegreesToCirclePoint(float degrees, float radius = 1){
        return new Vector2(Mathf.Cos(degrees * Mathf.Deg2Rad), Mathf.Sin(degrees * Mathf.Deg2Rad)) * radius;
    }
}

/// <summary>
/// Represents a value with some random variation
/// </summary>
[Serializable]
struct VariedValue{
    /// <summary>
    /// The value
    /// </summary>
    public float BaseValue;
    /// <summary>
    /// The amount of variation
    /// </summary>
    public float Variation;

    public VariedValue(float baseValue, float variation){
        BaseValue = baseValue;
        Variation = variation;
    }

    /// <summary>
    /// Gets a value that is varied from BaseValue
    /// </summary>
    /// <returns></returns>
    public float GetValue(){
        return BaseValue + Random.Range(-Variation, Variation);
    }
}