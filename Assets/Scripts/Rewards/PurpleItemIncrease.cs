using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PurpleItemTurnIncrease", menuName = "Rewards/PurpleItemTurnIncrease", order = 51)]
public class PurpleItemIncrease : Reward
{
    [SerializeField] float _IncreaseAmount = 0;

    public override void Activate()
    {
        if (PayCost())
        {
            GameManager.Instance.IncreasePurpleItemChance(_IncreaseAmount);
            RewardsManager.Instance.ActivateReward(this);
            Debug.Log("Purple Item turns Increased By: " + _IncreaseAmount);
        }
        else
            _Payable = false;
    }

}
