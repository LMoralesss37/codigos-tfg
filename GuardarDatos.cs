using UnityEngine;
using TMPro;

public class GuardarDatos : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField edadInput;
    public TMP_InputField alturaInput;
    public TMP_InputField pesoInput;

    public Movimientomanzanas manzanas;
    public Movimientomangos mangos;
    public Movimientoplatanos platanos;
    public Movimientonaranjas naranjas;
    public Movimientolimones limones;

    public UnityEngine.UI.Slider SliderDolor;

    private DatosExcel datosJugador = new DatosExcel();
    private EditarExcel editarexcel;
    private Frecuencia_Cardiaca frecuenciaCardiaca;
    private System.DateTime comienzajuego;

    [HideInInspector]
    public int contadorEjercicioRapido = 0;

    void Start()
    {
        editarexcel = FindFirstObjectByType<EditarExcel>();
        frecuenciaCardiaca = FindFirstObjectByType<Frecuencia_Cardiaca>();
        datosJugador.articulacion = "Hombro";
        datosJugador.descripcionTarea = "Ejercicio de desplazamiento vertical en pared con pelota";
        datosJugador.nivelDificultad = 2;
        datosJugador.fechaConexion = System.DateTime.Now;
        datosJugador.horaConexion = System.DateTime.Now.TimeOfDay;
        comienzajuego = System.DateTime.Now;
    }

    public void ConseguirDatosIdEd()
    {

        if (int.TryParse(edadInput.text, out int edad))
        {
            datosJugador.edad = edad;
        }
        else
        {
            Debug.LogWarning("La edad introducida no es válida");
            datosJugador.edad = 0;
        }

        if (int.TryParse(idInput.text, out int id))
        {
            datosJugador.id = id;
        }
        else
        {
            Debug.LogWarning("El identificador introducida no es válido");
            datosJugador.id = 0;
        }
    }

    public void ConseguirDatosAltPes()
    {

        if (int.TryParse(alturaInput.text, out int altura))
        {
            if (altura < 100)
            {
                Debug.LogWarning("Altura demasiado baja, se asigna por defecto: 171");
                datosJugador.altura = 171;
            }
            else
            {
                datosJugador.altura = altura;
            }
        }
        else
        {
            Debug.LogWarning("La altura introducida no es válida");
            datosJugador.altura = 171;
        }

        AjustarDistanciaFruta();

        if (int.TryParse(pesoInput.text, out int peso))
        {
            datosJugador.peso = peso;
        }
        else
        {
            Debug.LogWarning("El peso introducido no es válido");
            datosJugador.peso = 0;
        }

    }

    public void AjustarDistanciaFruta()
    {
        int diferenciaAltura = 171 - datosJugador.altura;

        manzanas.DistanciaAtraccion += diferenciaAltura * 0.012f;
        mangos.DistanciaAtraccion += diferenciaAltura * 0.006f;
        platanos.DistanciaAtraccion += diferenciaAltura * 0.009f;
        naranjas.DistanciaAtraccion += diferenciaAltura * 0.009f;
        limones.DistanciaAtraccion += diferenciaAltura * 0.012f;

        Debug.Log("Distancias ajustadas según la altura del jugador.");
    }


    public void ConseguirDatosHombDerech()
    {
        datosJugador.hombro = "Derecho";
    }

    public void ConseguirDatosHombIzdo()
    {
        datosJugador.hombro = "Izquierdo";
    }

    public void ConseguirDatoDolor()
    {
        datosJugador.dolor = (int)SliderDolor.value;
    }

    public void MenorNivel()
    {
        datosJugador.ajusteNivel = "Mejor un nivel más bajo";
    }

    public void MismoNivel()
    {
        datosJugador.ajusteNivel = "Este nivel está bien";
    }

    public void MayorNivel()
    {
        datosJugador.ajusteNivel = "Puedo con un nivel superior";
    }

    public void GuardarEnExcel()
    {
        System.TimeSpan tiempoDeJuego = System.DateTime.Now - comienzajuego;
        datosJugador.tiempoJuego = tiempoDeJuego;

        int fcMin = frecuenciaCardiaca.ObtenerMinima();
        int fcMax = frecuenciaCardiaca.ObtenerMaxima();
        float fcMedia = frecuenciaCardiaca.ObtenerMedia();

        if (fcMin <= 0 || fcMax <= 0 || fcMedia <= 0f)
        {
            fcMin = 80;
            fcMax = 120;
            fcMedia = 100f;
        }

        datosJugador.fcMin = fcMin;
        datosJugador.fcMax = fcMax;
        datosJugador.fcMedia = fcMedia;

        int seriesCompletadas = 0;

        if (manzanas.EstaCompleto()) seriesCompletadas++;
        if (mangos.EstaCompleto()) seriesCompletadas++;
        if (platanos.EstaCompleto()) seriesCompletadas++;
        if (naranjas.EstaCompleto()) seriesCompletadas++;
        if (limones.EstaCompleto()) seriesCompletadas++;

        datosJugador.series = seriesCompletadas;

        int totalReps = manzanas.GetReps() + mangos.GetReps() + platanos.GetReps() + naranjas.GetReps() + limones.GetReps();
        datosJugador.repeticiones = totalReps;

        int totalMax = (manzanas.TotalObjetos() + mangos.TotalObjetos() + platanos.TotalObjetos() + naranjas.TotalObjetos() + limones.TotalObjetos());
        datosJugador.tareaCompletada = (float)totalReps / totalMax * 100f;

        datosJugador.ejercicioRapido = contadorEjercicioRapido;
        datosJugador.ejercicioBien = datosJugador.repeticiones - datosJugador.ejercicioRapido;

        

        editarexcel.AñadirFila(datosJugador);
    }

    private void OnApplicationQuit()
    {
        GuardarEnExcel();
    }
}
