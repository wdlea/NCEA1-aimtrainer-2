using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public HealthBar Instance {get; private set;}

    private uint _health;
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

    void Awake(){
        if(Instance != null)
            Debug.LogWarning("More than 1 health bars in scene, there can only be 1!");
        Instance = this;
        Health = _startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Health <= 0)   
            Debug.Log("U ded lol");
    }
}
