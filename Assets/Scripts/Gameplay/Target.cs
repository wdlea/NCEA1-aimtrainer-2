using UnityEngine;

public class Target : MonoBehaviour
{
    private const float TARGET_OBJECTIVE_RADIUS = 2f;

    [SerializeField] private short _healthPenalty;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SimpleAnimationPlayer _animationPlayer;
    [SerializeField] private float _speed = 5f;

    public void OnSpawn(){
        _animationPlayer.Play();
    }

    void OnMouseDown(){
        EffectManager.Instance.SpawnKillParticles(transform.position);
        TargetManager.Instance.ReturnTarget(this);
        Statistics.OnHitTarget();
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
        TargetManager.Instance.ReturnTarget(this);
        try{
            checked{
                HealthBar.Instance.Health -= _healthPenalty;
            }
        }catch{
            GameOverManager.GameOver();//trigger GameOver because if the short overflows while losing health it must be below 0
        }
    }
}
