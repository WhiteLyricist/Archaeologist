using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text _bagText;
    [SerializeField] private TMP_Text _shovelText;

    [SerializeField] private Image _victory;
    [SerializeField] private Image _losing;

    [SerializeField] private GameObject _panel;

    public static Action Menu = delegate { };

    public static Action EndGame = delegate { };

    private int _numberGoldBar = 0;

    private int _numberShovel;

    void Start()
    {
        _bagText.text = _numberGoldBar.ToString();

        _shovelText.text = _numberShovel.ToString();

        GoldBarController.PlacedInBag += OnPlacedInBag;
        ItemController.Digging += OnDigging;
        ItemController.EndOfGame += OnEndOfGame;
        SceneController.SaveGame += OnSaveGame;
        SceneController.LoadGame += OnLoadGame;
    }

    void OnPlacedInBag() 
    {
        _numberGoldBar++;
        _bagText.text = _numberGoldBar.ToString();

        if (_numberGoldBar == Parameters.NumberGoldBar) 
        {
            _panel.SetActive(true);
            _victory.gameObject.SetActive(true);

            EndGame();

            Parameters.isNewGame = true;

            PlayerPrefs.DeleteKey("numberGoldBar");
            PlayerPrefs.Save();
        }
    }

    void OnEndOfGame() 
    {
        _panel.SetActive(true);
        _losing.gameObject.SetActive(true);

        EndGame();

        Parameters.isNewGame = true;

        PlayerPrefs.DeleteKey("numberGoldBar");
        PlayerPrefs.Save();
    }

    void OnDigging(int numberShovel) 
    {
        _shovelText.text = numberShovel.ToString();
    }

    void OnSaveGame() 
    {
        PlayerPrefs.SetInt("numberGoldBar", _numberGoldBar);
        PlayerPrefs.Save();
    }

    void OnLoadGame() 
    {
        if (Parameters.isNewGame) 
        {
            _numberGoldBar = 0;
            _bagText.text = _numberGoldBar.ToString();

            return;
        }
        _numberGoldBar = PlayerPrefs.GetInt("numberGoldBar", 0);
        _bagText.text = _numberGoldBar.ToString();
    }

    private void OnDestroy()
    {
        GoldBarController.PlacedInBag -= OnPlacedInBag;
        ItemController.Digging -= OnDigging;
        ItemController.EndOfGame -= OnEndOfGame;
        SceneController.SaveGame -= OnSaveGame;
        SceneController.LoadGame -= OnLoadGame;
    }

    public void ExitToTheMenu() 
    {
        PlayerPrefs.DeleteKey("numberGoldBar");
        PlayerPrefs.Save();

        Menu();
    }
}
