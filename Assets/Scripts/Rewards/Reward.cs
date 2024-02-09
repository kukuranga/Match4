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

    public virtual void Activate()
    {
        Debug.Log(this + ": Activate Method has not been overriden");
    }

}
