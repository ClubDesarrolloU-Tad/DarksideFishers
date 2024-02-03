using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pez : MonoBehaviour
{
    bool picado = false;   //Para saber si ha picado o no
    bool persiguiendo = false;  //PAra saber si está persiguiendo el sedal o no
    Vector3 velocidad;   //Para saber la dirección y velocidad a la que se mueve el pez (en movimiento aleatorio)
    Vector3 velocidad_perseguir;
    [SerializeField] ControlPesca controlador;    //A través de este script sabrá si hay otro pez en el sedal
    [SerializeField] GameObject anzuelo;  
    float dist_anzuelo;   //Distancia del pez al anzuelo

    void Start()
    {
        StartCoroutine(cambiarMovimiento());
    }

    void Update()
    {
        if(!controlador.pescando && !persiguiendo)   //Movimiento aleatorio
        {
            transform.position += velocidad*Time.deltaTime*0.3f;
        }

        else if(!controlador.pescando && persiguiendo)   //Persigue el anzuelo
        {
            if(anzuelo.transform.position.x - transform.position.x > 0.1f)
            {
                velocidad_perseguir = new Vector3(1, velocidad_perseguir.y, 0);   //Hacia la derecha
            }

            else if(anzuelo.transform.position.x - transform.position.x < -0.1f)
            {
                velocidad_perseguir = new Vector3(-1, velocidad_perseguir.y, 0);   //Hacia la izquierda
            }

            else
            {
                velocidad_perseguir = new Vector3(0, velocidad_perseguir.y, 0);   //Ni derecha ni izquierda
            }

            if(anzuelo.transform.position.y - transform.position.y > 0.1f)
            {
                velocidad_perseguir = new Vector3(velocidad_perseguir.x, 2, 0);   //Hacia arriba
            }

            else if(anzuelo.transform.position.y - transform.position.y < -0.1f)
            {
                velocidad_perseguir = new Vector3(velocidad_perseguir.x, -2, 0);   //Hacia abajo
            }

            else
            {
                velocidad_perseguir = new Vector3(velocidad_perseguir.x, 0, 0);   //Ni arriba ni abajo
            }

            transform.position += velocidad_perseguir*Time.deltaTime*0.3f;   //Persigue el anzuelo
        }

        if(!controlador.pescando) 
        {
            dist_anzuelo = Mathf.Sqrt(Mathf.Pow((anzuelo.transform.position.x - transform.position.x), 2) + Mathf.Pow((anzuelo.transform.position.y -transform.position.y), 2));  //Se calcula la distancia entre el anzuelo y el pez

            if(dist_anzuelo < 2 && controlador.lanzado)
            {
                persiguiendo = true;
            }
            else
            {
                persiguiendo = false;
            }

            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.2f;
        }

        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;  //Para que no se hundan mientras se pesca
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "anzuelo" && !controlador.pescando)   //El pez llega al anzuelo
        {
            controlador.pescando = true;
            controlador.pez_pescando = gameObject;
            transform.SetParent(anzuelo.transform);  //Para que siga al anzuelo
            persiguiendo = false;
            controlador.picado();
        }
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
