using UnityEngine;
using TMPro;

public class GuardarDatosPan : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField edadInput;
    public TMP_InputField alturaInput;
    public TMP_InputField pesoInput;

    public GameObject BotonesMovimiento;
    private Vector3 posicionBotones;

    public MoverOVRpersonajePan moverOVR;
    private int PuntosTotales;

    public UnityEngine.UI.Slider SliderDolor;

    public GameObject RightAnchor;
    public GameObject LeftAnchor;

    private DatosExcel datosJugador = new DatosExcel();
    private EditarExcel editarexcel;
    private Frecuencia_Cardiaca frecuenciaCardiaca;
    private System.DateTime comienzajuego;

    void Start()
    {
        editarexcel = FindFirstObjectByType<EditarExcel>();
        frecuenciaCardiaca = FindFirstObjectByType<Frecuencia_Cardiaca>();
        datosJugador.articulacion = "Hombro";
        datosJugador.descripcionTarea = "Ejercicio de ABD-ADD plano escapular con goma";
        datosJugador.nivelDificultad = 3;
        datosJugador.fechaConexion = System.DateTime.Now;
        datosJugador.horaConexion = System.DateTime.Now.TimeOfDay;
        comienzajuego = System.DateTime.Now;

        posicionBotones = BotonesMovimiento.transform.position;
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
            datosJugador.altura = altura;
        }
        else
        {
            Debug.LogWarning("La altura introducida no es válida");
            datosJugador.altura = 0;
        }

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

    public void ConseguirDatosHombDerech()
    {
        datosJugador.hombro = "Derecho";
        posicionBotones.x -= 0.7f;
        posicionBotones.z -= 0.2f;
        BotonesMovimiento.transform.position = posicionBotones;
        BotonesMovimiento.transform.rotation = Quaternion.Euler(0f, -110f, 0f);
        LeftAnchor.SetActive(false);
    }

    public void ConseguirDatosHombIzdo()
    {
        datosJugador.hombro = "Izquierdo";
        posicionBotones.x += 0.7f;
        posicionBotones.z += 0.2f;
        BotonesMovimiento.transform.position = posicionBotones;
        BotonesMovimiento.transform.rotation = Quaternion.Euler(0f, 110f, 0f);
        RightAnchor.SetActive(false);
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

        datosJugador.series = moverOVR.ContadorSeries;
        datosJugador.repeticiones = moverOVR.ContadorRepeticiones;

        datosJugador.ejercicioRapido = moverOVR.contadorEjercicioRapido;
        datosJugador.ejercicioBien = datosJugador.repeticiones - datosJugador.ejercicioRapido;

        PuntosTotales = moverOVR.totalPuntos;
        int puntosSinParar = moverOVR.PuntosSinParar.Count;
        int puntosValidos = PuntosTotales - puntosSinParar;

        float porcentajeCompletado = 0f;
        if (puntosValidos > 0)
        {
            porcentajeCompletado = ((float)moverOVR.ContadorRepeticiones / puntosValidos) * 100f;
        }

        datosJugador.tareaCompletada = porcentajeCompletado;

        editarexcel.AñadirFila(datosJugador);
    }

    private void OnApplicationQuit()
    {
        GuardarEnExcel();
    }
}
