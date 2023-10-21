using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private const float TARGET_SPAWN_RADIUS = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    Vector2 GenerateRandomPositionAroundUnitCircle(){
        float degrees = Random.Range(0, 360);

        return new Vector2(Mathf.Sin(degrees), Mathf.Cos(degrees));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
