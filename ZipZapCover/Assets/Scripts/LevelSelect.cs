using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {

        if (Application.CanStreamedLevelBeLoaded(levelName))
        {
            SceneManager.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("Level \"" + levelName + "\" cannot be loaded. Check if it's added in Build Settings.");
        }
    }
}
