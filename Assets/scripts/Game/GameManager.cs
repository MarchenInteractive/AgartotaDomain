using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject cellPrefab;
    public GameObject piecePrefab;
    public int horizontalDimension = 6;
    public int verticalDimension = 5;
    public Dictionary<string, Cell> PlayerADomain;
    public Dictionary<string, Cell> PlayerBDomain;
    public Dictionary<string, Cell> openDomain;
    public GameObject[] prefPirate;
    public GameObject[] prefMandrake;
    public int player;
    public bool move;
    public GameObject currentPiece;
    public Cell currentCell;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SlowBoardGenerator());
        // BoardGenerator();
    }



    private IEnumerator SlowBoardGenerator()
    {
        GameObject temp;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < horizontalDimension; i++)
        {
            for (int j = 0; j < verticalDimension; j++)
            {
                temp = GameObject.Instantiate(cellPrefab, new Vector3(i, 0f, j), Quaternion.identity);
                if (i < horizontalDimension / 2)
                {
                    temp.GetComponent<Cell>().player = 0;
                }
                else if (i >= horizontalDimension / 2)
                {
                    temp.GetComponent<Cell>().player = 1;
                }

                temp.GetComponent<Cell>().col = i;
                temp.GetComponent<Cell>().row = j;
                temp.name = i + "-" + j;
                // openDomain.Add(temp.name, temp.GetComponent<Cell>());
                yield return new WaitForSeconds(0.02f);
            }

        }
    }


    public void BoardGenerator()
    {
        GameObject temp;
        for (int i = 0; i < horizontalDimension; i++)
        {
            for (int j = 0; j < verticalDimension; j++)
            {
                temp = GameObject.Instantiate(cellPrefab, new Vector3(i, 0f, j), Quaternion.identity);
                if (i < horizontalDimension / 2)
                {
                    temp.GetComponent<Cell>().player = 0;
                }
                else if (i >= horizontalDimension / 2)
                {
                    temp.GetComponent<Cell>().player = 1;
                }
                temp.GetComponent<Cell>().col = i;
                temp.GetComponent<Cell>().row = j;

                temp.name = "Cell-" + i + "-" + j;
                temp.tag = i + "-" + j;
            }
        }
    }

    public GameObject InsertNewPiece(int col, int row, int player, Vector3 position, int lvl)
    {
        GameObject piece;
        if (player == 0)
        {
            piece = Instantiate(prefPirate[lvl - 1], position, Quaternion.identity) as GameObject;
        }
        else
        {
            piece = Instantiate(prefMandrake[lvl - 1], position, Quaternion.identity) as GameObject;
        }
        piece.GetComponent<Piece>().level = lvl;
        piece.GetComponent<Piece>().col = col;
        piece.GetComponent<Piece>().row = row;
        piece.GetComponent<Piece>().owner = player;
        piece.name = "Piece player" + player;
        TurnChange();
        return piece;
    }

    public void TurnChange()
    {
        if (player == 0)
            player = 1;
        else
            player = 0;


    }

    public void CleanBoard()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
        {
            Cell cell = item.GetComponent<Cell>();
            if (player == 0)
            {
                cell.render.material = cell.nightMaterial;

            }
            else if (player == 1)
            {
                cell.render.material = cell.dayMaterial;

            }
        }

    }


}
