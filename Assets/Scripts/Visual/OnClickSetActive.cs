using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickSetActive : MonoBehaviour
{
    public GameObject _GameObject;

    public void SetActive()
    {
        _GameObject.SetActive(true);
    }

    public void SetInactive()
    {
        _GameObject.SetActive(false);
    }
}
