using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private Grid gridPrefab;
    [SerializeField] private int width, height;
    [SerializeField] private Camera maincam;
    private List<Vector2> enemyGridsPositions = new List<Vector2>();
    private int countlimit = 20;
    private int counter = 0;
    public Dictionary<Vector2, Grid> GridsDict = new Dictionary<Vector2, Grid>();

    private void Awake()
    {
        Instance = this;
        enemyGridsPositions.Add(new Vector2(5, 0));
        enemyGridsPositions.Add(new Vector2(5, 1));
        enemyGridsPositions.Add(new Vector2(5, 2));
        enemyGridsPositions.Add(new Vector2(5, 3));
        enemyGridsPositions.Add(new Vector2(4, 0));
        enemyGridsPositions.Add(new Vector2(4, 3));
    }

    // Start is called before the first frame update
    void Start()
    {
        maincam.transform.position = new Vector3(width / 2, height / 2, -10);
    }

    /// <summary>
    /// Function to generate the gameboard, which consists of grids
    /// </summary>
    public void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Grid thisGrid = Instantiate(gridPrefab, new Vector2(x, y), Quaternion.identity);
                bool isOffset = (x + y) % 2 == 1;
                thisGrid.Init(isOffset);

                GridsDict.Add(new Vector2(x, y), thisGrid);
            }
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SetupEnemies);
    }

    /// <summary>
    /// Function that randomly picks an unoccupied enemy Grid.
    /// </summary>
    /// <returns>Grid object. Use grid.transform.position to get position.</returns>
    public Grid GetEnemySpawnGrid()
    {
        Vector2 enemyGridPosition = enemyGridsPositions[Random.Range(0, enemyGridsPositions.Count)];
        //Debug.Log(enemyGridPosition);
        while (GridsDict[enemyGridPosition].unitOnGrid != null && counter < countlimit)
        {
            enemyGridPosition = enemyGridsPositions[Random.Range(0, enemyGridsPositions.Count)];
            Debug.Log(enemyGridPosition);
            counter++; //prevents infinite loop
        }
        counter = 0; //not sure if required
        return GridsDict[enemyGridPosition];
    }
}
