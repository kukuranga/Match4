using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : Singleton<ButtonManager>
{
    //Notes: each button will need an index number to ref
    //:options: Create a struct that contains the original placing of the button,
    //:-- The ref number assigned to it
    //:-- its correct placing and current placing

    private int _index = 0;
    public int RowNumber = 1;

    public GameObject prefab;
    public GameObject Original;
    public GameObject Parent;

    public List<Buttons> _FirstRow = new List<Buttons>();
    public List<Buttons> _SecondRow = new List<Buttons>();
    public List<Buttons> _ThirdRow = new List<Buttons>();

    public Buttons _FirstClicked;
    public Buttons _SecondClicked;

    private void Start()
    {
        //Get all button positions
        //set all buttons to onclick set position and on second button click swap positions and update the checker
       
        
        
    }

    public void SelectButtons(Buttons btn)
    {
        if(_FirstClicked == null)
        {
            _FirstClicked = btn;
        }
        else if(_SecondClicked == null)
        {
            _SecondClicked = btn;

            //Swap Position Logic
            Vector2 pos1 = _FirstClicked.transform.position;
            Vector2 pos2 = _SecondClicked.transform.position;

            _FirstClicked.transform.position = pos2;
            _SecondClicked.transform.position = pos1;

            _FirstClicked = null;
            _SecondClicked = null;

            //CreateNewButtons();
        }
    }

    public int GetIndex()
    {
        _index++;
        return _index;
    }

    public void AddToRows(Buttons btn)
    {
        switch (RowNumber)
        {
            case 1:
                _FirstRow.Add(btn);
                break;
            case 2:
                _SecondRow.Add(btn);
                break;
            case 3:
                _ThirdRow.Add(btn);
                break;
        }
    }

    public void CreateNewButtons()
    {
        


        //RowNumber++;
        if (RowNumber > 3) 
        {
            //End Game
        }

        
        Vector3 s = new Vector3(Original.transform.position.x, Original.transform.position.y - 100, Original.transform.position.z);

        Original = Instantiate(Original, s, Quaternion.identity, Parent.transform);
        
        switch (RowNumber - 1)
        {
            case 1:
                foreach (Buttons btn in _FirstRow)
                {
                    //btn._Interactable = false;
                }
                break;
            case 2:
                foreach (Buttons btn in _SecondRow)
                {
                    btn._Interactable = false;
                }
                break;
            case 3:
                foreach (Buttons btn in _ThirdRow)
                {
                    btn._Interactable = false;
                }
                break;
        }
    }

    //Checks if the images are in the correct positions
    private void CheckPositions()
    {

    }

    //Sets a preset for each buttons correct position and Randomizes the positions
    private void RandomizeAndSetCorrectPositions()
    {

    }

    //Return the number of images in the correct place
    private int GetCorrectNumbers()
    {

        return 0;
    }
}
