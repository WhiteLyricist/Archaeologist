using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _gridRows;
    [SerializeField] private TMP_InputField _gridColumns;
    [SerializeField] private TMP_InputField _numberShovel;
    [SerializeField] private TMP_InputField _numberGoldBar;
    [SerializeField] private TMP_InputField _�ellDepth;

    // Start is called before the first frame update
    void Start()
    {
        _gridRows.text = Parameters.GridRows.ToString();
        _gridColumns.text = Parameters.GridColumns.ToString();
        _numberShovel.text = Parameters.NumberShovel.ToString();
        _numberGoldBar.text = Parameters.NumberGoldBar.ToString();
        _�ellDepth.text = Parameters.�ellDepth.ToString();
    }

    public void SetParameters() 
    {
        Parameters.SetParameters(int.Parse(_�ellDepth.text), int.Parse(_gridRows.text), int.Parse(_gridColumns.text), int.Parse(_numberShovel.text), int.Parse(_numberGoldBar.text));
    }
}
