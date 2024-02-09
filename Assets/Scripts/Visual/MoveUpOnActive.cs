using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveInDirection
{
    MoveDown,
    MoveUp,
    MoveLeft,
    MoveRight,
}

public class MoveUpOnActive : MonoBehaviour
{

    private Vector2 _StartingPos;
    private Camera _MainCamera;
    private RectTransform _RT;
    //public RectTransform _OutsidePosition;
    public float _Speed;


    private void Awake()
    {
        _RT = this.GetComponent<RectTransform>();
        _StartingPos = _RT.anchoredPosition;
        _MainCamera = Camera.main;
    }

    private void Start()
    {
        MoveDown();
    }
    private void FixedUpdate()
    {
        LerpToPoint();
    }

    private void MoveDown()
    {
        Vector3 bottomOfCameraViewport = new Vector3(0f, 0f, _MainCamera.nearClipPlane);
        Vector3 bottomWorldPosition = _MainCamera.ViewportToWorldPoint(bottomOfCameraViewport);
        transform.position = new Vector3(transform.position.x, bottomWorldPosition.y - 100, transform.position.z);
    }

    private void LerpToPoint()
    {
        float step = _Speed * Time.deltaTime;
        _RT.anchoredPosition = Vector2.Lerp(_RT.anchoredPosition, _StartingPos, step);
    }


}
