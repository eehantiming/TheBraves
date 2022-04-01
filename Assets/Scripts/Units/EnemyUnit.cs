using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    public bool isStunned = false;
    public bool isAlive = true;

    protected int rageLevel = 0;
    protected bool isBaited = false;
    protected MapGrid baitedTo;
    //TODO: create line renderer as part of script instead?
    protected MapGrid nearestHero;

    // Start is called before the first frame update
    void Start()
    {

    }
    protected virtual IEnumerator ActivateRage() 
    {
        yield break;
    }

    /// <summary>
    /// Function to increase Rage level by 1. If you do, activates associated ability
    /// </summary>
    public virtual IEnumerator IncreaseRageLevel()
    {
        if(rageLevel < 2)
        {
            rageLevel++; // TODO: add visualization
            yield return StartCoroutine(ActivateRage());
        }
    }
    
    /// <summary>
    /// Function to move from current grid towards target grid, which may have 1 or 2 possible paths.
    /// </summary>
    /// <param name="targetGrid">MapGrid to move towards</param>
    protected IEnumerator MoveTowardsGrid(MapGrid targetGrid)
    {
        int goalGridIndex;
        var directionToMove = targetGrid.IndexToVect() - currentGrid.IndexToVect();
        if (directionToMove.x == 0) // move vertical
        {
            goalGridIndex = currentGrid.index + 4 * System.Math.Sign(directionToMove.y);
        }
        else if (directionToMove.y == 0) // move horizontal
        {
            goalGridIndex = currentGrid.index + System.Math.Sign(directionToMove.x);
        }
        else // two possible moves, roll dice to decide
        {
            // TODO: add visualization for which dice roll values correspond to each move
            List<int> possibleGridIndex = new List<int>()
                {
                    currentGrid.index + 4 * System.Math.Sign(directionToMove.y),
                    currentGrid.index + System.Math.Sign(directionToMove.x)
                };
            int roll = DiceRoll.Instance.GenerateRoll();
            goalGridIndex = possibleGridIndex[(roll - 1) / 3];
        }
        MapGrid goalGrid = GridManager.Instance.IndexToGrid[goalGridIndex];
        yield return StartCoroutine(MoveTo(goalGrid));
    }

    /// <summary>
    /// Function to move from current grid towards baitedTo, which may have 1 or 2 possible paths. Note: does not remove the bait status.
    /// </summary>
    protected IEnumerator MoveTowardsBait()
    {
        yield return StartCoroutine(MoveTowardsGrid(baitedTo));
    }

    /// <summary>
    /// Function to randomly select valid west, east or south grid to move to.
    /// </summary>
    /// <param name="adjacentGrids">List of MapGrids which are valid movement based on this unit's movement rules</param>
    protected IEnumerator RandomMovement(List<MapGrid> adjacentGrids)
    {
        MapGrid goalGrid;
        int roll;
        switch (adjacentGrids.Count)
        {
            case 0:
                UIManager.Instance.ShowGameMessageText($"{unitName} can't Move");
                Debug.Log($"{unitName} can't move");
                break;
            case 1:
                UIManager.Instance.ShowGameMessageText($"{unitName} has only 1 Move");
                goalGrid = adjacentGrids[0];
                yield return StartCoroutine(MoveTo(goalGrid));
                break;
            case 2:
                Debug.Log("2 moves, rolling dice");
                roll = DiceRoll.Instance.GenerateRoll();
                goalGrid = adjacentGrids[(roll - 1) / 3];
                yield return StartCoroutine(MoveTo(goalGrid));
                break;
            case 3:
                Debug.Log("3 moves, rolling dice");
                roll = DiceRoll.Instance.GenerateRoll();
                goalGrid = adjacentGrids[(roll - 1) / 2];
                yield return StartCoroutine(MoveTo(goalGrid));
                break;
            default:
                Debug.LogError("Invalid movement outcome");
                break;
        }
    }

    /// <summary>
    /// Function for Enemy to move this turn. Throws dice if neccesary.
    /// </summary>
    /// <param name="keepBait">true if monster does not lose debuff after this movement</param>
    public virtual IEnumerator DecideMovement(bool keepBait) // Make this abstract?
    {
        yield break;
    }



    /// <summary>
    /// Function to make enemy baited and move towards bait source on its next turn.
    /// </summary>
    /// <param name="baitSource">The MapGrid that the baited enemy will move towards.</param>
    public void TakeBait(MapGrid baitSource)
    {
        isBaited = true;
        baitedTo = baitSource;

        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, currentGrid.transform.position);
        lr.SetPosition(1, baitSource.transform.position);

        GetComponent<SpriteRenderer>().color = new Color(0.7294118f, 0.4941176f, 0.7490196f);
        UIManager.Instance.ShowGameMessageText($"{unitName} is baited!");
        Debug.Log($"{unitName} is baited to {baitedTo.IndexToVect()}");
    }

    public void LoseBait() // TODO: make this protected?
    {
        isBaited = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
    }

    protected MapGrid FindNearestHero()
    {
        DistanceMapGridComparer compareDistance = new DistanceMapGridComparer();
        //pending code to find a mapgrid containing nearest hero.
        List<MapGrid> allGrids = GridManager.Instance.IndexToGrid.Values.ToList();
        List<MapGrid> heroGrids = allGrids.FindAll(grid => grid.heroesOnGrid.Count > 0 );
        //need to sort heroGrids by distant to active unit current grid
        heroGrids.Sort(compareDistance);
        Debug.Log(heroGrids[0].IndexToVect());
        return heroGrids[0];
        
    }

    protected MapGrid FindNearestSpawnPoint()
    {
        DistanceMapGridComparer compareDistance = new DistanceMapGridComparer();
        //pending code to find a mapgrid containing nearest spawnPoint.
        List<MapGrid> allGrids = GridManager.Instance.IndexToGrid.Values.ToList();
        List<MapGrid> spawnGrids = allGrids.FindAll(grid => grid.isEnemySpawnGrid);
        //need to sort spawnGrids by distant to active unit current grid
        spawnGrids.Sort(compareDistance);
        Debug.Log(spawnGrids[0].IndexToVect());
        return spawnGrids[0];
        
    }



    public class DistanceMapGridComparer : IComparer<MapGrid>
    {
        public int Compare(MapGrid grid1, MapGrid grid2)
        {
            if(Vector2Int.Distance(grid1.IndexToVect(), UnitManager.Instance.activeUnit.currentGrid.IndexToVect()) > Vector2Int.Distance(grid2.IndexToVect(), UnitManager.Instance.activeUnit.currentGrid.IndexToVect()))
                return 1;
            if(Vector2Int.Distance(grid1.IndexToVect(), UnitManager.Instance.activeUnit.currentGrid.IndexToVect()) < Vector2Int.Distance(grid2.IndexToVect(), UnitManager.Instance.activeUnit.currentGrid.IndexToVect()))
                return -1;
            else
                return 0;
        }

    }

    public void GetStunned()
    {
        isStunned = true; //TODO: add animation to indicate trapped 
        GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f);
        UIManager.Instance.ShowGameMessageText($"{unitName} is stunned by the trap!");
        Debug.Log($"{unitName} stunned by trap on {currentGrid.IndexToVect()}");
    }

    public void LoseStun()
    {
        isStunned = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        Debug.Log($"{unitName} is un-stunned");
    }

}
