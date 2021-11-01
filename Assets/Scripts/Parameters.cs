using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParametersData 
{
    public int �ellDepth;
    public int GridRows;
    public int GridColumns;
    public int NumberShovel;
    public int NumberGoldBar;
    public bool IsNewGame;
    public ParametersData(int �ellDepth, int GridRows, int GridColumns, int NumberShovel, int NumberGoldBar, bool IsNewGame) 
    {
        this.�ellDepth = �ellDepth;
        this.GridRows = GridRows;
        this.GridColumns = GridColumns;
        this.NumberShovel = NumberShovel;
        this.NumberGoldBar = NumberGoldBar;
        this.IsNewGame = IsNewGame;
    }

    public string Serialize()
    {
        return JsonUtility.ToJson(this);
    }
}

public static class Parameters 
{
    static Parameters() 
    {
        SceneController.SaveGame = OnSaveGame;

        if (PlayerPrefs.HasKey("parameters")) 
        {
            var par = JsonUtility.FromJson<ParametersData>(PlayerPrefs.GetString("parameters"));
            _�ellDepth = par.�ellDepth;
            _gridColumns = par.GridColumns;
            _gridRows = par.GridRows;
            _numberShovel = par.NumberShovel;
            _numberGoldBar = par.NumberGoldBar;
            isNewGame = par.IsNewGame;
        }
    }

    private static int _�ellDepth = 3;
    public static int �ellDepth
    {
        get => _�ellDepth;
    }

    private static int _gridRows = 9;
    public static int GridRows
    {
        get => _gridRows;
    }

    private static int _gridColumns = 5;
    public static int GridColumns
    {
        get => _gridColumns;
    }

    private static int _numberShovel = 10;
    public static int NumberShovel 
    {
        get => _numberShovel;
    }

    private static int _numberGoldBar = 3;
    public static int NumberGoldBar 
    {
        get => _numberGoldBar;
    }

    public static bool isNewGame = true;

    public static void SetParameters(int �ellDepth, int gridRows, int gridColumns, int numberShovel, int numberGoldBar) 
    {
        _�ellDepth = �ellDepth;
        _gridRows = gridRows;
        _gridColumns = gridColumns;
        _numberShovel = numberShovel;
        _numberGoldBar = numberGoldBar;
    }

    static void OnSaveGame() 
    {
        var parametersData = new ParametersData(_�ellDepth, _gridRows, _gridColumns, _numberShovel, _numberGoldBar, isNewGame);
        PlayerPrefs.SetString("parameters", parametersData.Serialize());
        PlayerPrefs.Save();
    }
}
