using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSarsa2 : MonoBehaviour{

    public AlgoritmoSarsa sarsa;
    public float puntosporSegundoVivo = 1;
    public float puntosPorCuracion = 1;
    public float puntosPorPeligros = 1;
    public float puntosPorEliminacion = 1;
    public float puntosPorMovimiento = 0.002f;
    public float castigoPeligro = -0.002f;
    public float castigoPorColision = -0.002f;
    public GridAI AmbienteLocal;
    public IAPlayer companionSarsa;
    public float valorControlador;

    private float puntosSegundosDeVida;
    private float puntosCuracion;
    private float puntosPeligros;
    private float puntosEliminacion;
    private float puntosMovimiento;
    private Red redNeural;

    public bool controladorCompanionNavegacion;
    public bool controladorCompanionCombabe;
    public float valDesicionCompanion;
    public float valDesicionAuxiliarCompanion;

    public int ValorInicialAgente = 0;

    private float timer = 0.0f;
    public bool entranador = false;
    public float[] objetivoEntrenamiento = { 1, 0, 0, 0, 0, 0, 0, 0 };

    // Start is called before the first frame update
    private void Awake()
    {
        redNeural = ScriptableObject.CreateInstance<Red>();
    }

    void Start(){
        Debug.Log("existe"+AmbienteLocal.enabled);
        
        redNeural.estado = AmbienteLocal;
        redNeural.RedBase(10000);
    }

    // Update is called once per frame
    void Update(){
        if (!entranador)
        {
            Debug.Log("Paso1");
            redNeural.EvaluarEntorno();
            Debug.Log("Paso2");
            float[] salida = redNeural.EvaluarEntorno();
            ProcesarOutput(salida);
        }
        else {
            float[] salida=redNeural.entrenar(objetivoEntrenamiento);
            ProcesarOutput(salida);
            Debug.Log("Paso3");

        }
        //Evaluar();
        //evolutivo.calificarUno(individuo);
    }

    public List<float> recibirParametrosAmbiente(GridAI AmbienteLocal)
    {
        List<float> entradasAgente;
        entradasAgente = AmbienteLocal.ConstruirEntradasSarsa();
        return entradasAgente;
    }

    public float ValorDesicion(GridAI AmbienteLocal)
    {
        GridAI AmbienteSarsa = AmbienteLocal;
        float valControlador;
        List<float> EntradaControlador = recibirParametrosAmbiente(AmbienteLocal);
        valControlador = AmbienteSarsa.YoDecidoElControlador(EntradaControlador);
        valorControlador = valControlador;
        return valorControlador;
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
                companionSarsa.Disparar();
                break;
            case 1:
                companionSarsa.Adelante();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 2:
                companionSarsa.Derecha();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 3:
                companionSarsa.Atras();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;

            case 4:
                companionSarsa.Izquierda();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 5:
                companionSarsa.Rotar45();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 6:
                companionSarsa.Rotar45N();
                aplicarPuntos((int)Categoria.MOVIMIENTO);
                break;
            case 7:
                companionSarsa.curar();
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

}
