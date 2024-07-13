using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : Singleton<PointsManager>
{
    public int _Points = 0;

    public int GetPoints()
    {
        return _Points;
    }

    public void AddPoint()
    {
        _Points++;
    }

    public void AddPoint(int _val)
    {
        _Points += _val;
    }

    public void ResetPoints()
    {
        StatsManager.Instance.AddToTotalPointsAchieved(_Points);
        StatsManager.Instance.CheckHighestPointTotal(_Points);
        _Points = 0;
    }
}
