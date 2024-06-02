using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "GoldenItemTurnIncrease", menuName = "Rewards/GoldenItemTurnIncrease", order = 51)]
public class GoldenItemTurnIncrease : Reward
{

    [SerializeField] int _IncreaseTurns = 0;

    public override void Activate()
    {
        if (PayCost())
        {
            GameManager.Instance.IncreaseGoldItemBonus(_IncreaseTurns);
            RewardsManager.Instance.ActivateReward(this);
            Debug.Log("Gold Item turns Increased By: " + _IncreaseTurns);
        }
        else
            _Payable = false;
    }
}
