using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casilla : MonoBehaviour
{
    public int jugador = 0;
    public GameObject resaltar;


    void OnMouseOver()
    {
        Debug.Log("Mouse Encima");
        if (GameManager.instance.jugador == jugador)
        {
            resaltar.SetActive(true);
        }
    }


    void OnMouseExit()
    {
        if (GameManager.instance.jugador == jugador)
        {
            resaltar.SetActive(false);
        }
    }
}
