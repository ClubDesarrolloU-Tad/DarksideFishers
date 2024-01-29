using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPesca : MonoBehaviour
{

    bool lanzado = false;  //Variable para saber si se ha lanzado el sedal
    bool pescando = false;   //Variable para saber si ha picado un pez
    float velocidad_scroll = 1;   //Velocidad a la que aumenta la barra de fuerza para lanzar el sedal
    int sentido_barra = 1;   //Sentido en el que se mueve la barra (1 es arriba, -1 es abajo)
    float fuerza;   //Fuerza con la que se lanzará el sedal, va de 0 a 1

    [SerializeField] GameObject barra_lanzar;   //Barra (con slider) que indica la fuerza con la que se lanzara el sedal
    [SerializeField] GameObject sedal;   //El sedal que se lanza

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && !pescando)   //Se empieza a pulsar click derecho sin haber un pez enganchado
        {
            barra_lanzar.SetActive(true);
            sedal.SetActive(false);
            lanzado = false;
            fuerza = 0;
            sentido_barra = 1;
        }


        if(!lanzado && !pescando)
        {

            if(Input.GetMouseButton(1))   //Se mantiene el click derecho
            {
                fuerza += sentido_barra*velocidad_scroll*Time.deltaTime;

                if(fuerza > 1)   //La fuerza no debe estar por encima de 1 ni por debajo de 0. Si los supera, pasa de sumar a restar o al reves
                {
                    fuerza = 1;
                    sentido_barra = -1;
                }

                else if(fuerza < 0)
                {
                    fuerza = 0;
                    sentido_barra = 1;
                }


                barra_lanzar.GetComponent<Slider>().value = fuerza;   //La barra va aumentando o disminuyendo
            }

            if(Input.GetMouseButtonUp(1))   //Se suelta el click derecho
            {
                StartCoroutine(lanzarSedal());
            }
        }
    }

    IEnumerator lanzarSedal()  //Funcion para lanzar el sedal
    {
        lanzado = true;
        yield return new WaitForSeconds(1);
        sedal.SetActive(true);
        barra_lanzar.SetActive(false);
        sedal.transform.position = new Vector3(transform.position.x - 0.3f, transform.position.y, 0);
        sedal.GetComponent<Rigidbody2D>().AddForce(new Vector2(-0.2f -1.2f*fuerza, 0), ForceMode2D.Impulse);

    }
}
