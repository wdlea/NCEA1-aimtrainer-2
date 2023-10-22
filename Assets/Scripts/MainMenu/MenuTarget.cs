using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTarget : MonoBehaviour
{
    [SerializeField] float _baseSpeed = 20f;
    [SerializeField] float _speedVariation = 5f;

    [SerializeField] float _travelWidth = 30f;

    float _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        //multiplying floats first becuase float*float then float*V3 is faster than V3*float then V3*float and Unity/C# does not optimise this out
        transform.position += Time.deltaTime * _currentSpeed * Vector3.right;

        if(transform.position.x > _travelWidth)
            ResetPosition();
    }

    void ResetPosition(){
        _currentSpeed = _baseSpeed + Random.Range(-_speedVariation, _speedVariation);
        transform.position = new Vector3(
            -_travelWidth,
            Random.Range(-10f, 10f),//could be offscreen but this will give the illusion of different amounts of targets instead of the same couple of targets
            0
        );
    }
}
