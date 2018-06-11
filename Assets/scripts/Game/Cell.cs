using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int player = 0;
    public int col;
    public int row;
    public Renderer render;
    public Material dayMaterial;
    public Material nightMaterial;
    public Material highlightMaterial;
    public Material posibleSelectMaterial;
    public GameObject piece;
    public bool isSelectedPiece;
    public bool posibleMovement;
    float x;
    float z;

    private void Start()
    {
        x = transform.position.x + 0.1f;
        z = this.transform.position.z - 0.2f;
    }

    void OnMouseDown()
    {
        if (piece == null)
        {
            OnEmptyCell();
        }
        else if (piece.GetComponent<Piece>().owner == GameManager.instance.player)
        {
            OnCellWithOwnPiece();
        }
        else if (GameManager.instance.currentPiece != null && piece.GetComponent<Piece>().owner != GameManager.instance.player)
        {
            OnCellWithEnemyPiece();
            Debug.Log("Attack");
        }
    }

    void OnMouseOver()
    {
        if (
            GameManager.instance != null &&
            !GameManager.instance.move
            )
        {
            if (GameManager.instance.player == player)
            {
                if (piece != null)
                {
                    if (piece.GetComponent<Piece>().owner == GameManager.instance.player)
                    {
                        render.material = highlightMaterial;
                    }
                }
                else
                {
                    render.material = highlightMaterial;
                }
            }
            else if (piece != null && piece.GetComponent<Piece>().owner == GameManager.instance.player)
            {
                render.material = highlightMaterial;
            }

        }
    }

    void OnMouseExit()
    {
        if (!GameManager.instance.move && !posibleMovement)
        {
            GameManager.instance.CleanBoard();
        }
    }


    public void PieceIsSelected(bool select)
    {
        Debug.Log(select);
        int nextCol = col + 1;
        int nextRow = row + 1;
        int backCol = col - 1;
        int backRow = row - 1;
        GameManager.instance.move = select;
        GameObject[] cellsObj = GameObject.FindGameObjectsWithTag("cell");
        isSelectedPiece = select;
        if (select)
        {
            GameManager.instance.currentPiece = piece;
            GameManager.instance.currentCell = this;
            if (nextCol < GameManager.instance.horizontalDimension) GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().render.material = posibleSelectMaterial;
            if (nextRow < GameManager.instance.verticalDimension) GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().render.material = posibleSelectMaterial;
            if (backCol >= 0) GameObject.Find(backCol + "-" + row).GetComponent<Cell>().render.material = posibleSelectMaterial;
            if (backRow >= 0) GameObject.Find(col + "-" + backRow).GetComponent<Cell>().render.material = posibleSelectMaterial;
        }
        else
        {
            GameManager.instance.currentPiece = null;
            GameManager.instance.currentCell = null;
            if (nextCol < GameManager.instance.horizontalDimension) GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().render.material = nightMaterial;
            if (nextRow < GameManager.instance.verticalDimension) GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().render.material = nightMaterial;
            if (backCol >= 0) GameObject.Find(backCol + "-" + row).GetComponent<Cell>().render.material = nightMaterial;
            if (backRow >= 0) GameObject.Find(col + "-" + backRow).GetComponent<Cell>().render.material = nightMaterial;
        }
        if (nextCol < GameManager.instance.horizontalDimension) GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().posibleMovement = select;
        if (nextRow < GameManager.instance.verticalDimension) GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().posibleMovement = select;
        if (backCol >= 0) GameObject.Find(backCol + "-" + row).GetComponent<Cell>().posibleMovement = select;
        if (backRow >= 0) GameObject.Find(col + "-" + backRow).GetComponent<Cell>().posibleMovement = select;
    }


    void OnEmptyCell()
    {
        if (player == GameManager.instance.player && GameManager.instance.currentPiece == null)
        {

            piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, 0.552f, z), 1);
        }
        if (posibleMovement)
        {
            GameManager.instance.currentPiece.GetComponent<Piece>().Move(x, z, this.gameObject.GetComponent<Cell>());
        }
    }

    void OnCellWithOwnPiece()
    {
        if ((player == GameManager.instance.player || piece.GetComponent<Piece>().owner == GameManager.instance.player) && (GameManager.instance.currentPiece == null || this.gameObject.GetComponent<Cell>() == GameManager.instance.currentCell))
        {
            if (isSelectedPiece)
            {
                PieceIsSelected(false);
            }
            else
            {
                PieceIsSelected(true);
            }
        }
        else if (posibleMovement && piece.GetComponent<Piece>().owner == GameManager.instance.player)
        {
            piece.GetComponent<Piece>().Evolve(x, z, this.gameObject.GetComponent<Cell>());
        }
    }

    void OnCellWithEnemyPiece()
    {
        int luck = Random.Range(0, 10);
        GameObject attacker = GameManager.instance.currentPiece;
        GameObject defender = piece;
        int attackerLvl = attacker.GetComponent<Piece>().level;
        int defenderLvl = piece.GetComponent<Piece>().level;

        if (defenderLvl < attackerLvl)
        {
            Destroy(defender);
            GameManager.instance.currentPiece.GetComponent<Piece>().Move(x, z, this.gameObject.GetComponent<Cell>());
            GameManager.instance.currentCell.GetComponent<Cell>().isSelectedPiece = false;
            GameManager.instance.currentCell.GetComponent<Cell>().piece = null;
            piece = null;
        }
        else if (defenderLvl > attackerLvl)
        {
            Debug.Log("Nope");
        }
        else if (defenderLvl == attackerLvl)
        {
            if (luck <= 3)
            {
                Destroy(defender);
                GameManager.instance.currentPiece.GetComponent<Piece>().Move(x, z, this.gameObject.GetComponent<Cell>());
            }
            else if (luck <= 7)
            {
                GameManager.instance.currentCell.GetComponent<Cell>().isSelectedPiece = false;
                GameManager.instance.currentCell.GetComponent<Cell>().piece = null;
                Destroy(attacker);
                GameManager.instance.TurnChange();
            }
            else if (luck == 8)
            {
                Destroy(defender);
                Destroy(attacker);
                GameManager.instance.currentCell.GetComponent<Cell>().isSelectedPiece = false;
                GameManager.instance.currentCell.GetComponent<Cell>().piece = null;
                piece = null;
                GameManager.instance.TurnChange();
            }
            else if (luck == 9)
            {
                Debug.Log("Nothing");
                GameManager.instance.TurnChange();
            }
        }
        GameManager.instance.currentCell = null;
        GameManager.instance.currentPiece = null;

        foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
        {
            // item.GetComponent<Cell>().render.material = nightMaterial;
            item.GetComponent<Cell>().posibleMovement = false;
        }
    }


}
