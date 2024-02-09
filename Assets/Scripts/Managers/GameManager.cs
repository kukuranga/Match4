using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    //Todo: add logic to persist the number of moves available and add the moves on each level up

    public int _Level = 1;
    public SceneReference _Homepage;
    public SceneReference _GameOverScene;
    //Set scene to load when the game starts
    public SceneReference _LevelToLoad;
    private int _MovesToGive = 5;
    public int _RowsToGive = 1;
    public bool _GameOver = false;
    private int _MovesLeft = 0;

    //GoldItem
    [SerializeField] private float _GoldenItemChance = 0.3f;
    private int _GoldenItemBonus = 10;
    //Event chance


    private void CheckLevel()
    {
        Debug.Log("Level checkd");
        switch (_Level)
        {
            case 1:
                _MovesToGive = 5;
                _RowsToGive = 1;
                break;
            case 2:
                _MovesToGive = 3;
                _RowsToGive = 1;
                break;
            case 5:
                _MovesToGive = 3;
                _RowsToGive = 2;
                break;
            case 10:
                _MovesToGive = 10;
                _RowsToGive = 3;
                break;
            case 25:
                _MovesToGive = 8;
                _RowsToGive = 3;
                break;
            case 30:
                _MovesToGive = 5;
                _RowsToGive = 3;
                break;
            default:
                //_MovesToGive--;
                break;

        }
    }

    public void AddToGoldSpawnChance(float _Increase)
    {
        _GoldenItemChance += _Increase;
    }

    public void IncreaseGoldItemBonus(int _value)
    {
        _GoldenItemBonus += _value;
    }

    public int GetGoldenBonus()
    {
        return _GoldenItemBonus;
    }

    public bool SpawnGoldenItem()
    {
        float c = Random.Range(0f, 1f);
        if (c <= _GoldenItemChance)
            return true;

        return false;
    }

    public int GetMovesToGive()
    {
        return _MovesToGive;
    }

    public void StoreMoves(int n)
    {
        _MovesLeft = n;
    }

    public int SetMoves()
    {

        return _MovesLeft + _MovesToGive;
    }
    
    public void GameOver()
    {
        if (!_GameOver)
        {
            //SceneLoader.Instance.LoadScene(_GameOverScene);
            ResetGame();
            _GameOver = true;
        }
    }
    public void GameWon()
    {
        SetMoves();
        _Level++;
        CheckLevel();
    }

    public void ResetGame()
    {
        _Level = 1;
        _MovesLeft = 0;
        _GameOver = false;
        CheckLevel();
    }

    
    
}
