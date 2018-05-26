using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject casillaA;
    public GameObject casillaB;
    public int dimesionHorizontal = 6;
    public int dimesionvertical = 5;

    public int jugador;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(GenerarTableroLento());
    }

    public void CambiarJugador()
    {
        jugador = 1;
    }

    private IEnumerator GenerarTableroLento()
    {
        GameObject temp;
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < dimesionHorizontal; i++)
        {


            for (int j = 0; j < dimesionvertical; j++)
            {
                temp = GameObject.Instantiate(casillaA, new Vector3(i, 0f, j), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void GenerarTablero()
    {
        GameObject temp;
        for (int i = 0; i < dimesionHorizontal; i++)
        {
            for (int j = 0; j < dimesionvertical; j++)
            {
                temp = GameObject.Instantiate(casillaA, new Vector3(i, 0f, j), Quaternion.identity);
            }
        }
    }

    public int Batalla(int atacante, int defensor)
    {
        int suerte = Random.Range(0, 10);

        if (defensor < atacante)
        {
            return 0;
        }
        else if (defensor > atacante)
        {
            return 1;
        }
        else if (defensor == atacante)
        {
            if (suerte <= 3)
            {
                return 2;
            }
            else if (suerte <= 7)
            {
                return 0;
            }
            else if (suerte == 8)
            {
                return 3;
            }
            else if (suerte == 9)
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
