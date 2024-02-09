using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    public List<Shake> _Shakes = new List<Shake>();
    public List<RedShift> _Redshifts = new List<RedShift>();


   public void StartAllShake()
    {
        foreach(Shake s in _Shakes)
        {
            s.SetShaking();
        }
        foreach (RedShift s in _Redshifts)
        {
            s.StartRedShift();
        }
        // Code To vibrate but was too anoying in practice
        //if (SystemInfo.supportsVibration)
        //{
        //    Handheld.Vibrate();
        //}
    }

    public void StartPotionShake()
    {
        foreach(Shake s in _Shakes)
        {
            if(s._IsPotion)
            {
                s.SetShaking();
            }
        }
        foreach (RedShift s in _Redshifts)
        {
            if (s._IsPotion)
            {
                s.StartRedShift();
            }
        }
    }

    public void StopAllShaking()
    {
        foreach(Shake s in _Shakes)
        {
            s.StopShaking();
        }
        foreach (RedShift s in _Redshifts)
        {
            s.StopRedShift();
        }
    }
}
