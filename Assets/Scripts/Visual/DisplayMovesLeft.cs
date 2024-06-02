using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayMovesLeft : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _Text;

    private void Update()
    {
       _Text.text =  ButtonManager.Instance._MovesLeft.ToString();
    }
}
