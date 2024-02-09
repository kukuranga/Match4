using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeButton : MonoBehaviour
{
    public void ReturnHome()
    {

        if (GameManager.Instance._GameOver)
            GameManager.Instance.ResetGame();
        SceneLoader.Instance.UnloadScene(GameManager.Instance._LevelToLoad);        
        SceneLoader.Instance.LoadScene(GameManager.Instance._Homepage);        
    }
}
