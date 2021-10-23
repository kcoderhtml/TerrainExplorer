using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelClear : MonoBehaviour
{
    

    
    void Start()
    {
        if (PlayerPrefs.HasKey("LevelCleared"))
        {

        }
        else
        {
            PlayerPrefs.SetInt("LevelCleared", 0);

            if (PlayerPrefs.HasKey("LevelClearedCount"))
            {

            }
            else
            {
                PlayerPrefs.SetInt("LevelClearedCount", 0);
            }
        }
    }

    public void LevelCleared()
    {
        int previousBuildIndex = PlayerPrefs.GetInt("LevelCleared");
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        int previousLevelCount = PlayerPrefs.GetInt("LevelClearedCount");

        if (currentBuildIndex > previousBuildIndex)
        {
            PlayerPrefs.SetInt("LevelCleared", SceneManager.GetActiveScene().buildIndex);
            PlayerPrefs.SetInt("LevelClearedCount", previousLevelCount + 1);

        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
