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
    public void IniciarExit(float x, float z)
    {
        StartCoroutine(WaitExit(x, z));
    }

    IEnumerator WaitExit(float x, float z)
    {
        anima.SetTrigger("Exit");
        yield return new WaitForSeconds(1f);
        transform.position = new Vector3(x, 0.552f, z);
    }


}
