using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// It shakes the screen, as the name suggests.
/// </summary>
public class ScreenShaker : MonoBehaviour
{
    /// <summary>
    /// The currently active instance of the screen shaker.
    /// </summary>
    public static ScreenShaker Instance {get; private set;}

    /// <summary>
    /// A reference to the UI so it could shake that too if I enable it
    /// </summary>
    [SerializeField] RectTransform _UI;

    /// <summary>
    /// A reference to the target camera to shake.
    /// </summary>
    Camera _mainCamera;
    
    // Math stuff to determine the duration, intensity and number of shakes
    [SerializeField] float _shakeDurationBase = 1;
    [SerializeField] float _shakeDurationCoeffecient = 1;
    [SerializeField] float _shakeDistanceMultiplier = 0.1f;
    [SerializeField] float _shakeUIMultiplier = -1;
    [SerializeField] float _shakeDamping = 0.7f;
    [SerializeField] float _shakeClip = 0.1f;

    void Awake(){
        //singleton pattern
        if(Instance != null)
            Debug.LogWarning("More than 1 screen shaker in scene");//if there is more than 1 warn me
        Instance = this;//regardless of ^ set the instance
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;//Cache the main camera to avoid calling the underlying c++ code
        StartCoroutine(DoShake());//start the shaker
    }
    /// <summary>
    /// The shake intensity, used to determine the speed, duration and number of the shakes.
    /// </summary>
    public float CurrentShakeIntensity;

    /// <summary>
    /// The thing that does the shaking
    /// </summary>
    /// <returns>A coroutine to do the shaking.</returns>
    IEnumerator DoShake(){
        float startTime = Time.realtimeSinceStartup;//keep track of the time at the start of the shake
        while(true){
            yield return ShakeOnce();//shake once

            float currentTime = Time.realtimeSinceStartup;

            //dampen the shake
            CurrentShakeIntensity *= Mathf.Pow(_shakeDamping, currentTime - startTime);
            startTime = currentTime;

            if(CurrentShakeIntensity < _shakeClip) CurrentShakeIntensity = 0f;//if the shake is too low set it to 0
        }
    }

    IEnumerator ShakeOnce(){
        if(CurrentShakeIntensity <= 0){//dont do math if there is no need to
            yield return null;
            yield break;
        }

        float shakeDistance = _shakeDistanceMultiplier * CurrentShakeIntensity;//calculat the desired distance

        Vector3 startOffset = _mainCamera.transform.position;
        Vector3 finalOffset = new Vector3(
            Random.Range(-shakeDistance, shakeDistance),//set a random position to go to that matches the desired distance
            Random.Range(-shakeDistance, shakeDistance)//i could normalise and set the magnitude here but that is probably going to be slower becuase i am not using an expensive PRNG
        );
        
        float shakeDuration = _shakeDurationCoeffecient * Mathf.Pow(_shakeDurationBase, CurrentShakeIntensity);//calculate how long the shake should be


        //initialise some values
        float startTime = Time.realtimeSinceStartup;
        float currentTime = startTime;
        float progress = 0;
        float endTime = startTime + shakeDuration;
        
        while(currentTime < endTime){//for the duration of the shake
            Vector3 currentOffset = Vector3.Lerp(startOffset, finalOffset, progress);// lerp between start and end positions by the current(normalised) time

            _mainCamera.transform.position = currentOffset + (10 * Vector3.back);
            _UI.transform.position = currentOffset * _shakeUIMultiplier;///use these positions to determine the positions of the camera and the UI

            yield return null;//wait a frame
            currentTime = Time.realtimeSinceStartup;//recalculate the values on the next frame
            progress = (currentTime - startTime)/shakeDuration;
        }
    }
}
