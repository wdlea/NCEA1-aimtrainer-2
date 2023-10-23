using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The health bar HUD
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// The currently active instance of the health bar
    /// </summary>
    public static HealthBar Instance {get; private set;}

    /// <summary>
    /// The current amount of health that the player has
    /// </summary>
    [SerializeField][Range(0, 100)]private short _health;
    public short Health {
        get => _health;
        set{
            //clamp _health between 0 and 100
            if(value > 100)
                _health = 100;
            else if(value <= 0){
                _health = 0;//still display health so it shows after gameover
                GameOverManager.GameOver();
            }
            else _health = value;
        }
    }

    [SerializeField] private short _startingHealth = 100;

    //the GUI elements
    [SerializeField] private Image _healthBar;
    private RectTransform _healthBarTransform;
    [SerializeField] private Text _healthCount;

    void Awake(){
        //singleton pattern
        if(Instance != null)
            Debug.LogWarning("More than 1 health bars in scene, there can only be 1!");//warn user if there is more than 1 instance
        Instance = this;//regardless of ^ override it
        Health = _startingHealth;//initialise the health
    }
    void Start(){
        _healthBarTransform = _healthBar.GetComponent<RectTransform>();//cache to transform to avoid c++ call
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)   
            Debug.Log("U ded lol");

        _healthCount.text = Health.ToString() + "%";//not using StringBuilder becuase it is only 2 things

        _healthBarTransform.localScale = new Vector3(Health/100f, 1, 1);//set the scale of the inner bar
    }
}
