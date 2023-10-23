using UnityEngine;

/// <summary>
/// Calls DontDestroyOnLoad for the gameobject on start
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
