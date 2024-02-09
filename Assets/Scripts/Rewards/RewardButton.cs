using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardButton : MonoBehaviour
{
    //Reward button with onclick set reward 
    public Reward _Reward;
    public TextMeshProUGUI _Text;
    public Image _Image;

    public void Onclick()
    {
        _Reward.Activate();
        //Move on with the scene

        if (GameManager.Instance._GameOver)
            GameManager.Instance.ResetGame();
        SceneLoader.Instance.UnloadScene(GameManager.Instance._LevelToLoad);
        SceneLoader.Instance.LoadScene(GameManager.Instance._LevelToLoad);
    }

}