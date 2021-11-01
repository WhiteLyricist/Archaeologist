using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBarController : MonoBehaviour
{
    public static Action PlacedInBag = delegate { };


    private Vector2 _startPosition;

    private float _speed = 100f;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void OnMouseDrag()
    {
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * _speed);
    }

    private void OnMouseUp()
    {
        transform.position = _startPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlacedInBag();

        Destroy(gameObject);
    }
}
