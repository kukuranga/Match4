using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseMovesPerTurn", menuName = "Rewards/IncreaseMovesPerTurn", order = 51)]
public class MovesPerRoundIncrease : Reward
{
    [SerializeField] int _IncreaseAmount = 0;

    public override void Activate()
    {
        if (PayCost())
        {
            RewardsManager.Instance.IncreaseMovesEarned(_IncreaseAmount);
            RewardsManager.Instance.ActivateReward(this);
            Debug.Log("Moves Increased By: " + _IncreaseAmount);
        }
        else
            _Payable = false;
    }
}
