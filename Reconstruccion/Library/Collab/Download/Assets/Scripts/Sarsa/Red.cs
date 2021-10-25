using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : ScriptableObject
{
    public List<Conexion> conexiones = new List<Conexion>();
    public GridAI estado;
    public List<NeuronaSarsa> redNeural;
    public int count = 0;
    public int maximoNeuronas=10000;
    public int maxNodos;
    public int tamanioEntrada=11;
    public int tamanioSalida=8;
    public int tamañoRed;

    private int posicionSalida = 0;
    private int numCapasOcultas = 1;
    public float learingRate = .9F;

    public void SetConexiones(List<Conexion> conex) {
        conexiones = conex;
    }

    public void SetRedInicial(List<NeuronaSarsa> conex)
    {
        tamañoRed = conex.Count;
        foreach (NeuronaSarsa neuronaS in conex)
        {
            foreach (Conexion neurona in neuronaS.entrante)
            {
                neurona.peso = Random.Range(-1.0f, 1.0f);
            }
        }
        foreach (Conexion neurona in conexiones)
        {
            neurona.peso = Random.Range(-1.0f, 1.0f);
        }
        redNeural = new List<NeuronaSarsa>(conex);
    }

    public List<NeuronaSarsa> RedBase(int maxNeuronas)
    {
        List<NeuronaSarsa> neuroSarsa = new List<NeuronaSarsa>();
        maxNodos = maxNeuronas;
        Debug.Log("parte 1 red base");
        for (int i = 0; i < tamanioEntrada; i++)
        {//inputs
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Debug.Log("parte 2 red base");
            Aux.idTipo = -1;
            neuroSarsa.Add(Aux);
            Debug.Log("parte 3 red base");
            maximoNeuronas = i;
        }
        posicionSalida = maximoNeuronas + 1;
        for (int o = 0; o < tamanioSalida; o++)
        {
            NeuronaSarsa Aux = CreateInstance<NeuronaSarsa>();
            Aux.idTipo = 1;
            neuroSarsa.Add(Aux);
        }
        int count = 0;
        for (int i = 0; i < tamanioEntrada; i++)
        {
            for (int o = 0; o < tamanioSalida; o++)
            {
                Conexion nuevaConex = CreateInstance<Conexion>();

                nuevaConex.entrada = i;
                nuevaConex.salida = posicionSalida + o;
                nuevaConex.peso = Random.Range(0f, 1f);

                count++;
                nuevaConex.habilitado = true;
                conexiones.Add(nuevaConex);
            }
        }

        foreach (Conexion conexion in conexiones)
        {

            // conexiones[conexion.salida].
        }

        redNeural = neuroSarsa;
        tamañoRed = neuroSarsa.Count;
        return redNeural;
    }

    /// <summary>
    /// construye una red base sin capa oculta
    /// </summary>
    
    public void GenerarRed(int maxNeuronas)
    {
        List<NeuronaSarsa> neuronasSarsa = new List<NeuronaSarsa>();
        maxNodos = maxNeuronas;
        Debug.Log("parte 1 red base");
        //List<float> entradas = estado.ConstruirEntradasSarsa();
       // maxNodos = maxNeuronas;

       // if (conexiones.Count != 0) { }

        //entradas
        for (int i = 0; i < tamanioEntrada; i++)
        {
            NeuronaSarsa Aux = ScriptableObject.CreateInstance<NeuronaSarsa>();
            Aux.indice = count; Debug.Log("parte 2 red base");
            redNeural.Add(Aux);
            Debug.Log("parte 3 red base");
            count++;
        }
        //salida
        posicionSalida = redNeural.Count;
        for (int i = 0; i < 8; i++)
        {
            NeuronaSarsa Aux = ScriptableObject.CreateInstance<NeuronaSarsa>();
            Aux.indice = count;
            redNeural.Add(Aux);
            count++;
        }
        Debug.Log("pruebas conexions");
        foreach (Conexion conexion in conexiones)
        {
            if (conexion.habilitado)
            {
                try
                {
                    if (conexion.salida >= redNeural.Count)
                    {
                        conexion.salida = redNeural.Count;
                        redNeural.Add(ScriptableObject.CreateInstance<NeuronaSarsa>());               

                    }

                    redNeural[conexion.salida].entrante.Add(conexion);
                    if (conexion.entrada >= redNeural.Count)
                    {
                        conexion.entrada = redNeural.Count;
                        redNeural.Add(ScriptableObject.CreateInstance<NeuronaSarsa>());
                    
                    }

                    redNeural[conexion.entrada].entrante.Add(conexion);
                }
                catch (System.Exception e)
                {
                    Debug.Log("salida= error" + e.Data.ToString());
                    // Debug.Log("salida=" + e.Data.ToString());
                }
            }
        }
    }

    public float[] EvaluarEntorno()
    {
        List<float> entradaSarsa = estado.ConstruirEntradasSarsa();
        for (int i = 0; i < entradaSarsa.Count; i++)
        {//inputs
            redNeural[i].valor = entradaSarsa[i];
        }

        foreach (NeuronaSarsa neuronaS in redNeural)
        {
            float suma = 0;
            foreach (Conexion conex in neuronaS.entrante)
            {
                NeuronaSarsa otraNeurona = redNeural[conex.entrada];
                suma += conex.peso * otraNeurona.valor;
            }

            if (neuronaS.entrante != null)
            {
                neuronaS.valor = Sigmoide(suma);
                if (neuronaS.idTipo == 1)
                {

                }
            }
        }

        // 8 con respecto a las acciones posibles del companion

        float[] salida = new float[8];

        int limite = posicionSalida + tamanioSalida;
        int count = 0;
        for (int i = posicionSalida; i < limite; i++)
        {//outputs
            salida[count] = redNeural[i].valor;
            count++;
        }
        return salida;
    }


    public float Sigmoide(float x)
    {
        float relu = Mathf.Max(0, x);
        float soft = Mathf.Log((1 + Mathf.Exp(x)));
        float sig = 1 / (1 + Mathf.Exp(-.1f * x));
        return relu;
    }


    public float[] entrenar(float[] objetivo) {

        float[] salida = EvaluarEntorno();

        // calcular error restando el objetivo comparado con  lo obtenido 
        float[] errorSalida= new float [salida.Length];
        for (int i = 0; i < salida.Length; i++) {
            errorSalida[i] = objetivo[i] - salida[i];
        }
        float[] gradiente = new float[salida.Length];

        for (int i=0;i<salida.Length;i++) {
            gradiente[i] = salida[i] * (1 - salida[i]);//para sigmoide
            gradiente[i] = gradiente[i] * errorSalida[i];
            gradiente[i] = gradiente[i] * learingRate;
        }

        int tamañoCapaAnterior=redNeural[posicionSalida].entrante.Count;
        float[,] pesosCapaOculta = new float[tamañoCapaAnterior,salida.Length];
        float[] salidaCapaOculta = new float[tamañoCapaAnterior];
        for (int i = 0; i < salida.Length; i++)
        {
            for (int j = 0; j < tamañoCapaAnterior; j++)
            {
                pesosCapaOculta[j, i] = redNeural[posicionSalida+i].entrante[j].peso;
                salidaCapaOculta[j] = redNeural[redNeural[posicionSalida + i].entrante[j].entrada].valor;
            }
        }

        //revisar este ciclo
        for (int i = 0; i < gradiente.Length; i++)
        {
            for (int j = 0; j < tamañoCapaAnterior; j++)
            {
                salidaCapaOculta[j] = salidaCapaOculta[j] * gradiente[i];
            }
        }

        for (int i = 0; i < salida.Length; i++)
        {
            for (int j = 0; j < tamañoCapaAnterior; j++)
            {
                redNeural[posicionSalida + i].entrante[j].peso+=salidaCapaOculta[j];//actualizo pesos de la capa oculta a la de salida 
            }
        }




        float[] errorCapaOculta = new float[tamañoCapaAnterior];
        //transponer la matrix de pesos para calcular el error de la capa oculta 
        for (int i = 0; i < tamanioSalida; i++) {
            for (int j = 0; j < tamañoCapaAnterior; j++) {
                errorCapaOculta[j] += pesosCapaOculta[j, i] * errorSalida[i];
            }
        }

        return salida;
    }


    public NeuronaSarsa kEsimaNeurona(int k)
    {
        return redNeural[k];
    }
}



/// <summary>
/// este procesa la entrada y devuelve los valores de confianza de la salida (accion)
/// </summary>
/// <returns></returns>
/* public float[] EvaluarEntorno()
 {
     float[] entradas = vision.construirEntradas();
     for (int i = 0; i < entradas.Length; i++)
     {//inputs
         redNeural[i].valor = entradas[i];
     }

     foreach (Neurona neurona in redNeural)
     {
         float suma = 0;
         foreach (GenNEAT gen in neurona.entrante)
         {
             Neurona otraNeurona = redNeural[gen.entrada];
             suma += gen.peso * otraNeurona.valor;
         }

         if (neurona.entrante != null)
         {
             neurona.valor = Sigmoide(suma);
             if (neurona.idTipo == 1)
             {
                 // Debug.Log("Sigmoide a la salida=" + suma);
             }
         }
     }
     float[] salida = new float[8];

     int limite = posicionSalida + tamanioSalida;
     int count = 0;
     for (int i = posicionSalida; i < limite; i++)
     {//outputs
         salida[count] = redNeural[i].valor;
         count++;

     }
     return salida;
 }*/

