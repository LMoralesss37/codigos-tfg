using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movimientomangos : MonoBehaviour, IMagnetico
{
    public Transform handTransform;
    public Transform Pivote_cesta;

    public Transform mangosArbol;
    public Transform mangosEnCesta;

    public float DistanciaAtraccion = 1.5f;
    public float VelocidadMovimiento = 4f;
    public float DistanciaCesta = 0.2f;

    public TextMeshProUGUI TextoContadormangos;

    private List<GameObject> mangosObjeto = new List<GameObject>();
    private List<GameObject> mangosCesta = new List<GameObject>();

    private int IndiceActual = 0;
    private bool MoverAMano = false;
    private bool SujetoEnMano = false;
    private bool MoverACesta = false;

    public float TiempoMinimoMovimiento = 5f;
    public AudioSource audioEjercicioLento;
    private float tiempoUltimaEntrega = -1f;

    public GuardarDatos guardarDatos;

    void Start()
    {
        if (mangosArbol != null)
        {
            foreach (Transform child in mangosArbol)
            {
                mangosObjeto.Add(child.gameObject);
            }
        }

        if (mangosEnCesta != null)
        {
            foreach (Transform child in mangosEnCesta)
            {
                mangosCesta.Add(child.gameObject);
            }
        }

        UpdatemangosCounter();

        if (mangosObjeto.Count > 0)
        {
            ActivateObject(IndiceActual);
        }
    }

    void Update()
    {
        if (handTransform == null || IndiceActual >= mangosObjeto.Count)
            return;

        GameObject currentObj = mangosObjeto[IndiceActual];
        if (currentObj == null) return;

        if (MoverAMano)
        {
            float distance = Vector3.Distance(currentObj.transform.position, handTransform.position);

            if (distance < 0.05f)
            {
                MoverAMano = false;
                SujetoEnMano = true;
            }
            else if (distance < DistanciaAtraccion)
            {
                currentObj.transform.position = Vector3.MoveTowards(
                    currentObj.transform.position,
                    handTransform.position,
                    VelocidadMovimiento * Time.deltaTime
                );
            }
        }
        else if (SujetoEnMano)
        {
            currentObj.transform.position = handTransform.position;
            currentObj.transform.rotation = handTransform.rotation;

            float distanceToBox = Vector3.Distance(currentObj.transform.position, Pivote_cesta.position);
            if (distanceToBox < DistanciaCesta)
            {
                SujetoEnMano = false;
                StartCoroutine(SnapToBox(currentObj));
            }
        }
    }

    IEnumerator SnapToBox(GameObject obj)
    {
        GameObject manzanacestaactual = mangosCesta[IndiceActual];
        MoverACesta = true;

        while (Vector3.Distance(obj.transform.position, Pivote_cesta.position) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(
                obj.transform.position,
                Pivote_cesta.position,
                VelocidadMovimiento * Time.deltaTime
            );
            obj.transform.rotation = Pivote_cesta.rotation;
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        obj.SetActive(false);
        manzanacestaactual.SetActive(true);

        float tiempoAhora = Time.time;
        if (tiempoUltimaEntrega > 0f)
        {
            float diferencia = tiempoAhora - tiempoUltimaEntrega;
            if (diferencia < TiempoMinimoMovimiento && audioEjercicioLento != null)
            {
                audioEjercicioLento.Play();
                if (guardarDatos != null)
                {
                    guardarDatos.contadorEjercicioRapido++;
                }
            }
        }
        tiempoUltimaEntrega = tiempoAhora;

        IndiceActual++;
        UpdatemangosCounter();

        MoverACesta = false;

        if (IndiceActual < mangosObjeto.Count)
        {
            ActivateObject(IndiceActual);
            MoverAMano = true;
        }
    }

    void ActivateObject(int index)
    {
        if (index < mangosObjeto.Count && mangosObjeto[index] != null)
        {
            mangosObjeto[index].SetActive(true);
        }
    }

    public void IniciarMagnetismo()
    {
        MoverAMano = true;
    }

    void UpdatemangosCounter()
    {
        if (TextoContadormangos != null)
        {
            TextoContadormangos.text = (mangosObjeto.Count - IndiceActual).ToString();
        }
    }

    public bool EstaEsperandoNuevoObjeto()
    {
        return !MoverAMano && !SujetoEnMano && !MoverACesta;
    }

    public bool EstaCompleto()
    {
        return IndiceActual >= mangosObjeto.Count;
    }

    public int GetReps()
    {
        return IndiceActual;
    }

    public int TotalObjetos()
    {
        return mangosObjeto.Count;
    }
}



