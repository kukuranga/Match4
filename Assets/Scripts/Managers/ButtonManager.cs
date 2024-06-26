using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonManager : Singleton<ButtonManager>//, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private int _BtnIndex = -1;
    private int _ContIndex = -1;
    public int RowNumber = 1;
    public int _CorrectAnswers = 0;
    public int _MovesLeft;

    public int _MovingItems = 0;
    public TextMeshProUGUI _WinText;
    public TextMeshProUGUI _MoveText;
    public GameObject _GameOverScreen;
    public GameObject _GameWonScreen;
    public GameObject _GameRewardsScreen;
    public RewardButton _RewardButton1;
    public RewardButton _RewardButton2;
    public RewardButton _RewardButton3;

    public List<Container> _Containers = new();
    public List<Buttons> _ButtonsRow = new();

    public Buttons _FirstClicked;
    public Buttons _SecondClicked;
    //public bool _AlreadySpawnedPurpleItem = false;

    private bool _IsFirstActiveFrame = true;
    public float _ItemMoveSpeed = 2300f;
    private List<Buttons> _OrderedButtonList = new();

    private List<GameObject> _InstatiatedButtons = new();



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

        _WinText.text = GameManager.Instance._Level.ToString();
        if(_MovesLeft <= 0)
        {
            _GameOverScreen.SetActive(true);
        }
    }

    //TODO: FIX LOGIC TO RESET THE GAME WITHOUT THE NEED FOR RELOADING THE SCENE -------------------------------------------------------------------------------------
    public void ResetGame()
    {
        //TODO: - clear all previously spwned buttons
        //      - set all ui elements to inactive
        foreach(GameObject g in _InstatiatedButtons)
        {
            g.SetActive(false);
        }// ToDo: FIX ----------------------------------------------------------------------------------------------------------------

        CreateButtons();
        _MovesLeft = GameManager.Instance.SetMoves();
        RandomizeAndSetCorrectPositions();
    }
    
    //resets moves when a gold item triggers
    public void ResetMoves()
    {
        _MovesLeft += GameManager.Instance.GetGoldenBonus();
    }

    //TODO: Change logic to only work either one way up down left oo right ----------------------------------------------------------------

    
    public void SelectButtons(Buttons btn , int Direction) //called on buttons drag
    {
        _FirstClicked = btn;
        bool _CanMove = false;

        int index = GetButtonInOrderedList(btn);

        // 1:Up 2:Down 3:Right 4:Left
        switch(Direction)
        {
            case 1:
                _CanMove = CheckUp(index);
                break;
            case 2:
                _CanMove = CheckDown(index);
                break;
            case 3:
                _CanMove = CheckRight(index);
                break;
            case 4:
                _CanMove = CheckLeft(index);
                break;
        }

        if (_CanMove)
        {
            //st first & second click than move them
            _FirstClicked = btn;
            _FirstClicked.SetUnSelected();
            _FirstClicked.Zoom(1f);
            
            if(_FirstClicked._ItemType == ItemType.MotionItem && _SecondClicked._ItemType == ItemType.MotionItem)
            {
                PurpleItemClash();
            }
            else if (_FirstClicked._ItemType == ItemType.MotionItem || _SecondClicked._ItemType == ItemType.MotionItem)
            {
                PurpleItemMove();
            }
            else if (_FirstClicked._ItemType == ItemType.SemiMotionItem || _SecondClicked._ItemType == ItemType.SemiMotionItem)
            {
                YellowItemMove();
            }
            else if (_FirstClicked._ItemType == ItemType.FrozenItem || _SecondClicked._ItemType == ItemType.FrozenItem)
            {
                //Todo: add a tell (ANIMATION OR SFX) to show its not interactable
                _MovesLeft++;
            }
            else if (_FirstClicked._ItemType == ItemType.RedItem || _SecondClicked._ItemType == ItemType.RedItem)
            {
                //TODO: Make the move penalty happen after the animation ends
                OverwierManager.Instance.FadeIn(GameManager.Instance._RedItemColorChange, 0.4f, 0.5f);
                _MovesLeft--;
                SwapPositionsAndContainers(_FirstClicked, _SecondClicked);
            }
            else
            {
                SwapPositionsAndContainers(_FirstClicked, _SecondClicked);
            }
            _FirstClicked = null;
            _SecondClicked = null;

            

            _MovesLeft--;
            //CheckMoves();
        }

    }

    #region Check if the drag location is possible
    private bool CheckUp(int indx)
    {
        int val = indx - 3;
        if(val < 0)
        {
            return false;
        }
        _SecondClicked = _OrderedButtonList[val];
        return true;

    }
    private bool CheckDown(int indx)
    {
        int val = indx + 3;
        if (val > (GameManager.Instance._RowsToGive * 3) - 1)
        {
            return false;
        }
        _SecondClicked = _OrderedButtonList[val];
        return true;
    }
    private bool CheckLeft(int indx)
    {
        int val = indx - 1;
        if (val < 0 || val == 2 || val == 5 || val == 8)
        {
            return false;
        }
        _SecondClicked = _OrderedButtonList[val];
        return true;
    }
    private bool CheckRight(int indx)
    {
        int val = indx + 1;
        if (val > (GameManager.Instance._RowsToGive * 3) - 1 || val == 3 || val == 6)
        {
            return false;
        }
        _SecondClicked = _OrderedButtonList[val];
        return true;
    }
    #endregion

    #region Ability to select any two items logic
    //public void SelectButtons(Buttons btn)
    //{

    //    if(_FirstClicked == null)
    //    {
    //        _FirstClicked = btn;
    //        btn.Zoom(1.2f);
    //        btn.SetSelected();
    //        _CharacterController.SetAttack(true);
    //        VFXManager.Instance.StartPotionShake();
    //    }
    //    else if(_SecondClicked == null)
    //    {

    //        _SecondClicked = btn;
    //        _CharacterController.SetAttack(false);
    //        VFXManager.Instance.StopAllShaking();
    //        _FirstClicked.Zoom(1f);
    //        _FirstClicked.SetUnSelected();

    //        if (_FirstClicked == _SecondClicked)
    //        {
    //            _FirstClicked = null;
    //            _SecondClicked = null;
    //            return;
    //        }

    //        foreach (Buttons s in _ButtonsRow)
    //        {
    //            s.SetInteractable(false);
    //        }

    //        if (_FirstClicked._ItemType == ItemType.MotionItem || _SecondClicked._ItemType == ItemType.MotionItem)
    //        {
    //            PurpleItemMove();
    //        }
    //        else
    //        {
    //            SwapPositionsAndContainers(_FirstClicked, _SecondClicked);
    //        }
    //        _FirstClicked = null;
    //        _SecondClicked = null;

    //        foreach (Buttons s in _ButtonsRow)
    //        {
    //            s.SetInteractable(true);
    //        }

    //        CheckMoves();
    //    }
    //}
    #endregion

    //Checks the number of moves left the player has
    private void CheckMoves()
    {      
        if(_MovesLeft <= 0)
        {
            GameManager.Instance.GameOver();
            _GameOverScreen.SetActive(true);
        }
    }

    //Subtracts the Cost From the Number of moves Left
    public void PayCost(int cost)
    {
        _MovesLeft -= cost;
        CheckMoves();
    }

    //Swaps the position of the buttons
    private void SwapPositionsAndContainers(Buttons a, Buttons b)
    {
        a.SetInteractable(false);
        b.SetInteractable(false);
        //Store each container here
        Container cA = a._Container;
        Container cB = b._Container;

        //set each buttons container to the new container
        a._Container = cB;
        b._Container = cA;

        Vector2 aV = a.GetRectTransform().anchoredPosition;
        Vector2 bV = b.GetRectTransform().anchoredPosition;

        StartCoroutine(MoveToPosition(a, aV));
        StartCoroutine(MoveToPosition(b, bV));
        
    }

    IEnumerator MoveToPosition(Buttons a , Vector2 target)
    {
        _MovingItems++;
        RectTransform _r = a.GetRectTransform();
        Vector2 startPosition = _r.anchoredPosition; //a.transform.position;
        Vector2 targetPosition = a._Container.GetRectTransform().anchoredPosition; //target;

        // Calculate the distance to move
        float distance = Vector2.Distance(startPosition, targetPosition);
        a.SetInteractable(false);
        a.SetDance(false);

        // Move towards the target position while the distance is greater than a small threshold
        while (distance > 0.1f)
        {
            // Calculate the new position based on current position, target position, and speed
            Vector2 newPosition = Vector2.MoveTowards(_r.anchoredPosition, targetPosition, _ItemMoveSpeed * Time.deltaTime);

            // Update the UI object's position
            _r.anchoredPosition = newPosition;

            // Update the distance to the target position
            distance = Vector2.Distance(_r.anchoredPosition, targetPosition);

            // Wait for the next frame
            yield return null;
        }

        // Ensure precise positioning at the target position
        _r.anchoredPosition = targetPosition;

        a.ResetAnchor();
        a.SetInteractable(true);
        a.SetDance(true);
        _MovingItems--;
        CheckPositions();
        CheckMoves();
        

        yield return null;
    }

    public void PurpleItemClash()
    {
        OrderButtonList();
        StartCoroutine(RandomizeAllItems());
    }

    bool _PurpleCanMove = true;
    public void PurpleItemMove()
    {
        if (_PurpleCanMove)
        {
            //OverwierManager.Instance.FadeIn(GameManager.Instance._PurpleItemColorChange,0.2f, 0.4f);

            _PurpleCanMove = false;
            OrderButtonList();

            int one = 0;
            int two = 0;
            int _sone = 0;
            int _stwo = 0;
            foreach (Buttons btn in _OrderedButtonList)
            {
                if (btn == _FirstClicked)
                {
                    _sone++;
                    break;
                }
                _sone++;
            }
            foreach (Buttons btn in _OrderedButtonList)
            {
                if (btn == _SecondClicked)
                {
                    _stwo++;
                    break;
                }
                _stwo++;
            }

            for (int i = 0; i < _OrderedButtonList.Count; i++)
            {
                if (_OrderedButtonList[i] == _FirstClicked)
                {
                    if (i < 3)
                        one = 1;
                    else if (i < 6)
                        one = 2;
                    else
                        one = 3;
                    break;
                }
            }
            for (int i = 0; i < _OrderedButtonList.Count; i++)
            {
                if (_OrderedButtonList[i] == _SecondClicked)
                {
                    if (i < 3)
                        two = 1;
                    else if (i < 6)
                        two = 2;
                    else
                        two = 3;
                    break;
                }
            }

            int difference = (one - two);
            int sdiff = (_sone - _stwo);

            if (_SecondClicked._ItemType == ItemType.MotionItem)
            {
                difference = -difference;
                sdiff = -sdiff;
            }

            if (difference >= 1)
            {
                StartCoroutine(MoveAllUp());
            }
            else if (difference <= -1)
            {
                StartCoroutine(MoveAllDown());
            }
            else if (sdiff >= 1)
            {
                StartCoroutine(MoveAllLeft());
            }
            else if (sdiff <= -1)
            {
                StartCoroutine(MoveAllRight());
            }
        }
    }

    private void EndPurpleMove()
    {
        foreach (Buttons s in _ButtonsRow)
        {
            s.SetInteractable(true);
        }
        _PurpleCanMove = true;
    }

    bool _yellowCanMove = true;
    private void YellowItemMove()
    {
        if (_yellowCanMove)
        {
            //OverwierManager.Instance.FadeIn(GameManager.Instance._PurpleItemColorChange,0.2f, 0.4f);

            _yellowCanMove = false;
            OrderButtonList();

            int one = 0;
            int two = 0;
            int _sone = 0;
            int _stwo = 0;
            foreach (Buttons btn in _OrderedButtonList)
            {
                if (btn == _FirstClicked)
                {
                    _sone++;
                    break;
                }
                _sone++;
            }
            foreach (Buttons btn in _OrderedButtonList)
            {
                if (btn == _SecondClicked)
                {
                    _stwo++;
                    break;
                }
                _stwo++;
            }

            for (int i = 0; i < _OrderedButtonList.Count; i++)
            {
                if (_OrderedButtonList[i] == _FirstClicked)
                {
                    if (i < 3)
                        one = 1;
                    else if (i < 6)
                        one = 2;
                    else
                        one = 3;
                    break;
                }
            }
            for (int i = 0; i < _OrderedButtonList.Count; i++)
            {
                if (_OrderedButtonList[i] == _SecondClicked)
                {
                    if (i < 3)
                        two = 1;
                    else if (i < 6)
                        two = 2;
                    else
                        two = 3;
                    break;
                }
            }


            int _CPos = 0;
            int _RPos = 0;
            if (_FirstClicked._ItemType == ItemType.SemiMotionItem)
            {
                _CPos = GetColumnNumber(_FirstClicked);
                _RPos = GetRowNumber(_FirstClicked);
            }
            else
            {
                _CPos = GetColumnNumber(_SecondClicked);
                _RPos = GetRowNumber(_SecondClicked);
            }
            
            int difference = (one - two);
            int sdiff = (_sone - _stwo);

            if (_SecondClicked._ItemType == ItemType.MotionItem)
            {
                difference = -difference;
                sdiff = -sdiff;
            }

            foreach (Buttons s in _ButtonsRow)
            {
                s.SetInteractable(false);
            }

            OrderButtonList();

            if (difference >= 1)
            {
                StartCoroutine(MoveSectionUp(_CPos));
            }
            else if (difference <= -1)
            {
                StartCoroutine(MoveSectionDown(_CPos));
            }
            else if (sdiff >= 1)
            {
                StartCoroutine(MoveSectionLeft(_RPos));
            }
            else if (sdiff <= -1)
            {
                StartCoroutine(MoveSectionRight(_RPos));
            }

            //todo remove this for testing
            //_yellowCanMove = true;
        }
    }

    IEnumerator RandomizeAllItems()
    {
        List<int> _Nums = new();
        int v = 0;
        foreach (var b in _OrderedButtonList)
        {
            _Nums.Add(v);
            v++;
        }

        // Randomize the list
        RandomizeList(_Nums);

        // Temporary list to store new container assignments
        List<Container> newContainers = new List<Container>(_OrderedButtonList.Count);

        // Ensure the newContainers list has the same length as _OrderedButtonList
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            newContainers.Add(null);
        }

        // Assign every button in order to the new container using the randomized list
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            newContainers[i] = _OrderedButtonList[_Nums[i]]._Container;
        }

        // Apply the new container assignments
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            _OrderedButtonList[i]._Container = newContainers[i];
        }

        // Move to container
        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        EndPurpleMove();

        yield return null;
    }
    void RandomizeList<T>(List<T> list)
    {
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(i, count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    #region Movement Coroutines
    IEnumerator MoveAllDown()
    {
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            int p = i + 3;

            int val = _OrderedButtonList.Count;
            if (p >= val )
            {
                p -= val;
            }

            _OrderedButtonList[i]._Container = _Containers[p];
        }

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }


        CheckPositions();
        EndPurpleMove();

        yield return null;
    }

    IEnumerator MoveAllRight()
    {
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            int p = i + 1;

            int val = _OrderedButtonList.Count;
            if(p == val || p == val - 3 || p == val - 6)
            {
                p -= 3;
            }
            _OrderedButtonList[i]._Container = _Containers[p];
        }

        foreach(Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        EndPurpleMove();

        yield return null;
    }

    IEnumerator MoveAllUp()
    {
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            int p = i - 3;

            int val = _OrderedButtonList.Count;
            if (p < 0)
            {
                p += val;
            }

            _OrderedButtonList[i]._Container = _Containers[p];
        }

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }


        CheckPositions();
        EndPurpleMove();

        yield return null;
    }

    IEnumerator MoveAllLeft()
    {
        for (int i = 0; i < _OrderedButtonList.Count; i++)
        {
            int p = i - 1;

            int val = _OrderedButtonList.Count;
            if (p < 0 || p == 2 || p == 5)
            {
                p += 3;
            }
            _OrderedButtonList[i]._Container = _Containers[p];
        }

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        EndPurpleMove();

        yield return null;
    }

    IEnumerator MoveSectionDown(int _col)
    {
        int _ref = _col - 1;

        _OrderedButtonList[_ref]._Container = _Containers[_ref + 3];

        _OrderedButtonList[_ref +3]._Container = _Containers[_ref + 6];

        _OrderedButtonList[_ref +6]._Container = _Containers[_ref];

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        foreach (Buttons s in _ButtonsRow)
        {
            s.SetInteractable(true);
        }
        _yellowCanMove = true;

        yield return null;
    }

    IEnumerator MoveSectionRight(int _row)
    {
        int _ref = (_row - 1) * 3;

        _OrderedButtonList[_ref]._Container = _Containers[_ref + 1];

        _OrderedButtonList[_ref + 1]._Container = _Containers[_ref + 2];

        _OrderedButtonList[_ref + 2]._Container = _Containers[_ref];

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        foreach (Buttons s in _ButtonsRow)
        {
            s.SetInteractable(true);
        }
        _yellowCanMove = true;

        yield return null;
    }
    IEnumerator MoveSectionUp(int _col)
    {
        int _ref = _col - 1;

        _OrderedButtonList[_ref]._Container = _Containers[_ref + 6];

        _OrderedButtonList[_ref +3]._Container = _Containers[_ref];

        _OrderedButtonList[_ref +6]._Container = _Containers[_ref + 3];

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        foreach (Buttons s in _ButtonsRow)
        {
            s.SetInteractable(true);
        }
        _yellowCanMove = true;

        yield return null;
    }

    IEnumerator MoveSectionLeft(int _row)
    {
        int _ref = (_row - 1) * 3;

        _OrderedButtonList[_ref]._Container = _Containers[_ref + 2];

        _OrderedButtonList[_ref + 1]._Container = _Containers[_ref];

        _OrderedButtonList[_ref + 2]._Container = _Containers[_ref + 1];

        foreach (Buttons _btn in _ButtonsRow)
        {
            _btn.MoveToContainer();
        }

        CheckPositions();
        foreach (Buttons s in _ButtonsRow)
        {
            s.SetInteractable(true);
        }
        _yellowCanMove = true;

        yield return null;
    }
    #endregion

    private void OrderButtonList()
    {
        _OrderedButtonList.Clear();

        foreach(Container _c in _Containers)
        {
            foreach (Buttons _b in _ButtonsRow)
            {
                if (_b._Container == _c)
                {
                    _OrderedButtonList.Add(_b);
                    break;
                }
            }
        }
    }

    private int GetButtonInOrderedList(Buttons btn)
    {
        OrderButtonList();

        //Return the buttons position in the order button list

        int i = 0;
        foreach(Buttons b in _OrderedButtonList)
        {
            if(btn == b)
            {
                return i;
            }
            i++;
        }

        return 0;

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

    private int GetColumnNumber(Buttons _btn)
    {
        OrderButtonList();

        int i = 0;

        foreach (Buttons b in _OrderedButtonList)
        {
            if (b == _btn)
                break;
            i++;
        }

        if (i == 0 || i == 3 || i == 6)
            return 1;
        else if (i == 1 || i == 4 || i == 7)
            return 2;
        else
            return 3;
    }

    private int GetRowNumber(Buttons _btn)
    {
        OrderButtonList();

        int i = 0;

        foreach (Buttons b in _OrderedButtonList)
        {
            if (b == _btn)
                break;
            i++;
        }

        if (i < 3)
            return 1;
        else if (i > 2 && i < 6)
            return 2;
        else
            return 3;
    }

    bool _GameWon = false;
    //Checks if the images are in the correct positions
    private void CheckPositions()
    {
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        SetCorrectNumbers();

        CheckBtnCorrect();

        if(_CorrectAnswers == _ButtonsRow.Count && !_GameWon)
        {
            _GameWon = true;
            _GameWonScreen.SetActive(true);
            SynthManager.Instance.RandomUpgrade();


            GameManager.Instance.StoreMoves(_MovesLeft);
            _Containers[0].SetWave(true);
            GameManager.Instance.GameWon();
        }

        //FREEZE ITEM CODE
        OrderButtonList();
        foreach (Buttons _btn in _OrderedButtonList)
        {
            if (_btn._ItemType == ItemType.FrozenItem)
            {
                List<Buttons> _sur = GetSurroundingObjectsCardinal(_btn);
                foreach (Buttons _b in _sur)
                {
                    if (_b.IsCorrect())
                    {
                        _btn.Unfreeze();
                    }
                }
            }
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
    private List<int> uniqueNumbers = new();

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
                _InstatiatedButtons.Add(button);
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
                _InstatiatedButtons.Add(button);
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
        if(RewardsManager.Instance.RollForRewards()) 
        {
            RewardsManager.Instance.SetRewards(_RewardButton1, _RewardButton2, _RewardButton3);
            int a = GetNormalItem();
            _ButtonsRow[a].SetTreasureItem();
        }
    }

    private List<int> normalItemIndices = new List<int>();
    //Returns the number of normal Items
    private int GetNormalItem()
    {
        if (normalItemIndices.Count == 0)
        {
            for(int i = 0; i< _ButtonsRow.Count; i++)
            {
                if (_ButtonsRow[i]._ItemType == ItemType.NormalItem)
                    normalItemIndices.Add(i);
            }
        }

        int a = Random.Range(0,normalItemIndices.Count);
        normalItemIndices.Remove(a);

        return a;
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
    public List<Buttons> GetSurroundingObjectsCardinal(Buttons targetObject)
    {
        List<Buttons> surroundingObjects = new List<Buttons>();

        // Find the index of the target object in the list
        int targetIndex = _OrderedButtonList.IndexOf(targetObject);
        int gridSize = 3;// GameManager.Instance._RowsToGive;

        if (targetIndex == -1)
        {
            Debug.LogError("Object not found in the list.");
            return surroundingObjects;
        }

        // Calculate the row and column of the target object in the 3x3 grid
        int targetRow = targetIndex / 3;
        int targetColumn = targetIndex % gridSize;

        // Define the positions of the surrounding objects (N, S, E, W)
        int[,] directions = new int[,]
        {
            { 0, -1 }, // North
            { 0, 1 },  // South
            { -1, 0 }, // West
            { 1, 0 }   // East
        };

        // Check each direction
        for (int i = 0; i < directions.GetLength(0); i++)
        {
            int newRow = targetRow + directions[i, 1];
            int newColumn = targetColumn + directions[i, 0];

            // Ensure the position is within the bounds of the 3x3 grid
            if (newRow >= 0 && newRow < gridSize && newColumn >= 0 && newColumn < gridSize)
            {
                int index = newRow * gridSize + newColumn;
                surroundingObjects.Add(_OrderedButtonList[index]);
            }
        }

        return surroundingObjects;
    }
}
