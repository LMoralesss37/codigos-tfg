using UnityEngine;
using UnityEngine.UI;

public class EjecutarPantallas : MonoBehaviour
{
    public GameObject pantalla1;
    public GameObject pantalla2;
    public GameObject pantalla3;
    public GameObject pantalla4;
    public GameObject pantalla5;
    public GameObject canvas_pantallainit;

    public Movimientomanzanas eventoManzanas;
    public Movimientomangos eventoMangos;
    public Movimientoplatanos eventoPlatanos;
    public Movimientonaranjas eventoNaranjas;
    public Movimientolimones eventoLimones;
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;

    private void Start()
    {
        MostrarPantalla(1);
    }

    public void IrAPantalla2()
    {
        MostrarPantalla(2);
    }

    public void IrAPantalla3()
    {
        MostrarPantalla(3);
    }

    public void IrAPantalla4()
    {
        MostrarPantalla(4);
    }

    public void ElegirHombroIzquierdo()
    {
        eventoManzanas.handTransform = leftHandAnchor;
        eventoMangos.handTransform = leftHandAnchor;
        eventoPlatanos.handTransform = leftHandAnchor;
        eventoNaranjas.handTransform = leftHandAnchor;
        eventoLimones.handTransform = leftHandAnchor;

        canvas_pantallainit.SetActive(false);
        MostrarPantalla(5);
    }

    public void ElegirHombroDerecho()
    {
        eventoManzanas.handTransform = rightHandAnchor;
        eventoMangos.handTransform = rightHandAnchor;
        eventoPlatanos.handTransform = rightHandAnchor;
        eventoNaranjas.handTransform = rightHandAnchor;
        eventoLimones.handTransform = rightHandAnchor;

        canvas_pantallainit.SetActive(false);
        MostrarPantalla(5);
    }

    public void IniciarMovimiento(GameObject objetoMovimiento)
    {
        MoverOVRpersonaje mover = objetoMovimiento.GetComponent<MoverOVRpersonaje>();
        if (mover != null)
        {
            mover.ComenzarMovimiento();
            pantalla5.SetActive(false);
        }
    }

    private void MostrarPantalla(int numero)
    {
        pantalla1.SetActive(numero == 1);
        pantalla2.SetActive(numero == 2);
        pantalla3.SetActive(numero == 3);
        pantalla4.SetActive(numero == 4);
        pantalla5.SetActive(numero == 5);
    }
}

