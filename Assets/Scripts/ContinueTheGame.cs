using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueTheGame : MonoBehaviour
{
    [SerializeField] private Button _button;
    void Start()
    {
        if (Parameters.isNewGame) 
        {
            _button.interactable = false;
        }
    }
}
