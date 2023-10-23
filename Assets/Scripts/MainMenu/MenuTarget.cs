using UnityEngine;

/// <summary>
/// A target in the background of the menu
/// </summary>
public class MenuTarget : MonoBehaviour
{
    /// <summary>
    /// The speed at which the target travels
    /// </summary>
    [SerializeField] VariedValue _speed = new VariedValue(20, 15);

    /// <summary>
    /// The point at where to destroy the target and spawn a new one
    /// </summary>
    [SerializeField] float _travelWidth = 30f;

    /// <summary>
    /// The current speed of the target
    /// </summary>
    float _currentSpeed;

    // Start is called before the first frame update
    void Start()
    {
        ResetPosition();//reset the position on start so they dont all spawn in the middle
    }

    // Update is called once per frame
    void Update()
    {
        //multiplying floats first becuase float*float then float*V3 is faster than V3*float then V3*float and Unity/C# does not optimise this out
        transform.position += Time.deltaTime * _currentSpeed * Vector3.right;//move right

        if(transform.position.x > _travelWidth)//if i am too far just respawn
            ResetPosition();
    }

    void ResetPosition(){
        _currentSpeed = _speed.GetValue();//reset my speed to a different one
        transform.position = new Vector3(//go to the other side of the screen
            -_travelWidth,
            Random.Range(-10f, 10f),//could be offscreen but this will give the illusion of different amounts of targets instead of the same couple of targets
            0
        );
    }
}
