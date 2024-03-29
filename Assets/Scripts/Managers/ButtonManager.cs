using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ButtonManager : Singleton<ButtonManager>
{
    private int _BtnIndex = -1;
    private int _ContIndex = -1;
    public int RowNumber = 1;
    public int _CorrectAnswers = 0;
    public int _MovesLeft;

    public Animator _animator;
    public TextMeshProUGUI _WinText;
    public TextMeshProUGUI _MoveText;
    public GameObject _GameOverScreen;
    public GameObject _GameWonScreen;
    public GameObject _GameRewardsScreen;
    public RewardButton _RewardButton1;
    public RewardButton _RewardButton2;
    public RewardButton _RewardButton3;

    public List<Container> _Containers = new List<Container>();

    public List<Buttons> _ButtonsRow = new List<Buttons>();

    public Buttons _FirstClicked;
    public Buttons _SecondClicked;

    private bool _IsFirstActiveFrame = true;
    private CharacterController _CharacterController;

    private void Awake()
    {
        CreateButtons();
    }

    private void Start()
    {
        _MovesLeft = GameManager.Instance.SetMoves();
        RandomizeAndSetCorrectPositions();
    }

    private void Update()
    {

        if(_IsFirstActiveFrame)
        {
            SetCorrectNumbers();
            CheckIFAllCoorect();
            CheckPositions();
            _IsFirstActiveFrame = false;
        }

        _MoveText.text = _MovesLeft.ToString();

        _WinText.text = GameManager.Instance._Level.ToString();//_CorrectAnswers.ToString();
        if(_MovesLeft <= 0)
        {
            //GameManager.Instance.GameOver();
            _GameOverScreen.SetActive(true);
        }
    }
    
    //Resets Moves after golden Item is correctly placed
    private bool _AlreadyReset = false;
    public void ResetMoves() //TODO CHANGE THE VALUE TO DOUBLE THE AMOUNT OF MOVES LEFT ------------------------------------------------
    {
        if(!_AlreadyReset)
        {
            _AlreadyReset = true;
            _MovesLeft += GameManager.Instance.GetGoldenBonus();
        }
    }

    //Triggers when a button is selected
    public void SelectButtons(Buttons btn)
    {

        if(_FirstClicked == null)
        {
            _FirstClicked = btn;
            btn.Zoom(1.2f);
            btn.SetSelected();
            _CharacterController.SetAttack(true);
            VFXManager.Instance.StartPotionShake();
        }
        else if(_SecondClicked == null)
        {

            _SecondClicked = btn;
            _CharacterController.SetAttack(false);
            VFXManager.Instance.StopAllShaking();
            _FirstClicked.Zoom(1f);
            _FirstClicked.SetUnSelected();

            if (_FirstClicked == _SecondClicked)
            {
                _FirstClicked = null;
                _SecondClicked = null;
                return;
            }

            foreach (Buttons s in _ButtonsRow)
            {
                s.SetInteractable(false);
            }

            SwapPositionsAndContainers(_FirstClicked, _SecondClicked);


            _FirstClicked = null;
            _SecondClicked = null;

            foreach (Buttons s in _ButtonsRow)
            {
                s.SetInteractable(true);
            }

            CheckMoves();

            //CreateNewButtons();
        }
    }

    //Checks the number of moves left the player has
    private void CheckMoves()
    {
        _MovesLeft--;
        if(_MovesLeft <= 0)
        {
            GameManager.Instance.GameOver();
            _CharacterController.SetDead();
            _GameOverScreen.SetActive(true);
        }
    }

    //Swaps the position of the buttons
    private void SwapPositionsAndContainers(Buttons a, Buttons b)
    {

        //Store each container here
        Container cA = a._Container;
        Container cB = b._Container;

        //set each buttons container to the new container
        a._Container = cB;
        b._Container = cA;

        Vector3 aV = a.transform.position;
        Vector3 bV = b.transform.position;

        a.transform.position = bV;
        b.transform.position = aV;

        a.ResetAnchor();
        b.ResetAnchor();
        CheckPositions();

    }

    //----- Collection of getters and setters for various components tracked by this script ----------------------------
    public int GetBtnIndex()
    {
        _BtnIndex++;
        return _BtnIndex;
    }

    int _SetConInt = -1;
    public Container SetContainer()
    {      
        _SetConInt++;
        return _Containers[_SetConInt];
    }

    public int GetContIndex()
    {
        _ContIndex++;
        return _ContIndex;
    }

    public void AddToContainers(Container Cont)
    {
        _Containers.Add(Cont);
    }

    public void AddToRows(Buttons btn)
    {
        _ButtonsRow.Add(btn);
    }

    public void SetCharacterController(CharacterController c)
    {
        _CharacterController = c;
    }

    //Checks if the images are in the correct positions
    private void CheckPositions()
    {
        SetCorrectNumbers();

        CheckBtnCorrect();

        if(_CorrectAnswers == _ButtonsRow.Count)
        {
            if (RewardsManager.Instance.RollForRewards())
            {
                RewardsManager.Instance.SetRewards(_RewardButton1, _RewardButton2, _RewardButton3);
                _GameRewardsScreen.SetActive(true);
            }
            else
                _GameWonScreen.SetActive(true);

            GameManager.Instance.StoreMoves(_MovesLeft);
            _CharacterController.SetAttack(true);
            VFXManager.Instance.StartPotionShake();
            _Containers[0].SetWave(true);
            GameManager.Instance.GameWon();
        }
    }

    //Checks which buttons are on the correct positions
    private void CheckBtnCorrect()
    {
        foreach (Buttons btn in _ButtonsRow)
        {
            btn.CheckCorrect();
        }
    }

    //Sets a preset for each buttons correct position and Randomizes the positions
    private void RandomizeAndSetCorrectPositions()
    {
        AssignNumbers();
        
    }

    //Return the number of images in the correct place
    private void SetCorrectNumbers()
    {
        _CorrectAnswers = 0;
        foreach (Buttons btns in _ButtonsRow)
        {
            if (btns._Container != null)
            {
                if (btns._CorrectPosition == btns._Container._Index)
                    _CorrectAnswers++;
            }
        }
    }

    //Random number list to assign the correct number
    private List<int> uniqueNumbers = new List<int>();

    //Assigns the correct positions for each button 
    private void AssignNumbers()
    {
        uniqueNumbers.Clear();
        int n = 0;
        foreach(Buttons btn in _ButtonsRow)
        {
            uniqueNumbers.Add(n);
            n++;
        }

        foreach(Buttons btn in _ButtonsRow)
        {
            int val = Random.Range(0, uniqueNumbers.Count);
            btn._CorrectPosition = uniqueNumbers[val];
            uniqueNumbers.RemoveAt(val);
        }
        SetCorrectNumbers();
        CheckIFAllCoorect();
        //SetCorrectNumbers();
        Debug.Log("assigned numbers checked");

    }

    //Checks the number of correct numbers and resguffles them if its past a specific threshold
    private void CheckIFAllCoorect()
    {

        if (_CorrectAnswers >= 1)
        {
            AssignNumbers();
            Debug.Log("Reshufled positions");
            CheckIFAllCoorect();
        }
    }

    //----------------------------------Button and container Creation ---------------------------------------------------------------------
    public GameObject ContainerPrefab;
    public GameObject buttonPrefab;
    public GameObject ContainerGO;
    public GameObject ButtonGO;
    public int rows;
    public int columns = 3;
    public float buttonSpacing = 100f;

    void CreateButtons()
    {

        rows = GameManager.Instance._RowsToGive;

        // Calculate total width and height of the grid
        float totalWidth = columns * (ContainerPrefab.GetComponent<RectTransform>().sizeDelta.x + buttonSpacing);
        float totalHeight = rows * (ContainerPrefab.GetComponent<RectTransform>().sizeDelta.y + buttonSpacing);

        // Calculate starting positions
        float startX = ContainerGO.transform.position.x;
        float startY = ContainerGO.transform.position.y;

        // Instantiate Containers in a grid
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position for each button
                float x = startX + col * (ContainerPrefab.GetComponent<RectTransform>().sizeDelta.x + buttonSpacing);
                float y = startY - row * (ContainerPrefab.GetComponent<RectTransform>().sizeDelta.y + buttonSpacing + 100f);

                // Instantiate the button
                GameObject button = Instantiate(ContainerPrefab, new Vector3(x, y, 0f), Quaternion.identity, ContainerGO.transform);
            }
        }

        //Spawn Buttons here
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Calculate position for each button
                float x = startX + col * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.x + buttonSpacing);
                float y = startY - row * (buttonPrefab.GetComponent<RectTransform>().sizeDelta.y + buttonSpacing + 100f);

                // Instantiate the button
                GameObject button = Instantiate(buttonPrefab, new Vector3(x, y, 0f), Quaternion.identity, ContainerGO.transform);
                
            }
        }

        RandomizeSprites();
    }

    private void RandomizeSprites()
    {
        foreach(Buttons btn in _ButtonsRow)
        {
            btn.SetSprite(GetRandomSprite());
        }
        if (GameManager.Instance.SpawnGoldenItem())
        {
            int a = Random.Range(0, _ButtonsRow.Count);
            _ButtonsRow[a]._GoldenItem = true;
            _ButtonsRow[a]._Image.color = Color.yellow;
        }
    }

    Sprite GetRandomSprite()
    {
        Sprite[] array = SpriteManager.Instance.GetMageItems();

        // Check if the array is not empty
        if (array != null && array.Length > 0)
        {
            // Generate a random index within the array's length
            int randomIndex = Random.Range(0, array.Length);

            // Return the element at the random index
            return array[randomIndex];
        }
        else
        {
            Debug.LogError("Array is empty or null.");
            return null; // or handle the situation accordingly
        }
    }

}
