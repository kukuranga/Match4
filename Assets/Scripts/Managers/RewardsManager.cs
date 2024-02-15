using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsManager : Singleton<RewardsManager>
{

    //Reward or power up Screen
    public Sprite _TreasureSprite;
    [SerializeField] private List<Reward> _Rewards = new List<Reward>();
    private List<Reward> _CommonRewards = new List<Reward>();
    private List<Reward> _UncommonRewards = new List<Reward>();
    private List<Reward> _RareRewards = new List<Reward>();
    private List<Reward> _MythicRewards = new List<Reward>();

    private List<Reward> _ActiveRewards = new List<Reward>();
    [SerializeField] private float _RewardsChance = 0.5f;
    [Range(0,9)]
    public int _Luck = 0;  
    private float[,] _LuckTable = new float[,]
    {
        {95f,90f,80f,75f,70f,60f,45f,30f,20f,10f},//Common
        {3f,8f,15f,20f,20f,30f,40f,50f,50f,55f},//Uncommon
        {1.5f,1f,4f,4f,7f,6f,10f,13f,20f,22f },//Rare
        {0.5f,1f,2f,2f,3f,4f,5f,7f,10f,13f},//Mythic
    };

    //public void AddToRewards(Reward r)
    //{
    //    _Rewards.Add(r);
    //}

    private void Start()
    {

        //Adds each reward to its correct list
        SortRewards(_Rewards);
    }

    private void SortRewards(List<Reward> rewards)
    {
        foreach (Reward r in rewards)
        {
            switch (r._Rarity)
            {
                case Rarity.Common:
                    _CommonRewards.Add(r);
                    break;
                case Rarity.uncommon:
                    _UncommonRewards.Add(r);
                    break;
                case Rarity.Rare:
                    _RareRewards.Add(r);
                    break;
                case Rarity.Mythic:
                    _MythicRewards.Add(r);
                    break;
            }
        }
    }

    public void ActivateReward(Reward r)
    {
        //Maybe remove from rewards list
        _ActiveRewards.Add(r);
    }

    public void IncreaseLuck(int rate)
    {
        _Luck += rate;
    }

    public void SetRewards(RewardButton r1, RewardButton r2, RewardButton r3 )
    {
        //Todo: chose a random reward to set up on each of these buttons based on rarity and chance of showing up
        //set the image text and info for each reward here

        List<Reward> re = GetAllRewards();

        r1._Reward = re[0];
        r2._Reward = re[1];
        r3._Reward = re[2];
        SetButtonProperties(r1);
        SetButtonProperties(r2);
        SetButtonProperties(r3);

    }

    public bool RollForRewards()
    {
        float r = Random.Range(0f, 1f);
        if (r < _RewardsChance)
            return true;
        return false;
    }

    private List<Reward> GetAllRewards()
    {
        List<Reward> rewards = new List<Reward>();

        Reward a = GetAReward();
        Reward b = GetAReward();
        Reward c = GetAReward();


        rewards.Add(a);
        rewards.Add(b);
        rewards.Add(c);

        SortRewards(rewards);
        return rewards;

    }


    private Reward GetAReward()
    {
        //shuffle up all rewards and bring one up based on the rarity
        //Luck factor should add to each buttons chance of aprearing
        //the higher the luck factor the higher the chance of each rarity apearing
        //this value should luck 1 - 10;
        float _comm = _LuckTable[0, _Luck];
        float _uncomm = _LuckTable[1, _Luck];
        float _rare = _LuckTable[2, _Luck];
        float _myth = _LuckTable[3, _Luck];

        float _val = Random.Range(0f, 100f);

        if(_val < _myth)
        {
            Reward r = _MythicRewards[Random.Range(0, _MythicRewards.Count)];
            _MythicRewards.Remove(r);
            return r;
        }
        else if (_val < _rare)
        {
            Reward r = _RareRewards[Random.Range(0, _RareRewards.Count)];
            _RareRewards.Remove(r);
            return r;
        }
        else if (_val < _uncomm)
        {
            Reward r =  _UncommonRewards[Random.Range(0, _UncommonRewards.Count)];
            _UncommonRewards.Remove(r);
            return r;
        }
        else
        {
            Reward r = _CommonRewards[Random.Range(0, _CommonRewards.Count)];
            _CommonRewards.Remove(r);
            return r;
        }
    }

    private void SetButtonProperties(RewardButton r)
    {
        //Set the properties of the buttons based on the reward assosciated for them
        r._Image.sprite = r._Reward._Image;
        r._Text.text = r._Reward._Description;
    }
    
}
