using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{

    //Todo: Add buttons for options and unlocks

    public void LoadLevel()
    {
        SceneLoader.Instance.UnloadScene(GameManager.Instance._Homepage);
        SceneLoader.Instance.UnloadScene(GameManager.Instance._GameOverScene);
        SceneLoader.Instance.LoadScene(GameManager.Instance._LevelToLoad);
    }
}
