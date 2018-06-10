﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public int player = 0;
    public int col;
    public int row;
    public Renderer render;
    public Material defaultMaterial;
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
            OnCellWithPiece();
        }


    }
    IEnumerator WaitForEndOfAnimCombined(int lvl, float y)
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (item.GetComponent<Cell>().isSelectedPiece)
            {
                Destroy(item.GetComponent<Cell>().piece);
            }
        }
        Destroy(this.piece);
        GameManager.instance.currentCell.render.material = defaultMaterial;
        GameManager.instance.currentCell.PieceIsSelected(false);
        GameManager.instance.currentPiece = null;
        GameManager.instance.currentCell = null;
        piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, y, z), lvl);
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
            if (nextCol < GameManager.instance.horizontalDimension) GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().render.material = defaultMaterial;
            if (nextRow < GameManager.instance.verticalDimension) GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().render.material = defaultMaterial;
            if (backCol >= 0) GameObject.Find(backCol + "-" + row).GetComponent<Cell>().render.material = defaultMaterial;
            if (backRow >= 0) GameObject.Find(col + "-" + backRow).GetComponent<Cell>().render.material = defaultMaterial;
        }
        if (nextCol < GameManager.instance.horizontalDimension) GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().posibleMovement = select;
        if (nextRow < GameManager.instance.verticalDimension) GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().posibleMovement = select;
        if (backCol >= 0) GameObject.Find(backCol + "-" + row).GetComponent<Cell>().posibleMovement = select;
        if (backRow >= 0) GameObject.Find(col + "-" + backRow).GetComponent<Cell>().posibleMovement = select;
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
            render.material = defaultMaterial;
        }
    }


    void OnEmptyCell()
    {
        if (player == GameManager.instance.player && GameManager.instance.currentPiece == null)
        {

            piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, 0.552f, z), 1);
            render.material = defaultMaterial;
        }
        if (posibleMovement)
        {
            GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
            GameManager.instance.CambioDeTurno();
            foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
            {
                if (item.GetComponent<Cell>().isSelectedPiece)
                {
                    item.GetComponent<Cell>().piece = null;
                }
            }
            piece = GameManager.instance.currentPiece;
            GameManager.instance.currentCell.render.material = defaultMaterial;
            GameManager.instance.currentCell.PieceIsSelected(false);
            GameManager.instance.currentPiece = null;
            GameManager.instance.currentCell = null;
        }
    }

    void OnCellWithPiece()
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
            Evolve();
        }
    }

    void Evolve()
    {
        if (piece.GetComponent<Piece>().level == 1 && GameManager.instance.currentPiece.GetComponent<Piece>().GetComponent<Piece>().level == 1)
        {
            //
            GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
            piece.GetComponent<Piece>().StartExit(x, z);

            StartCoroutine(WaitForEndOfAnimCombined(2, 0.552f));
        }
        if (piece.GetComponent<Piece>().level == 2 && GameManager.instance.currentPiece.GetComponent<Piece>().GetComponent<Piece>().level == 2)
        {
            GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
            piece.GetComponent<Piece>().StartExit(x, z);

            StartCoroutine(WaitForEndOfAnimCombined(3, 0.625f));
        }
        if (piece.GetComponent<Piece>().level == 3 && GameManager.instance.currentPiece.GetComponent<Piece>().GetComponent<Piece>().level == 3)
        {
            GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
            piece.GetComponent<Piece>().StartExit(x, z);

            StartCoroutine(WaitForEndOfAnimCombined(4, 0.77f));
        }
    }
}
