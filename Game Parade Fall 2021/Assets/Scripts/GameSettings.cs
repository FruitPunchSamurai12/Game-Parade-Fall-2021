using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float sfx = 0.5f;
    public float music = 0.5f;

    static GameSettings instance;

    public static GameSettings Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }
}
    
