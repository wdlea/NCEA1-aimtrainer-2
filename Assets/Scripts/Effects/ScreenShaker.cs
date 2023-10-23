using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour
{
    public static ScreenShaker Instance {get; private set;}

    [SerializeField] RectTransform _UI;
    Camera _mainCamera = Camera.main;

    [SerializeField] float _shakeDurationBase = 1;
    [SerializeField] float _shakeDurationCoeffecient = 1;
    [SerializeField] float _shakeDistanceMultiplier = 0.1f;
    [SerializeField] float _shakeUIMultiplier = -1;
    [SerializeField] float _shakeDamping = 0.7f;
    [SerializeField] float _shakeClip = 0.1f;

    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 screen shaker in scene");
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        StartCoroutine(DoShake());
    }

    public float CurrentShakeIntensity;
    IEnumerator DoShake(){
        float startTime = Time.realtimeSinceStartup;
        while(true){
            yield return ShakeOnce();

            float currentTime = Time.realtimeSinceStartup;
            CurrentShakeIntensity *= Mathf.Pow(_shakeDamping, currentTime - startTime);
            startTime = currentTime;

            if(CurrentShakeIntensity < _shakeClip) CurrentShakeIntensity = 0f;
        }
    }

    IEnumerator ShakeOnce(){
        if(CurrentShakeIntensity <= 0){//dont do math if there is no need to
            yield return null;
            yield break;
        }

        float shakeDistance = _shakeDistanceMultiplier * CurrentShakeIntensity;

        Vector3 startOffset = _mainCamera.transform.position;
        Vector3 finalOffset = new Vector3(
            Random.Range(-shakeDistance, shakeDistance),
            Random.Range(-shakeDistance, shakeDistance)
        );

        float shakeDuration = _shakeDurationCoeffecient * Mathf.Pow(_shakeDurationBase, CurrentShakeIntensity);

        float startTime = Time.realtimeSinceStartup;
        float currentTime = startTime;
        float progress = 0;
        float endTime = startTime + shakeDuration;
        
        while(currentTime < endTime){
            Vector3 currentOffset = Vector3.Lerp(startOffset, finalOffset, progress);

            _mainCamera.transform.position = currentOffset + (10 * Vector3.back);
            _UI.transform.position = currentOffset * _shakeUIMultiplier;

            yield return null;
            currentTime = Time.realtimeSinceStartup;
            progress = (currentTime - startTime)/shakeDuration;
        }
    }
}
