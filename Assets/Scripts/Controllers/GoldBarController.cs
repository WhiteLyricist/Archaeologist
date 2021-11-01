using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBarController : MonoBehaviour
{
    public static Action PlacedInBag = delegate { };


    private Vector2 _startPosition;

    private float _speed = 100f;

    private bool isMove = false;

    private RaycastHit2D _hit;

    private void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    Vector2 point = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

                    _hit = Physics2D.Raycast(point, Vector2.zero);

                    if (_hit && _hit.collider.tag == "GoldBar")
                    {
                        isMove = true;
                    }
                    break;
                case TouchPhase.Moved:
                    if (isMove)
                    {
                        _hit.transform.position = Vector2.Lerp(_hit.transform.position, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Time.deltaTime * _speed);
                    }
                    break;
                case TouchPhase.Ended:
                    if (isMove)
                    {
                        isMove = false;
                        _hit.transform.position = _startPosition;
                    }
                    break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMove = false;

        PlacedInBag();

        Destroy(gameObject);
    }
}
