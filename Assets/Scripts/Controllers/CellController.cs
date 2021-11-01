using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellController : MonoBehaviour
{
    [SerializeField] private Sprite[] _depthImage;

    public static Action<bool, Vector2> Dig = delegate { };

    public int cellDepth;

    private const int _minCellDepth = 1;
    private const int _maxCellDepth = 3;

    private int _currentDepth = 0;
    public int CurrentDepth 
    {
        get => _currentDepth;
    }

    private bool isFound = false;

    private SpriteRenderer _depth;

    void Awake()
    {
        cellDepth = Parameters.ÑellDepth;
        cellDepth = Mathf.Clamp(cellDepth, _minCellDepth, _maxCellDepth);

        _depth = GetComponent<SpriteRenderer>();

        GoldBarController.PlacedInBag += OnPlacedInBag;
    }

    private void OnMouseDown()
    {
        if (cellDepth != _currentDepth && !isFound && !ItemController.ActiveDig && ItemController.ActiveFound)
        {
            float probability = UnityEngine.Random.Range(0f, 1f);

            if (probability < 0.3f)
            {
                isFound = true;
            }

            StartCoroutine(Digging());

            Dig(isFound, transform.position);
        }
    }

    private IEnumerator Digging() 
    {
        yield return new WaitForSeconds(0.5f);

        _currentDepth++;

        _depth.sprite = _depthImage[_currentDepth];
    }

    void OnPlacedInBag() 
    {
        if (cellDepth != _currentDepth) 
        {
            isFound = false;
        }
    }

    public void LoadDepth(int depth) 
    {
        _currentDepth = depth;
        _depth.sprite = _depthImage[_currentDepth];
    }

    private void OnDestroy()
    { 
        GoldBarController.PlacedInBag -= OnPlacedInBag;
    }
}
