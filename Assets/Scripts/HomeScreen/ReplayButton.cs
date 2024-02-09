using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    
    public void ReplayGame()
    {
        if(GameManager.Instance._GameOver)
            GameManager.Instance.ResetGame();
        SceneLoader.Instance.UnloadScene(GameManager.Instance._LevelToLoad);
        SceneLoader.Instance.LoadScene(GameManager.Instance._LevelToLoad);
    }
}
