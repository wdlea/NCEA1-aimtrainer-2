using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private const float TARGET_SPAWN_RADIUS = 30f;

    [SerializeField] private float _speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GenerateRandomPositionAroundCircle(30);
    }

    public Vector2 GenerateRandomPositionAroundCircle(float radius = 1){
        float degrees = Random.Range(0, 360);

        return new Vector2(Mathf.Sin(degrees), Mathf.Cos(degrees)) * radius;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = -transform.position.normalized;
        float travelDistance = _speed * Time.deltaTime;

        float targetDistance = transform.position.magnitude;

        if(travelDistance >= targetDistance)
            DealDamage();

        transform.position += dir * travelDistance;
    }

    void DealDamage(){
        Destroy(gameObject);
        Debug.Log("Reached center, ouch!");
    }
}
