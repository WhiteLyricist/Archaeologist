using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemData 
{
    public int NumberShovel;
    public int NumberGoldBar;
    public bool isctiveDig;
    public bool isActiveFound;
    public List<Vector2> GoldBarPositions;

    public ItemData(int NumberShovel, int NumberGoldBar, bool isctiveDig, bool isActiveFound) 
    {
        this.NumberShovel = NumberShovel;
        this.NumberGoldBar = NumberGoldBar;
        this.isctiveDig = isctiveDig;
        this.isActiveFound = isActiveFound;

        GoldBarPositions = new List<Vector2>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("GoldBar")) 
        {
            GoldBarPositions.Add(new Vector2(go.transform.position.x, go.transform.position.y));
        }
    }

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }
}

public class ItemController : MonoBehaviour
{
    [SerializeField] private GameObject _shovelPrefab;
    [SerializeField] private GameObject _goldBarPrefab;

    public static Action EndOfGame = delegate { };
    public static Action<int> Digging = delegate { };

    private GameObject _shovel;
    private GameObject _goldBar;

    private int _numberShovel;
    private int _numberGoldBar;

    private const int _minNumberShovel = 1;
    private int _maxNumberShovel;

    private static bool isActiveDig = false;
    public static bool ActiveDig
    {
        get => isActiveDig;
    }

    private static bool isActiveFound = true;
    public static bool ActiveFound
    {
        get => isActiveFound;
    }

    void Start()
    {
        _numberGoldBar = Parameters.NumberGoldBar;
        _numberShovel = Parameters.NumberShovel;

        _maxNumberShovel = Parameters.GridColumns * Parameters.GridRows;

        _numberShovel = Mathf.Clamp(_numberShovel, _minNumberShovel, _maxNumberShovel);

        CellController.Dig += OnDig;
        SceneController.SaveGame += OnSaveGame;
        SceneController.LoadGame += OnLoadGame;
        UIController.EndGame += OnEndGame;
    }

    void OnDig(bool found, Vector2 position) 
    {
        _numberShovel--;

        StartCoroutine(Dig(position));

        if (found)
        { 
            if (_numberGoldBar >= 1)
            {
                StartCoroutine(Found(position));
                _numberGoldBar--;
            }
            if (_numberGoldBar == 0)
            {
                Debug.Log("Нашли все");
                isActiveFound = false;
            }
        }
    }

    private IEnumerator Found(Vector2 position)
    {
        yield return new WaitForSeconds(0.5f);

        _goldBar = Instantiate(_goldBarPrefab) as GameObject;
        _goldBar.transform.position = position;
        _goldBar.SetActive(true);
    }

    private IEnumerator Dig(Vector2 position) 
    {
        isActiveDig = true;

        Digging(_numberShovel);

        _shovel = Instantiate(_shovelPrefab) as GameObject; 
        _shovel.transform.position = position;
        _shovel.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        Destroy(_shovel);

        if (_numberShovel == 0)
        {
            if (_numberGoldBar == 0)
            {
                isActiveDig = true;
                isActiveFound = false;
            }
            else
            {
                isActiveDig = true;
                EndOfGame();
            }
        }
        else isActiveDig = false;
    }

    void OnSaveGame() 
    {
        var itemlData = new ItemData(_numberShovel, _numberGoldBar, isActiveDig, isActiveFound);
        PlayerPrefs.SetString("itemData", itemlData.Serialize());
        PlayerPrefs.Save();
    }

    void OnLoadGame() 
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("GoldBar"))
        {
            Destroy(go);
        }

        var ld = PlayerPrefs.GetString("itemData", "");

        if (!string.IsNullOrEmpty(ld))
        {
            var itemData = JsonUtility.FromJson<ItemData>(ld);

            _numberGoldBar = itemData.NumberGoldBar;

            _numberShovel = itemData.NumberShovel;
            _numberShovel = Mathf.Clamp(_numberShovel, _minNumberShovel, _maxNumberShovel);

            isActiveDig = itemData.isctiveDig;
            isActiveFound = itemData.isActiveFound;

            Digging(_numberShovel);

            foreach (Vector2 gbPosition in itemData.GoldBarPositions)
            {
                _goldBar = Instantiate(_goldBarPrefab) as GameObject;
                _goldBar.transform.position = gbPosition;
                _goldBar.SetActive(true);
            }
        }
    }

    void OnEndGame() 
    {
        PlayerPrefs.DeleteKey("itemData");
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        CellController.Dig -= OnDig;
        SceneController.SaveGame -= OnSaveGame;
        SceneController.LoadGame -= OnLoadGame;
        UIController.EndGame -= OnEndGame;
    }

}
