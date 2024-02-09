using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseLuck", menuName = "Rewards/IncreaseLuck", order = 51)]
public class LuckIncrease : Reward
{
    [SerializeField] int _IncreaseLuck= 0;

    public override void Activate()
    {
        RewardsManager.Instance.IncreaseLuck(_IncreaseLuck);
        RewardsManager.Instance.ActivateReward(this);
        Debug.Log("Luck Increased By: " + _IncreaseLuck);
    }
}
