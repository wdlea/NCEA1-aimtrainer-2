using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance {get; private set;}

    [SerializeField][Range(0, 100)]private uint _health;
    public uint Health {
        get => _health;
        set{
            //clamp _health between 0 and 100
            if(value > 100)
                _health = 100;
            else if(value < 0)
                _health = 0;
            else _health = value;
        }
    }

    [SerializeField] private uint _startingHealth = 100;

    [SerializeField] private Image _healthBar;
    private RectTransform _healthBarTransform;
    [SerializeField] private Text _healthCount;

    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 health bars in scene, there can only be 1!");
        Instance = this;
        Health = _startingHealth;
    }
    void Start(){
        _healthBarTransform = _healthBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)   
            Debug.Log("U ded lol");

        _healthCount.text = Health.ToString() + "%";//not using StringBuilder becuase it is only 2 things

        _healthBarTransform.localScale = new Vector3(Health/100f, 1, 1);
    }
}
