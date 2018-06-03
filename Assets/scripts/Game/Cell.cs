using System.Collections;
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
    public bool selectable=true;
    public bool isSelectedPiece;
    public bool isSelectable;

    void OnMouseDown()
    {


        if (piece == null)
        {
            if (player == GameManager.instance.player && selectable)
            {

                float x = transform.position.x;
                float z = this.transform.position.z - 0.4f;
                piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, 0.552f, z));
            }
            if (isSelectable)
            {
               
                float x = transform.position.x;
                float z = this.transform.position.z - 0.4f;
                GameManager.instance.currentPiece.transform.position = new Vector3(x, 0.552f, z);
                foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
                {
                    if (item.GetComponent<Cell>().isSelectedPiece)
                    {
                        item.GetComponent<Cell>().piece = null;

                    }
                }
                piece = GameManager.instance.currentPiece;
                Debug.Log(GameManager.instance.currentCell);
                GameManager.instance.currentCell.selectable=true;
                GameManager.instance.currentCell.render.material = defaultMaterial ;
                GameManager.instance.currentCell.PieceIsSelected(false);
                GameManager.instance.currentPiece = null;
                GameManager.instance.currentCell = null;
               
                GameManager.instance.Cambiarplayer();
     
            }
        }
        else if(player == GameManager.instance.player)
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

        Debug.Log("col:" + col + "row:" + row);
        
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
        foreach (var item in cellsObj)
        {
            item.GetComponent<Cell>().selectable = !select;
        }
        isSelectedPiece = select;
        
        selectable = select;
        if (select)
        {
        GameManager.instance.currentPiece = piece;
        GameManager.instance.currentCell = this;
        GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().render.material = posibleSelectMaterial;
        GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().render.material = posibleSelectMaterial;
        GameObject.Find(backCol + "-" + row).GetComponent<Cell>().render.material = posibleSelectMaterial;
        GameObject.Find(col + "-" + backRow).GetComponent<Cell>().render.material = posibleSelectMaterial;
        }
        else
        {
            GameManager.instance.currentPiece = null;
            GameManager.instance.currentCell = null;
            GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().render.material = defaultMaterial;
            GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().render.material = defaultMaterial;
            GameObject.Find(backCol + "-" + row).GetComponent<Cell>().render.material = defaultMaterial;
            GameObject.Find(col + "-" + backRow).GetComponent<Cell>().render.material = defaultMaterial;
        }
    
        GameObject.Find(nextCol + "-" + row).GetComponent<Cell>().isSelectable = select;
        GameObject.Find(col + "-" + nextRow).GetComponent<Cell>().isSelectable = select;
        GameObject.Find(backCol + "-" + row).GetComponent<Cell>().isSelectable = select;
        GameObject.Find(col + "-" + backRow).GetComponent<Cell>().isSelectable = select;
    }
    void OnMouseOver()
    {
        if (GameManager.instance != null && GameManager.instance.player == player && !GameManager.instance.move)
        {
            render.material = highlightMaterial;
        }
    }

    void OnMouseExit()
    {
        if (!GameManager.instance.move)
        {

        render.material = defaultMaterial;
        }
    }
}
