using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipRewards : MonoBehaviour
{
    
    public void SkipReward()
    {
        ButtonManager.Instance._GameRewardsScreen.SetActive(false);
    }
}
