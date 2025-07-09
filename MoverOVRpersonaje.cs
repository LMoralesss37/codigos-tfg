using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class MoverOVRpersonaje : MonoBehaviour
{
    public Transform Camino;
    private List<Transform> PuntosCamino;

    public List<int> IndicesPuntosEvento;

    public Transform canvasExplicacionEjercicios;
    private List<GameObject> canvasEjercicio = new List<GameObject>();
    public GameObject pantallaIntermediaFruta;

    public List<MonoBehaviour> ScriptsMovimientosFruta;
    private List<IMagnetico> magneticMoveScripts = new List<IMagnetico>();

    public List<TextMeshProUGUI> contadores;

    public GameObject PivoteCesta;

    public GameObject videoPlayerCanvas;
    public VideoPlayer videoPlayer;
    private RawImage rawImage;

    public PantallasFinales pantallasFinales;
    private bool finalMostrado = false;

    public float VelocidadPersonaje = 2f;

    private int IndicePuntoActual = 0;
    private bool Esperando = false;
    private bool Empezar = false;
    private int IndiceEventoActual = -1;

    private bool EventoConfirmado = false;


    void Start()
    {
        PuntosCamino = new List<Transform>();
        foreach (Transform child in Camino)
        {
            PuntosCamino.Add(child);
        }

        if (canvasExplicacionEjercicios != null)
        {
            foreach (Transform child in canvasExplicacionEjercicios)
            {
                canvasEjercicio.Add(child.gameObject);
            }
        }

        foreach (var comp in ScriptsMovimientosFruta)
        {
            IMagnetico m = comp as IMagnetico;
            if (m != null)
                magneticMoveScripts.Add(m);
            else
                Debug.LogError($"{comp.name} no implementa IMagnetico. Asegúrate de que tus scripts de magnetismo implementen la interfaz IMagnetico.");
        }

        if (IndicesPuntosEvento.Count != canvasEjercicio.Count ||
            IndicesPuntosEvento.Count != magneticMoveScripts.Count ||
            IndicesPuntosEvento.Count != contadores.Count)
        {
            Debug.LogError("Listas desincronizadas: revisa IndicesPuntosEvento, canvases, scripts y contadores. Deben tener el mismo número de elementos.");
        }

        if (PivoteCesta != null)
            PivoteCesta.SetActive(false);

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
                pantallasFinales.MostrarSiguientePantalla();
                finalMostrado = true;
            }
            return;
        }

        MoveTowardsPoint(PuntosCamino[IndicePuntoActual]);

        if (Vector3.Distance(transform.position, PuntosCamino[IndicePuntoActual].position) < 0.1f)
        {
            int specialIndex = IndicesPuntosEvento.IndexOf(IndicePuntoActual);
            if (specialIndex != -1)
            {
                IndiceEventoActual = specialIndex;
                StartCoroutine(HandleSpecialEvent(specialIndex));
            }
            else
            {
                IndicePuntoActual++;
            }
        }
    }


    public void ComenzarMovimiento()
    {
        Empezar = true;
    }

    void MoveTowardsPoint(Transform targetPoint)
    {
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0; 

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f); 
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, VelocidadPersonaje * Time.deltaTime);
    }


    IEnumerator HandleSpecialEvent(int index)
    {
        Esperando = true;

        if (canvasEjercicio[index] != null)
            canvasEjercicio[index].SetActive(true);

        yield return new WaitUntil(() => EventoConfirmado);
        EventoConfirmado = false;

        if (magneticMoveScripts[index] != null)
            magneticMoveScripts[index].IniciarMagnetismo();

        if (PivoteCesta != null)
            PivoteCesta.SetActive(true);

        if (contadores[index] != null)
        {
            yield return new WaitUntil(() => contadores[index].text.Trim() == "0");
        }

        if (PivoteCesta != null)
            PivoteCesta.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        IndicePuntoActual++;
        Esperando = false;
    }

    public void CambiarPantallaIntermedia()
    {
        if (IndiceEventoActual != -1)
        {
            if (canvasEjercicio[IndiceEventoActual] != null)
                canvasEjercicio[IndiceEventoActual].SetActive(false);
                pantallaIntermediaFruta.SetActive(true);
        }
    }

    public void PantallaEventoInicial()
    {
        if (IndiceEventoActual != -1)
        {
            if (canvasEjercicio[IndiceEventoActual] != null)
                canvasEjercicio[IndiceEventoActual].SetActive(false);

            StartCoroutine(PlayVideoSequence());
        }
    }

    public void ConfirmarEvento()
    {
        pantallaIntermediaFruta.SetActive(false);    
        StartCoroutine(PlayVideoSequence());
    }

    public void ConfirmarSinVideo()
    {
        pantallaIntermediaFruta.SetActive(false);
        EventoConfirmado = true;
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
        EventoConfirmado = true;
    }

}







