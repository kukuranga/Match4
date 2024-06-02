using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldReward", menuName = "Rewards/GoldReward", order = 51)]
public class GoldItemReward : Reward
{

    //Influences how likely a gold item is to spawn
    [SerializeField] float _IncreaseToGoldItemSpawnRate = 0.0f;

    public override void Activate()
    {
        if (PayCost())
        {
            GameManager.Instance.AddToGoldSpawnChance(_IncreaseToGoldItemSpawnRate);
            RewardsManager.Instance.ActivateReward(this);
            Debug.Log("Gold Item Spawn Rate Increased By: " + _IncreaseToGoldItemSpawnRate);
        }
        else
            _Payable = false;
    }
}
