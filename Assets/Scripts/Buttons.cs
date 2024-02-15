using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    NormalItem,
    GoldenItem,
    TreasureItem,
    MotionItem,
}

public class Buttons : MonoBehaviour
{
    //flag if the item is the Golden Button 
    //public bool _GoldenItem = false; //Change code to enum for ItemType
    public ItemType _ItemType = ItemType.NormalItem;

    public int _Index = 0;
    public int _CorrectPosition = 0;
    public bool _Pressed = false;
    public bool _Interactable = true;
    public Button btn;
    public Container _Container;
    public Image _Image;
    public GameObject _GoldenImage;

    private RectTransform _rect;
    private Quaternion _StartingRotation;
    private bool AlreadyChecked = false;

    //Dance
    private float _originalYPosition;
    private bool _Dance = true;
    public float _amplitude = 20f; // Adjust this value to control the amplitude of the dance
    public float _frequency = 7f; // Adjust this value to control the frequency of the dance


    private void Update()
    {
        if(_Dance)
            Dance();
    }

    private void Awake()
    {
        _Index = ButtonManager.Instance.GetBtnIndex();
        ButtonManager.Instance.AddToRows(this);
        btn = GetComponent<Button>();
        _Image = GetComponent<Image>();
        _rect = GetComponent<RectTransform>();
        CheckItem(); //Checks the item type to give this item
    }

    public RectTransform GetRectTransform()
    {
        return _rect;
    }

    //Checks the item type to give this item
    private void CheckItem()
    {
        //Items sorted with presidence, the lower the item the higher the priority

        

        if (GameManager.Instance.SpawnGoldenItem())
        {
            SetGoldenItem();
        }
    }

    private bool _CheckedAlready = false;
    public void CheckCorrect()
    {
        if(_CorrectPosition == _Container._Index)
        {
            _Container.SetCorrect();
            if (!AlreadyChecked)
            {
                switch (_ItemType)
                {
                    case ItemType.GoldenItem:
                        if (!_CheckedAlready)
                        {
                            _CheckedAlready = true;
                            ButtonManager.Instance.ResetMoves();
                        }
                        break;
                    case ItemType.TreasureItem:
                        ButtonManager.Instance._GameRewardsScreen.SetActive(true);
                        break;
                }
                AlreadyChecked = true;
            }
        }
        else
        {
            _Container.UnSetCorrect();
        }
    }

    private void Start()
    {
       _Container =  ButtonManager.Instance.SetContainer();
        this.transform.position = _Container.transform.position;
        ResetAnchor();
        _amplitude = Random.Range(1, 3.5f);
        _frequency = Random.Range(3, 10);
        _StartingRotation = this.transform.rotation;
        switch (_ItemType)
        {
            case ItemType.GoldenItem:
                _GoldenImage.SetActive(true);
                break;
            case ItemType.TreasureItem:
                break;
        }
            
    }

    public void ResetRotation()
    {
        this.transform.rotation = _StartingRotation;
    }

    public void ResetAnchor()
    {
        _originalYPosition = _rect.anchoredPosition.y;
        ResetRotation();

    }

    public Sprite GetSprite()
    {
        return _Image.sprite;
    }

    public void SetSprite(Sprite s)
    {
        _Image.sprite = s;
    }

    public void SetInteractable(bool value)
    {
        _Interactable = value;
        btn.interactable = value;
        
    }

    public void SetGoldenItem()
    {
        _ItemType = ItemType.GoldenItem;
        _Image.color = Color.yellow;
    }

    public void SetTreasureItem()
    {
        _ItemType = ItemType.TreasureItem;
        SetSprite(RewardsManager.Instance._TreasureSprite);
        _Image.color = Color.blue;
        _GoldenImage.SetActive(true);
    }

    public void CreateButtons()
    {
        if (_Interactable)
        {

            _Pressed = true;
            ButtonManager.Instance.SelectButtons(this);
        }
    }

    private float _OldFreq;

    private float spinSpeed = 1440f;
    private bool isRotating = false;
    public void SetSelected()
    {
        _OldFreq = _frequency;
        _frequency =  13f;
        if(!isRotating)
            StartCoroutine(SpinCoroutine());
    }

    public void SetUnSelected()
    {
        _frequency = _OldFreq;
    }

    IEnumerator SpinCoroutine()
    {
        isRotating = true;


        float duration = 0.25f;
        float elapsedTime = 0f;
        Quaternion initialRotation = _rect.rotation;

        while (elapsedTime < duration)
        {
            _rect.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            if (Quaternion.Angle(_rect.rotation, initialRotation) < 0.1f)
                break;

            yield return null;
        }

        isRotating = false;
    }

    public void Zoom(float ScaleFactor)
    {
        _rect.localScale = new Vector3(ScaleFactor, ScaleFactor, 1f);
    }

    public void SetDance(bool value)
    {
        _Dance = value;
    }
    
    private void Dance()
    {
        float offsetY = _amplitude * Mathf.Sin(Time.time * _frequency);

        _rect.anchoredPosition = new Vector2(_rect.anchoredPosition.x, _originalYPosition + offsetY);
    }
}
