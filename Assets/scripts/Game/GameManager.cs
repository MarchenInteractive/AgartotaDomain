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

    public int player;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(SlowBoardGenerator());
        // BoardGenerator();
    }

    public void Cambiarplayer()
    {
        if (player == 0)
        {
            player = 1;
        }
        else if (player == 1)
        {
            player = 0;
        }

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
                temp.GetComponent<Cell>().row = horizontalDimension;
                temp.name = "Cell-" + i + "-" + j;
                // openDomain.Add(temp.name, temp.GetComponent<Cell>());
                yield return new WaitForSeconds(0.2f);
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
            }
        }
    }

    public GameObject insertNewPiece(int col, int row, int player, Vector3 position)
    {
        GameObject piece = GameObject.Instantiate(piecePrefab, position, Quaternion.identity);
        piece.GetComponent<Piece>().level = 1;
        piece.GetComponent<Piece>().col = col;
        piece.GetComponent<Piece>().row = row;
        piece.name = "Piece player" + player;
        return piece;
    }

    public int Battle(int attacker, int defender)
    {
        int luck = Random.Range(0, 10);

        if (defender < attacker)
        {
            return 0;
        }
        else if (defender > attacker)
        {
            return 1;
        }
        else if (defender == attacker)
        {
            if (luck <= 3)
            {
                return 2;
            }
            else if (luck <= 7)
            {
                return 0;
            }
            else if (luck == 8)
            {
                return 3;
            }
            else if (luck == 9)
            {
                return 4;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }


}
