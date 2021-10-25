﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizacion : MonoBehaviour {
    public LayerMask Obstaculos;
    public LayerMask enemigos;
    public float alcanceVisual = 10f;
   // public Color colorRayo;
   // private GameObject objetivo;
    private  int distancia;
    //  private bool alerta;

    Nodo[,] grid;
    public int visualizacionGridX, visualizacionGridY;


    public int Distancia { get => distancia; set => distancia = value; }

    private void Start()
    {
        Distancia = 0;
    }
    private void Update()
    {
        //Debug.Log("Prueba");
        ComprobrarVisulizacion();
    }
    public bool ComprobrarVisulizacion()
    {
        //Debug.Log("inicio comprobacion");
        Vector3 inicio = transform.position;
        Vector3 director = transform.forward;
        bool hit = Physics.Raycast(inicio, director, alcanceVisual, enemigos, QueryTriggerInteraction.Collide);
        Debug.DrawRay(inicio, (director*alcanceVisual), Color.blue, 1f);
        if (hit) {
            Debug.Log("Golpee un enemigo");
            distancia = 4;
        }

        
        /*
        if (hit !=null)
        {
            if (hit.("Enemy"))
            {
                Debug.Log(hit.collider.gameObject.name);
                //Debug.Log("mira un enemigo");
                Debug.Log("Golpee un enemigo");
                float distanciaRelativa = hit.distance;
                //Debug.Log("Distancia "+distanciaRelativa);
                if (distanciaRelativa <= alcanceVisual && distanciaRelativa > 4f)
                {
                    distancia = 1;
                    Debug.Log("Cambio a distacia 1");
                }
                else if (distanciaRelativa <= 4f && distanciaRelativa > 0)
                {
                    distancia = 2;
                    Debug.Log("Cambio a distacia 2");
                }
                else
                {
                    distancia = 0;
                }
            }
            

        }
        */
        return false;
    }

}
