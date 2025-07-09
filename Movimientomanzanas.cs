using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Movimientomanzanas : MonoBehaviour, IMagnetico
{
    public Transform handTransform;
    public Transform Pivote_cesta;

    public Transform ManzanasArbol;
    public Transform ManzanasEnCesta;

    public float DistanciaAtraccion = 1.5f;
    public float VelocidadMovimiento = 4f;
    public float DistanciaCesta = 0.2f;

    public TextMeshProUGUI TextoContadorManzanas;

    private List<GameObject> manzanasObjeto = new List<GameObject>();
    private List<GameObject> manzanasCesta = new List<GameObject>();

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
        if (ManzanasArbol != null)
        {
            foreach (Transform child in ManzanasArbol)
            {
                manzanasObjeto.Add(child.gameObject);
            }
        }

        if (ManzanasEnCesta != null)
        {
            foreach (Transform child in ManzanasEnCesta)
            {
                manzanasCesta.Add(child.gameObject);
            }
        }

        UpdatemanzanasCounter();

        if (manzanasObjeto.Count > 0)
        {
            ActivateObject(IndiceActual); 
        }
    }

    void Update()
    {
        if (handTransform == null || IndiceActual >= manzanasObjeto.Count)
            return;

        GameObject currentObj = manzanasObjeto[IndiceActual];
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
        GameObject manzanacestaactual = manzanasCesta[IndiceActual];
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
        UpdatemanzanasCounter();

        MoverACesta = false;

        if (IndiceActual < manzanasObjeto.Count)
        {
            ActivateObject(IndiceActual);
            MoverAMano = true;
        }
    }

    void ActivateObject(int index)
    {
        if (index < manzanasObjeto.Count && manzanasObjeto[index] != null)
        {
            manzanasObjeto[index].SetActive(true);
        }
    }

    public void IniciarMagnetismo()
    {
        MoverAMano = true;
    }

    void UpdatemanzanasCounter()
    {
        if (TextoContadorManzanas != null)
        {
            TextoContadorManzanas.text = (manzanasObjeto.Count - IndiceActual).ToString();
        }
    }

    public bool EstaEsperandoNuevoObjeto()
    {
        return !MoverAMano && !SujetoEnMano && !MoverACesta;
    }

    public bool EstaCompleto()
    {
        return IndiceActual >= manzanasObjeto.Count;
    }

    public int GetReps()
    {
        return IndiceActual;
    }

    public int TotalObjetos()
    {
        return manzanasObjeto.Count;
    }
}
