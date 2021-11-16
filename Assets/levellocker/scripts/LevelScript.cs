using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        disableAll();
        if (!PlayerPrefs.HasKey("LevelClearedCount"))
            PlayerPrefs.SetInt("LevelClearedCount", 0);

        int clearedLevel = PlayerPrefs.GetInt("LevelClearedCount");
        for(int i =0; i< clearedLevel+1; ++i)
        {
            transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableAll()
    {
        int levelButtonsCount = transform.childCount;
        for(int i=0; i < levelButtonsCount; ++i)
        {
            transform.GetChild(i).GetComponent<Button>().interactable = false;
        }
    }

    public void playLevel(int level = '1')
    {
        SceneManager.LoadScene(level);
    }
}
