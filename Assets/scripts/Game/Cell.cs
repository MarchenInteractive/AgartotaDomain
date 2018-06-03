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

    void OnMouseDown()
    {
        if (piece == null)
        {
            if (player == GameManager.instance.player)
            {
                float x = this.transform.position.x;
                float z = this.transform.position.z - 0.4f;
                piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, 0.552f, z));

                //if (GameManager.instance.player == 0)
                //{
                //    piece.GetComponent<SpriteRenderer>().sprite = piece.GetComponent<Piece>().spritesMandrake;
                //    GameManager.instance.player = 1;
                //}
                //else if (GameManager.instance.player == 1)
                //{
                //    piece.GetComponent<SpriteRenderer>().sprite = piece.GetComponent<Piece>().spritesPirate;
                //    GameManager.instance.player = 0;
                //}
                //else
                //{ Debug.LogError("Error player not exist"); }
            }
        }
        else
        {
          
        }

        Debug.Log("col:" + col + "row:" + row);
    }

    void OnMouseOver()
    {
        if (GameManager.instance != null && GameManager.instance.player == player)
        {
            render.material = highlightMaterial;
        }
    }

    void OnMouseExit()
    {
        render.material = defaultMaterial;
    }
}
