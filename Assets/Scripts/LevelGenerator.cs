using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct LevelData 
{
    public int GridRows;
    public int GridColumns;
    public List<int> DepthData;
    public LevelData(int GridRows, int GridColumns, GameObject[,] Tiles) 
    {
        this.GridRows = GridRows;
        this.GridColumns = GridColumns;

        DepthData = new List<int>();

        foreach (GameObject Tile in Tiles) 
        {
            var tileDepth = Tile.GetComponent<CellController>().CurrentDepth;
            DepthData.Add(tileDepth); 
        }
    }

    public string Serialize() 
    {
        return JsonUtility.ToJson(this);
    }
}

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _background;

    public int gridRows;
    public int gridColumns;

    private const int _minGridRows = 1;
    private int _maxGridRows;

    private const int _minGridColumns =1;
    private int _maxGridColumns;

    private float _offsetX;
    private float _offsetY;

    private Vector2 _startPos; 

    private GameObject[,] _tiles;

    private void Start()
    {
        SceneController.SaveGame += OnSaveGame;
        SceneController.LoadGame += OnLoadGame;
        UIController.EndGame += OnEndGame;
    }

    void Generator(List<int> DepthData = null)
    {
        CleaningTheField();

        CalculationOfParameters();

        _tiles = new GameObject[gridColumns, gridRows];

        int count = 0;

        for (int i = 0; i < gridColumns; i++)
        {
            for (int j = 0; j < gridRows; j++)
            { 

                _tiles[i,j] = Instantiate(_groundPrefab) as GameObject;

                int currentDepth = (DepthData != null && DepthData.Count > count) ? DepthData[count] : 0;

                CellController cl = _tiles[i, j].GetComponent<CellController>();
                cl.LoadDepth(currentDepth);
                count++;

                float posX = (_offsetX * i) + _startPos.x;
                float posY = -(_offsetY * j) + _startPos.y;

                _tiles[i, j].transform.position = new Vector2(posX, posY);
            }
        }
    }

    void CalculationOfParameters() 
    {
        _offsetX = _groundPrefab.transform.localScale.x; //Расстояние между ячейками по высоте.
        _offsetY = _groundPrefab.transform.localScale.y; //Расстояние между ячейками по ширине.

        _maxGridRows = Mathf.FloorToInt(_background.transform.localScale.y / _offsetY - 1); //Максимальное кол-во строчек в зависимости от размера поля.
        _maxGridColumns = Mathf.FloorToInt(_background.transform.localScale.x / _offsetX); //Максимальное кол-во столбцов в зависимости от размера поля.

        gridRows = Parameters.GridRows; //Параметры берущиеся из настроек, можно отключить если настроек нет.
        gridColumns = Parameters.GridColumns; //Параметры берущиеся из настроек, можно отключить если настроек нет.

        gridRows = Mathf.Clamp(gridRows, _minGridRows, _maxGridRows); //Ограничения для кол-ва строк между минимальным и максимальным значением.
        gridColumns = Mathf.Clamp(gridColumns, _minGridColumns, _maxGridColumns); //Ограничения для кол-ва столбцов между минимальным и максимальным значением.

        _startPos = new Vector2((_offsetX - gridColumns) / 2, (gridRows - _offsetY) / 2); //Стартовая позиция в верхнем левом углу для симметрии относительно центра. 
    }

    [Button("Cleaning")]
    void CleaningTheField() //Очищение поля от всех ранее созданых ячеек.
    {
        if (_tiles == null)
        {
            return;
        }

        foreach (GameObject _tile in _tiles)
        {
            DestroyImmediate(_tile);
        }
    }

    void OnSaveGame() 
    {
        var levelData = new LevelData(gridRows, gridColumns, _tiles);
        PlayerPrefs.SetString("levelData", levelData.Serialize());
        PlayerPrefs.Save();
    }

    void OnLoadGame()
    {
        var ld = PlayerPrefs.GetString("levelData", "");
        if (!string.IsNullOrEmpty(ld) && !Parameters.isNewGame)
        {
            var levelData = JsonUtility.FromJson<LevelData>(ld);

            gridRows = levelData.GridRows;
            gridColumns = levelData.GridColumns;

            Generator(levelData.DepthData);
        }
        else
        {
            OnEndGame();
            Generator();
        }
    }

    void OnEndGame() 
    {
        PlayerPrefs.DeleteKey("levelData");
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        SceneController.SaveGame -= OnSaveGame;
        SceneController.LoadGame -= OnLoadGame;
        UIController.EndGame -= OnEndGame;
    }
}
