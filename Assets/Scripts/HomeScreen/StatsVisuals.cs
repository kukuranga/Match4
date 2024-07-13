using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsVisuals : MonoBehaviour
{
    //Updates the values on the stats page

    public TextMeshProUGUI _levelsCompletedTxt;
    public TextMeshProUGUI _HighestLevelTxt;
    public TextMeshProUGUI _TotalRunsTxt;
    public TextMeshProUGUI _TotalPoints;
    public TextMeshProUGUI _HighestPointTxt;
    public TextMeshProUGUI _NumberOfMovesUsedTxt;
    public TextMeshProUGUI _HighestMoveTotalTxt;
    public TextMeshProUGUI _TotalNumberOfUpgrades;


    private void Update()
    {
        UpdateStats();
    }

    private void UpdateStats()
    {
        _levelsCompletedTxt.text = StatsManager.Instance._LevelsCompleted.ToString();
        _HighestLevelTxt.text = StatsManager.Instance._HighestLevelAchieved.ToString();
        _TotalRunsTxt.text = StatsManager.Instance._TotalNumberOfRuns.ToString();
        _TotalPoints.text = StatsManager.Instance._TotalPointsScored.ToString();
        _HighestPointTxt.text = StatsManager.Instance._HighestPointTotal.ToString();
        _NumberOfMovesUsedTxt.text = StatsManager.Instance._TotalNumberOfMovesUsed.ToString();
        _HighestMoveTotalTxt.text = StatsManager.Instance._HighestMovesNumberAchieved.ToString();
        _TotalNumberOfUpgrades.text = StatsManager.Instance._TotalNumberOfUpgrades.ToString();

    }
}
