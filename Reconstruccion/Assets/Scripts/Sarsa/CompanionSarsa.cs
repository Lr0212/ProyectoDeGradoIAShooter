using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSarsa : MonoBehaviour{

    public AlgoritmoSarsa Sarsa;
    public GridAI AmbienteLocal;
    public IAPlayer companion;
    private Red redNeural;

    public float puntosporSegundoVivo = 1;
    public float puntosPorCuracion = 1;
    public float puntosPorPeligros = 1;
    public float puntosPorEliminacion = 1;
    public float puntosPorMovimiento = 0.002f;
    public float castigoPeligro = -0.002f;
    public float castigoPorColision = -0.002f;
  
    private float puntosSegundosDeVida;
    private float puntosCuracion;
    private float puntosPeligros;
    private float puntosEliminacion;
    private float puntosMovimiento;
    
    public int ValorInicialAgente = 0;
    public float valorControlador;


    private float timer = 0.0f;
    public bool entrenador = false;
    public float[] objetivoEntrenamiento = { 1, 0, 0, 0, 0, 0, 0, 0 };

    public bool controladorCompanionNavegacion;
    public bool controladorCompanionCombabe;
    public float valDesicionCompanion;
    public float valDesicionAuxiliarCompanion;

    public struct State
    {
        public float valControladorStruct;
        public bool controladorActual;
    }
    public State estadoActual = new State();
    
    // Start is called before the first frame update
    private void Awake()
    {
        redNeural = ScriptableObject.CreateInstance<Red>();
        //sarsa = FindObjectOfType<AlgoritmoSarsa>();
    }

    void Start(){
        //Debug.Log("existe " + AmbienteLocal.enabled);

        Sarsa = GameObject.FindGameObjectWithTag("Sarsa").GetComponent<AlgoritmoSarsa>();

        redNeural.estado = AmbienteLocal;
        redNeural.RedBase(10000);
    }

    // Update is called once per frame
    void Update(){
        


        /*
        // Entrenador 
        if (!entrenador)
        {
           // Debug.Log("Paso1");
            redNeural.EvaluarEntorno();
           // Debug.Log("Paso2");
            float[] salida = redNeural.EvaluarEntorno();
            ProcesarOutput(salida);
        }
        else {
            float[] salida=redNeural.entrenar(objetivoEntrenamiento);
            ProcesarOutput(salida);
             //Debug.Log("Paso Final Update ");

        }
        Debug.Log("Paso Final Update ");
        */
    }

    public void ProcesarOutput(float[] ValoresDeConfianza){

        float MejorValor = Mathf.Max(ValoresDeConfianza);
        String prueba = "[";
        prueba += "]";
        int Decision = Array.IndexOf(ValoresDeConfianza, MejorValor);
        // Debug.Log("Decision: "+ Decision+" Mejor valor"+MejorValor+"valores: "+prueba);
        switch (Decision)
        {
            case 0:
                companion.Disparar();
                break;
            case 1:
                companion.Adelante();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 2:
                companion.Derecha();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 3:
                companion.Atras();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;

            case 4:
                companion.Izquierda();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 5:
                companion.Rotar45();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 6:
                companion.Rotar45N();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 7:
                companion.curar();
                aplicarPuntos((int)Categoria.CURACION);
                break;

        }

        AmbienteLocal.RefreshGrid();
    }
    
    public void aplicarPuntos(int categoria){

        switch (categoria)
        {
            case 0:
                puntosSegundosDeVida = puntosSegundosDeVida * puntosporSegundoVivo;
                break;
            case 1:
                puntosCuracion += puntosPorCuracion;
                break;
            case 2:
                puntosPeligros += puntosPorPeligros;
                break;
            case 3:
                puntosEliminacion += puntosPorEliminacion;
                break;
            case 4:
                puntosMovimiento += puntosPorMovimiento;
                break;
        }
    }
    
    public void aplicarCastigo(int categoria){

        switch (categoria)
        {
            case 2:
                puntosPeligros += castigoPeligro;
                break;
            case 4:
                puntosPorMovimiento += castigoPorColision;
                break;
        }
    }
    
    public enum Categoria : int
    {
        TIEMPO,
        CURACION,
        PELIGROS,
        ELIMINACIONES,
        MOVIMIENTO
    }
    
    public List<float> recibirParametrosAmbiente(GridAI AmbienteLocal)
    {
        List<float> entradasAgente;
        entradasAgente = AmbienteLocal.ConstruirEntradasSarsa();
        return entradasAgente;
    }

    public void setRedSarsa(Red nuevaRed) {
        redNeural = nuevaRed;
        redNeural.estado = AmbienteLocal;
        redNeural.GenerarRed(Sarsa.maximoNeuronas);
        AmbienteLocal.transform.SetParent(companion.transform);
        Debug.Log("Se ha generado la red");
        // InvokeRepeating("Jugar", 0.2f, 0.2f);

    }
    public float ValorDesicion(GridAI AmbienteLocal, State estadoActual)
    {
        GridAI AmbienteSarsa = AmbienteLocal;
        float valControlador;
        List<float> EntradaControlador = recibirParametrosAmbiente(AmbienteLocal);
        valControlador = AmbienteSarsa.YoDecidoElControlador(EntradaControlador);
        valorControlador = valControlador;
        estadoActual.valControladorStruct = valorControlador;
        return valorControlador;
    }


}