using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MoverOVRpersonajePan : MonoBehaviour
{
    public Transform Camino;
    private List<Transform> PuntosCamino;

    [HideInInspector]
    public int totalPuntos;

    public List<int> PuntosEvento;
    public GameObject CanvaEvento;
    public List<int> PuntosSinParar;

    public float velocidad = 2f;

    private int IndicePuntoActual = 0;
    private bool Esperando = false;
    private bool Empezar = false;

    private bool botonCabezaPresionado = false;
    private bool botonPiesPresionado = false;

    private enum EstadoInteraccion
    {
        EsperandoPrimerPies,
        EsperandoCabeza,
        EsperandoSegundoPies
    }

    private EstadoInteraccion estadoInteraccion = EstadoInteraccion.EsperandoPrimerPies;

    public GameObject canvasBotonSuperior;
    public GameObject canvasBotonInferior;
    public Button botonCabeza;
    public Button botonPies;

    public GameObject pantallaEventoInicial;
    public GameObject PrimeraPantallaIntermediaPan;
    public GameObject SegundaPantallaIntermediaPan;

    public List<GameObject> CanvasPan;
    public List<GameObject> Pan;
    private int indiceEventoActual = -1;
    private bool EventoInicial = true;

    public GameObject videoPlayerCanvas;
    public VideoPlayer videoPlayer;
    private RawImage rawImage;

    public PantallasFinales pantallasFinales;
    private bool finalMostrado = false;

    [HideInInspector]
    public int ContadorSeries = 0;
    [HideInInspector]
    public int ContadorRepeticiones = 0;

    public AudioSource audioSource;
    public float TiempoMinimoEjercicio = 5f;
    [HideInInspector]
    public int contadorEjercicioRapido = 0;

    private float tiempoInicioPulsacion = -1f;

    void Start()
    {
        PuntosCamino = new List<Transform>();
        foreach (Transform child in Camino)
        {
            PuntosCamino.Add(child);
        }
        totalPuntos = PuntosCamino.Count;

        if (CanvaEvento != null)
        {
            CanvaEvento.SetActive(false);
        }

        if (videoPlayerCanvas != null)
        {
            videoPlayerCanvas.SetActive(false);
            rawImage = videoPlayerCanvas.GetComponentInChildren<RawImage>();
        }
    }

    void Update()
    {
        if (!Empezar || Esperando)
            return;

        if (IndicePuntoActual >= PuntosCamino.Count)
        {
            if (!finalMostrado)
            {
                DesaparecerBotonesSupInf();
                pantallasFinales.MostrarSiguientePantalla();
                finalMostrado = true;
            }
            return;
        }

        MoveTowardsPoint(PuntosCamino[IndicePuntoActual]);

        if (Vector3.Distance(transform.position, PuntosCamino[IndicePuntoActual].position) < 0.1f && !Esperando)
        {
            if (PuntosSinParar.Contains(IndicePuntoActual))
            {
                IndicePuntoActual++;
                return;
            }

            Esperando = true;
            indiceEventoActual = PuntosEvento.IndexOf(IndicePuntoActual);

            if (indiceEventoActual != -1)
            {
                StartCoroutine(HandleSpecialPoint());
            }
            else
            {
                StartCoroutine(HandleRegularStop());
            }
        }
    }

    void MoveTowardsPoint(Transform targetPoint)
    {
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f); 
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, velocidad * Time.deltaTime);
    }


    IEnumerator HandleSpecialPoint()
    {
        botonCabezaPresionado = false;
        botonPiesPresionado = false;
        estadoInteraccion = EstadoInteraccion.EsperandoPrimerPies;

        DesaparecerBotonesSupInf();
        if (CanvaEvento != null)
        {
            CanvaEvento.SetActive(true);
        }

        yield return new WaitUntil(() => botonCabezaPresionado && botonPiesPresionado);

        ContadorRepeticiones++;

        yield return new WaitForSeconds(1.5f);
        IndicePuntoActual++;

        ResetearColoresBotones();
        botonPies.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "1";

        estadoInteraccion = EstadoInteraccion.EsperandoPrimerPies;
        Esperando = false;
    }

    IEnumerator HandleRegularStop()
    {
        botonCabezaPresionado = false;
        botonPiesPresionado = false;
        estadoInteraccion = EstadoInteraccion.EsperandoPrimerPies;

        yield return new WaitUntil(() => botonCabezaPresionado && botonPiesPresionado);

        ContadorRepeticiones++;

        yield return new WaitForSeconds(1.5f);
        IndicePuntoActual++;

        ResetearColoresBotones();
        botonPies.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "1";

        estadoInteraccion = EstadoInteraccion.EsperandoPrimerPies;
        Esperando = false;
    }

    public void PresionarBotonCabeza()
    {
        if (estadoInteraccion == EstadoInteraccion.EsperandoCabeza)
        {
            botonCabeza.GetComponent<Image>().color = Color.green;
            botonCabezaPresionado = true;
            estadoInteraccion = EstadoInteraccion.EsperandoSegundoPies;
        }
    }

    public void PresionarBotonPies()
    {
        if (estadoInteraccion == EstadoInteraccion.EsperandoPrimerPies)
        {
            botonPies.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "3";
            tiempoInicioPulsacion = Time.time;
            estadoInteraccion = EstadoInteraccion.EsperandoCabeza;
        }
        else if (estadoInteraccion == EstadoInteraccion.EsperandoSegundoPies && botonCabezaPresionado)
        {
            float tiempoTranscurrido = Time.time - tiempoInicioPulsacion;

            if (tiempoTranscurrido < TiempoMinimoEjercicio && audioSource != null)
            {
                audioSource.Play();
                contadorEjercicioRapido++;
            }

            botonPies.GetComponent<Image>().color = Color.green;
            botonPiesPresionado = true;
        }
    }

    private void ResetearColoresBotones()
    {
        botonCabeza.GetComponent<Image>().color = Color.white;
        botonPies.GetComponent<Image>().color = Color.white;
    }

    public void AparecerCanvasPan()
    {
        if (indiceEventoActual != -1 && indiceEventoActual < CanvasPan.Count)
        {
            CanvasPan[indiceEventoActual].SetActive(true);
            CanvaEvento.SetActive(false);
        }
    }

    private IEnumerator MostrarPanYEsperar()
    {
        if (indiceEventoActual != -1 && indiceEventoActual < Pan.Count && indiceEventoActual < CanvasPan.Count)
        {
            Pan[indiceEventoActual].SetActive(true);
            CanvasPan[indiceEventoActual].SetActive(false);
        }

        yield return new WaitForSeconds(2f);

        if (PrimeraPantallaIntermediaPan != null)
            PrimeraPantallaIntermediaPan.SetActive(true);
    }

    public void IniciarMostrarPan()
    {
        StartCoroutine(MostrarPanYEsperar());
        ContadorSeries++;
    }

    public void AparecerBotonesSupInf()
    {
        canvasBotonSuperior.SetActive(true);
        canvasBotonInferior.SetActive(true);
    }

    public void DesaparecerBotonesSupInf()
    {
        canvasBotonInferior.SetActive(false);
        canvasBotonSuperior.SetActive(false);
    }

    public void PantallaEventoInicial()
    {
        pantallaEventoInicial.SetActive(false);
        StartCoroutine(PlayVideoSequence());
    }

    public void ConfirmarVideo()
    {
        SegundaPantallaIntermediaPan.SetActive(false);
        StartCoroutine(PlayVideoSequence());
    }

    public void ConfirmarSinVideo()
    {
        SegundaPantallaIntermediaPan.SetActive(false);
        AparecerBotonesSupInf();
    }

    private IEnumerator PlayVideoSequence()
    {
        videoPlayerCanvas.SetActive(true);

        yield return null;

        if (videoPlayer != null && rawImage != null)
        {
            if (videoPlayer.targetTexture != null)
                rawImage.texture = videoPlayer.targetTexture;

            videoPlayer.Play();

            yield return new WaitUntil(() => videoPlayer.isPlaying);
            yield return new WaitUntil(() => !videoPlayer.isPlaying);
        }
        else
        {
            Debug.LogError("VideoPlayer o RawImage no asignado.");
        }

        videoPlayerCanvas.SetActive(false);
        yield return new WaitForSeconds(2f);

        AparecerBotonesSupInf();

        if (EventoInicial == true)
        {
            Empezar = true;
            EventoInicial = false;
        }
    }
}






