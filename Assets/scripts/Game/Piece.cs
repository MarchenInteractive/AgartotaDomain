using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public int owner;
    public int level;
    public int col;
    public int row;
    Animator anima;

    private void Start()
    {
        anima = GetComponent<Animator>();
    }
    public void StartExit(float x, float z)
    {
        StartCoroutine(WaitExit(x, z));
    }

    IEnumerator WaitExit(float x, float z)
    {
        anima.SetTrigger("Exit");
        yield return new WaitForSeconds(1f);
        if (level == 1 || level == 2)
        {
            transform.position = new Vector3(x, 0.552f, z);
        }
        if (level == 3)
        {
            transform.position = new Vector3(x, 0.625f, z);
        }
        if (level == 4)
        {
            transform.position = new Vector3(x, 0.77f, z);
        }
    }

    IEnumerator WaitForEndOfAnimCombined(int lvl, float x, float y, float z, Cell cell)
    {
        yield return new WaitForSeconds(1f);
        foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (item.GetComponent<Cell>().isSelectedPiece)
            {
                Destroy(item.GetComponent<Cell>().piece);
            }
        }
        Destroy(cell.piece);
        // GameManager.instance.currentCell.render.material = defaultMaterial;
        GameManager.instance.currentCell.PieceIsSelected(false);
        GameManager.instance.currentPiece = null;
        GameManager.instance.currentCell = null;
        cell.piece = GameManager.instance.InsertNewPiece(col, row, GameManager.instance.player, new Vector3(x, y, z), lvl);
    }

    public void Move(float x, float z, Cell cell)
    {
        GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
        GameManager.instance.TurnChange();
        foreach (var item in GameObject.FindGameObjectsWithTag("cell"))
        {
            if (item.GetComponent<Cell>().isSelectedPiece)
            {
                item.GetComponent<Cell>().piece = null;
            }
        }
        cell.piece = GameManager.instance.currentPiece;
        GameManager.instance.currentCell.render.material = cell.defaultMaterial;
        GameManager.instance.currentCell.PieceIsSelected(false);
        GameManager.instance.currentPiece = null;
        GameManager.instance.currentCell = null;
    }

    public void Evolve(float x, float z, Cell cell)
    {
        if (
            this.gameObject.GetComponent<Piece>().owner == GameManager.instance.player &&
            GameManager.instance.currentPiece.GetComponent<Piece>().GetComponent<Piece>().owner == GameManager.instance.player
            )
        {
            if (this.gameObject.GetComponent<Piece>().level == GameManager.instance.currentPiece.GetComponent<Piece>().GetComponent<Piece>().level)
            {
                float y = 0.552f;
                if (this.gameObject.GetComponent<Piece>().level == 1)
                {
                    y = 0.552f;
                }
                else if (this.gameObject.GetComponent<Piece>().level == 2)
                {
                    y = 0.625f;
                }
                else if (this.gameObject.GetComponent<Piece>().level == 3)
                {
                    y = 0.77f;
                }
                GameManager.instance.currentPiece.GetComponent<Piece>().StartExit(x, z);
                this.gameObject.GetComponent<Piece>().StartExit(x, z);

                StartCoroutine(WaitForEndOfAnimCombined(this.gameObject.GetComponent<Piece>().level + 1, x, y, z, cell));
            }
        }
    }


}
