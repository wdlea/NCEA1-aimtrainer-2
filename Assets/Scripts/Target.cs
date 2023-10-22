using UnityEngine;

public class Target : MonoBehaviour
{
    private const float TARGET_SPAWN_RADIUS = 30f;
    private const float TARGET_OBJECTIVE_RADIUS = 2f;

    [SerializeField] private short _healthPenalty;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private float _speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = GenerateRandomPositionAroundCircle(TARGET_SPAWN_RADIUS);
    }

    public static Vector2 GenerateRandomPositionAroundCircle(float radius = 1){
        float degrees = Random.Range(0, 360);

        return new Vector2(Mathf.Sin(degrees), Mathf.Cos(degrees)) * radius;
    }
    
    void OnMouseDown(){
        Destroy(gameObject);
    }
    void Update()
    {
        Vector3 dir = -transform.position.normalized;

        _renderer.flipX = dir.x >= 0;

        float travelDistance = _speed * Time.deltaTime;

        float targetDistance = transform.position.magnitude;

        if(travelDistance >= targetDistance - TARGET_OBJECTIVE_RADIUS)
            OnHitCentre();

        transform.position += dir * travelDistance;
    }

    void OnHitCentre(){
        Destroy(gameObject);
        try{
            checked{
                HealthBar.Instance.Health -= _healthPenalty;
            }
        }catch{
            GameOverManager.GameOver();//trigger GameOver because if the short overflows while losing health it must be below 0
        }
    }
}
