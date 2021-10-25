using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoritmoSarsa : MonoBehaviour
{
    public CompanionSarsa companionSarsa;
    private List<Red> redNeuronal;
    // private float[] politica;//estado-action
    // private float[] trazaEligibilidadEstado;//estado-action
    public IAPlayer companion;
    List<NeuronaSarsa> RedNeural;
    public GridAI AmbienteLocal;//Deteccion Fisica
    public int tamanioEntradaX;
    public int tamanioEntradaY;
    public int tamamnioSalida;
    public int maximoNeuronas = 1000000;

    public List<float> estados;

    public float puntosPorDaño = 1;
    public float puntosPorMovimiento = 0.002f;
    public int puntosPorTiempo = 1;
    public float puntosPorEliminacion = 2;
    public int puntosPorCuracion = 5;
    public float castigoPorDaño = -1;
    public float castigoPorColision = -0.002f;
    GameObject ambientes;

    public bool controladorNavegacion;
    public bool controladorCombabe;
    public float valDesicion;
    public float valDesicionAuxiliar;
    public float recompensa;
    public float recompensaAuxiliar;


    public float q_valor;
    public float factorDescuento;
    public float tasaAprendizaje;
    public float trazaEligibilidad;





    // Start is called before the first frame update
    void Start()
    {

        //Revisar
        /*  RedNeural = new List<Neurona>();
          companion = GameObject.FindGameObjectWithTag("Companion").GetComponent<IAPlayer>();
          AmbienteLocal.IsSarsa = true;
          tamanioEntradaX = AmbienteLocal.tamanioGridX;
          tamanioEntradaY = AmbienteLocal.tamanioGridY;
          int aux = 0;

          //introducir neuronas entrada
          for (int i = 0; i < tamanioEntradaX; i++) {
              for (int j = 0; j < tamanioEntradaY; j++) {
                  Neurona neuronaLocal =ScriptableObject.CreateInstance<Neurona>();
                  neuronaLocal.idTipo = -1;
                  neuronaLocal.indice = aux;
                  RedNeural.Add(neuronaLocal);
                  aux++;
              }
          }

          //introducir neuronas salida
          for (int i = 0; i < tamamnioSalida; i++) {
              Neurona neuronaLocal = new Neurona();
              neuronaLocal.idTipo = 1;
              RedNeural.Add(neuronaLocal);
              aux++;
          }
          InvokeRepeating("Jugar",0.2f,0.2f);*/
    }

    void Update()
    {

        //Debug.Log("prueba jugar AlgSarsa ");
       // List<float> EntradaRed = AmbienteLocal.ConstruirEntradasSarsa();
        // Debug.Log(EntradaRed.ToString());
        float pruebaValContro;
        pruebaValContro = FuncionValorAccionEstado(AmbienteLocal);
        //pruebaValContro = AmbienteLocal.YoDecidoElControlador(EntradaRed);
        //Debug.Log("prueba Valor controlador " + pruebaValContro);
        //una vez termina la accion, actualiza entradas 
        CambiodeControlador(pruebaValContro);
        //Debug.Log("Prueba Controlador navegacion " + controladorNavegacion);
        //Debug.Log("Prueba Controlador Combate " + controladorCombabe);
        pruebaValContro = FuncionValorAccionEstado(AmbienteLocal);

        AmbienteLocal.RefreshGrid();
    }

    void Jugar()
    {
        

    }

    float FuncionValorAccionEstado(GridAI ambiente)
    {
        ambiente = AmbienteLocal;
       // List<float> entradas = ambiente.ConstruirEntradasSarsa();
        float valoreDesicion = ambiente.YoDecidoElControlador(ambiente.ConstruirEntradasSarsa());
        valDesicion = valoreDesicion;
        return valoreDesicion;

    }

    void CambiodeControlador(float valDesicion) {
        float valoreDesicionEstado = valDesicion;
        if (0.9090909091 <= valoreDesicionEstado && valoreDesicionEstado >= 3)
        {
            if (valoreDesicionEstado != 1.272727273)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 1.272727273)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 1.636363636)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.090909091)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.454545455)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.545454545)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.636363636)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.727272727)
            {
                controladorNavegacion = true;
            }
            if (valoreDesicionEstado != 2.818181818)
            {
                controladorNavegacion = true;
            }
            else if (1.818181818 <= valoreDesicionEstado && valoreDesicionEstado >= 4)
            {

                if (valoreDesicionEstado != 2.090909091)
                {
                    controladorCombabe = true;
                }
                if (valoreDesicionEstado != 2.454545455)
                {
                    controladorCombabe = true;
                }
                if (valoreDesicionEstado != 2.545454545)
                {
                    controladorCombabe = true;
                }
                if (valoreDesicionEstado != 2.636363636)
                {
                    controladorCombabe = true;
                }
                if (valoreDesicionEstado != 2.727272727)
                {
                    controladorCombabe = true;
                }
                if (valoreDesicionEstado != 2.818181818)
                {
                    controladorCombabe = true;
                }
                controladorCombabe = true;
            }
            controladorNavegacion = true;
        }


    }

    void tomarAccion(int id)
    {
        switch (id)
        {
            case 0:
                companion.Adelante();
                break;
            case 1:
                companion.Atras();
                break;
            case 2:
                companion.Izquierda();
                break;
            case 3:
                companion.Derecha();
                break;
            case 4:
                companion.Disparar();
                break;

        }
    }

    void Recompesar(float recompensa)
    {

    }
    void obtenerEstadoActual()
    {

    }
    


}



/* 
public float[][] q_tabla;   // Matriz que contiene los valores estimados.
float tasa_aprendizaje = 0.5f; // Tasa de actualización de estimaciones por valor dada una recompensa. 
int accion = -1;
float gamma = 0.99f; // Factor de descuento para calcular Q-Objetivo.
float epsilon = 1; // Epsilon inicial para la selección de acciones aleatorias.
float eMin = 0.1f; // Límite inferior de épsilon.
int recuentoPasos = 2000; // Número de pasos para bajar Epsilon a eMin.
int ultimoEstado;

public void EnviarParametros(ParametrosAmbiente env)
{
    q_tabla = new float[env.estados_tamaño][];
    accion = 0;
    for (int i = 0; i < env.estados_tamaño; i++)
    {
        q_tabla[i] = new float[env.accion_tamaño];
        for (int j = 0; j < env.accion_tamaño; j++)
        {
            q_tabla[i][j] = 0.0f;
        }
    }
}

/// <summary> 
/// Selecciona una acción para realizar desde su estado actual. 
/// </summary> 
/// <returns> La opción elegida por la política del agente </returns> 

public float[] ObtenerAccion()
{
    //accion = q_tabla[ultimoEstado].ToList().IndexOf(q_tabla[ultimoEstado].Max());
    if (Random.Range(0f, 1f) < epsilon) { accion = Random.Range(0, 3); }
    if (epsilon > eMin) { epsilon = epsilon - ((1f - eMin) / (float)recuentoPasos); }
    //GameObject.Find("ETxt").GetComponent<Text>().text = "Epsilon: " + epsilon.ToString("F2");
    float currentQ = q_tabla[ultimoEstado][accion];
    //GameObject.Find("QTxt").GetComponent<Text>().text = "Current Q-value: " + currentQ.ToString("F2");
    return new float[1] { accion };
}


/// <resumen>
/// Actualiza la matriz de estimación de valor dada una nueva experiencia (estado, acción, recompensa).
/// </summary>
/// <param name = "estado"> El estado del entorno en el que ocurrió la experiencia. </param>
/// <param name = "recompensa"> La recompensa recibida por el agente del entorno por su acción. </param>
/// <param name = "hecho"> Si el episodio ha terminado </param>

public void EnviarEstado(List<float> estado, float recompesa, bool hecho)
{
    //int nextState = Mathf.FloorToInt(estado.First());
    float proximoEstado;
    if (accion != -1)
    {
        if (hecho == true)
        {
            q_tabla[ultimoEstado][accion] += tasa_aprendizaje * (recompesa - q_tabla[ultimoEstado][accion]);
        }
        else
        {
            //   q_tabla[ultimoEstado][accion] += tasa_aprendizaje * (recompesa + gamma * q_tabla[nextState].Max() - q_tabla[ultimoEstado][accion]);
        }
    }
    //ultimoEstado = proximoEstado;
}

void FuncionValorAccionEstad2()
{
    int i = politica.Length;
    while (i > 0)
    {
        i--;
    }
}
*/
