using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Rarity{
    Common,
    uncommon,
    Rare, 
    Mythic
}

public class Reward : ScriptableObject
{
    //this script dectates a single reward
    public Sprite _Image;
    public string _Description; 
    public Rarity _Rarity;
    public int _Cost;
    public bool _Payable = true;

    public virtual void Activate()
    {
        Debug.Log(this + ": Activate Method has not been overriden");
    }

    public bool PayCost()
    {
        int m = ButtonManager.Instance._MovesLeft;

        if (m > _Cost)
        {
            StatsManager.Instance.AddToTotalNumberOfUpgrades(1);
            ButtonManager.Instance.PayCost(_Cost);
            _Payable = true;
            return true;
        }

        return false;
    }

}
