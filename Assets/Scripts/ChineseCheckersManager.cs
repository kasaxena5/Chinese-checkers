using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChineseCheckersManager : MonoBehaviour
{
    [SerializeField]
    private ChineseChecker checkersPrefab;

    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private int boardSizeX = 20;
    [SerializeField]
    private int boardSizeY = 20;

    private ChineseChecker activeChecker = null;
    private Queue<ChineseChecker> potentialMoves = new Queue<ChineseChecker>();

    private ChineseChecker[,] checkerBoard;

    private void InstantiateCheckerBoard()
    {
        checkerBoard = new ChineseChecker[boardSizeX, boardSizeY];
        for(int i = 0; i < boardSizeX; i++)
        {
            for(int j = 0; j < boardSizeY; j++)
            {
                if ((i + j) % 2 == 0 && (i + j) >= 10 && (i + j) <= 28 && (i - j) >= -8 && (i - j) <= 10)
                {
                    checkerBoard[i, j] = Instantiate(checkersPrefab);
                    checkerBoard[i, j].i = i;
                    checkerBoard[i, j].j = j;
                    checkerBoard[i, j].transform.SetParent(canvas, false);
                    checkerBoard[i, j].GetComponent<RectTransform>().localPosition = new Vector3(20 * i - 200, 20 * j - 200, 0);
                    if(j >= 14)
                    {
                        checkerBoard[i, j].SetState(ChineseChecker.ChineseState.Red);
                    }
                    else if(j <= 4)
                    {
                        checkerBoard[i, j].SetState(ChineseChecker.ChineseState.Blue);
                    }
                    int x = i;
                    int y = j;
                    checkerBoard[i, j].GetComponent<Button>().onClick.AddListener(() => { OnClick(x, y); });
                }

            }
        }
    }

    private void DestroyCheckerBoard()
    {
        if (checkerBoard == null)
            return;

        for(int i = 0; i < boardSizeX; i++)
        {
            for(int j = 0; j < boardSizeY; j++)
            {
                if(checkerBoard[i,j] != null)
                {
                    Destroy(checkerBoard[i, j].gameObject);
                }
            }
        }

        activeChecker = null;
    }

    public void ResetCheckerBoard()
    {
        DestroyCheckerBoard();
        InstantiateCheckerBoard();
    }

    private void Awake()
    {
        InstantiateCheckerBoard();   
    }

    private bool IsEmpty(int i, int j)
    {
        if (checkerBoard[i, j] == null)
            return false;
        return (checkerBoard[i, j].GetState() == ChineseChecker.ChineseState.Blank);
    }

    private bool IsOccupied(int i, int j)
    {
        if (checkerBoard[i, j] == null)
            return false;
        return (checkerBoard[i, j].GetState() == ChineseChecker.ChineseState.Red || checkerBoard[i, j].GetState() == ChineseChecker.ChineseState.Blue);
    }

    private void MoveChecker(int i, int j)
    {
        checkerBoard[i, j].SetState(activeChecker.GetState());
        checkerBoard[activeChecker.i, activeChecker.j].SetState(ChineseChecker.ChineseState.Blank);
        activeChecker = null;
    }

    private void ClearPotentialMoves()
    {
        while(potentialMoves.Count != 0)
        {
            ChineseChecker c = potentialMoves.Dequeue();
            c.SetState(ChineseChecker.ChineseState.Blank);
        }
    }

    public void OnClick(int i, int j)
    {
        if(activeChecker == checkerBoard[i, j])
        {
            activeChecker = null;
            ClearPotentialMoves();
            return;
        }

        if (!IsOccupied(i, j) && !IsEmpty(i, j))
        {
            ClearPotentialMoves();
            MoveChecker(i, j);
            return;
        }

        if (IsOccupied(i, j))
        {
            activeChecker = checkerBoard[i, j];
            Queue<ChineseChecker> q = new Queue<ChineseChecker>();

            // Direct Moves
            int[] dx = {1, -1, 1, -1 };
            int[] dy = { 1, -1, -1, 1 };
            for(int k = 0; k < 4; k++)
            {
                int x = i + dx[k];
                int y = j + dy[k];
                if (x >= 0 && y >= 0 && x < boardSizeX && y < boardSizeY)
                {
                    if (IsEmpty(x, y))
                    {
                        potentialMoves.Enqueue(checkerBoard[x, y]);
                        checkerBoard[x, y].SetState(ChineseChecker.ChineseState.Potential);
                    }
                }
            }

            // Indirect Moves
            q.Enqueue(checkerBoard[i, j]);
            while(q.Count != 0)
            {
                ChineseChecker c =  q.Dequeue();
                for(int k = 0; k < 4; k++)
                {
                    int x = c.i + 2 * dx[k];
                    int y = c.j + 2 * dy[k];

                    int x_ = c.i + dx[k];
                    int y_ = c.j + dy[k];

                    if(IsOccupied(x_, y_) && IsEmpty(x, y))
                    {
                        q.Enqueue(checkerBoard[x, y]);
                        potentialMoves.Enqueue(checkerBoard[x, y]);
                        checkerBoard[x, y].SetState(ChineseChecker.ChineseState.Potential);

                    }
                    

                }
                
            }
        }
        

    }
    
}
