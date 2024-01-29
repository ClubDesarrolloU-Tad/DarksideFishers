using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pez : MonoBehaviour
{
    bool picado = false;   //Para saber si ha picado o no
    Vector3 velocidad;   //Para saber la dirección y velocidad a la que se mueve el pez

    void Start()
    {
        StartCoroutine(cambiarMovimiento());
    }

    void Update()
    {
        if(!picado)
        {
            transform.position += velocidad*Time.deltaTime*0.3f;
        }
        transform.rotation = Quaternion.identity;  //Impide que roten al chocar
    }

    IEnumerator cambiarMovimiento()
    {
        velocidad = new Vector3(0, 0.75f, 0);
        yield return new WaitForSeconds(Random.Range(0, 2));   //Se mantiene quieto un poco antes de empezar a moverse
        velocidad = new Vector3(Random.Range(-2, 3), Random.Range(-1, 3), 0);
        yield return new WaitForSeconds(Random.Range(8, 13));   //Cambia de velocidad aleatoriamente
        StartCoroutine(cambiarMovimiento());
    }
}
