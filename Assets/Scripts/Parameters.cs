using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Parameters 
{
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
}
