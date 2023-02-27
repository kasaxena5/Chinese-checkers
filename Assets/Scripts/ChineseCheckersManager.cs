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
    private int boardSizeX;
    [SerializeField]
    private int boardSizeY;

    private ChineseChecker[] checkers;

    private ChineseChecker[,] checkerBoard;

    private void InstantiateCheckerBoard()
    {
        checkerBoard = new ChineseChecker[boardSizeX, boardSizeY];
        for(int i = 0; i < boardSizeX; i++)
        {
            for(int j = 0; j < boardSizeY; j++)
            {
                if ((i + j) % 2 == 0)
                {
                    checkerBoard[i, j] = Instantiate(checkersPrefab);
                    checkerBoard[i, j].transform.SetParent(canvas, false);
                    checkerBoard[i, j].GetComponent<RectTransform>().localPosition = new Vector3(40 * i, 40 * j, 0);
                }

            }
        }
    }

    public void DestroyCheckerBoard()
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
    }

    private void Awake()
    {
        InstantiateCheckerBoard();   
    }

    void Start()
    {
        //checkers = new ChineseChecker[2];
        //checkers[0] = Instantiate(checkersPrefab);
        //checkers[0].transform.SetParent(canvas, false);
        //checkers[0].GetComponent<RectTransform>().localPosition = new Vector3(10, 10, 0);
        //checkers[0].GetComponent<RectTransform>().localPosition = new Vector3(0, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
