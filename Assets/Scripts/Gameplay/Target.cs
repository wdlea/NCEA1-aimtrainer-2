using UnityEngine;

/// <summary>
/// A fly in the game
/// </summary>
public class Target : MonoBehaviour
{
    /// <summary>
    /// The radius of the center
    /// </summary>
    private const float TARGET_OBJECTIVE_RADIUS = 2f;
    
    /// <summary>
    /// How much damage to take when it reaches the centre
    /// </summary>
    [SerializeField] private short _healthPenalty;

    /// <summary>
    /// The renderer I am using
    /// </summary>
    [SerializeField] private SpriteRenderer _renderer;

    /// <summary>
    /// The thing playing the fly animation
    /// </summary>
    [SerializeField] private SimpleAnimationPlayer _animationPlayer;

    /// <summary>
    /// How fast I move towards the centre
    /// </summary>
    [SerializeField] private float _speed = 5f;


    /// <summary>
    /// Called by the TargetManager when I spawn
    /// </summary>
    public void OnSpawn(){
        _animationPlayer.Play();//restart the animation if neccessary
    }
    /// <summary>
    /// Called when i am clicked
    /// </summary>
    void OnMouseDown(){
        EffectManager.Instance.SpawnKillParticles(transform.position);//spawn some particles
        TargetManager.Instance.ReturnTarget(this);//return me to the pool
        Statistics.OnHitTarget();//and update the statistics
    }

    void Update()
    {
        //going to (0, 0, 0) so this works
        Vector3 dir = -transform.position.normalized;

        _renderer.flipX = dir.x >= 0;//flip the renderer if neccessary

        float travelDistance = _speed * Time.deltaTime;

        float targetDistance = transform.position.magnitude;
        //calculate the distance to move

        if(travelDistance >= targetDistance - TARGET_OBJECTIVE_RADIUS)//if i am going to hit the target take health
            OnHitCentre();

        transform.position += dir * travelDistance;//move towards the target
    }

    /// <summary>
    /// Called when i hit the plate
    /// </summary>
    void OnHitCentre(){
        TargetManager.Instance.ReturnTarget(this);//return me to the pool
        try{
            checked{
                HealthBar.Instance.Health -= _healthPenalty;//subtract health from the player
            }
        }catch{
            GameOverManager.GameOver();//trigger GameOver because if the short overflows while losing health it must be below 0
        }
    }
}
