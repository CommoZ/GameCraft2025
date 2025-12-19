using UnityEngine;

public class PersistentMusic : MonoBehaviour
{
    private static PersistentMusic instance;

    void Awake()
    {
        // Check if an instance already exists
        if (instance == null)
        {
            // This is the first time the music is loading
            instance = this;
            
            // Tell Unity not to destroy this object when loading new scenes
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // If another instance already exists (e.g., returning to the menu),
            // destroy this new one so the original music keeps playing.
            Destroy(gameObject);
        }
    }
}