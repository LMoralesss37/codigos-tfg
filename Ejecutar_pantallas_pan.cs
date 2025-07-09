using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class EjecutarPantallasPan : MonoBehaviour
{
    public GameObject pantalla1;
    public GameObject pantalla2;
    public GameObject pantalla3;
    public GameObject pantalla4;
    public GameObject pantalla5;
    public GameObject pantalla_eventos;
    public GameObject pantalla_eventoInicial;
    public GameObject canvas_pantallainit;
    public GameObject canvas_primpantallaintermedia;
    public GameObject canvas_segpantallaintermedia;

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

    public void IrAPantallaEvento()
    {
        MostrarPantalla(6);
    }

    public void IrAPantallaEvento_inicial()
    {
        MostrarPantalla(7);
    }

    public void ElegirHombroIzquierdo()
    {
        canvas_pantallainit.SetActive(false);
        MostrarPantalla(5);
    }

    public void ElegirHombroDerecho()
    {
        canvas_pantallainit.SetActive(false);
        MostrarPantalla(5);
    }

    public void IrAPantallaEvento_intermedio()
    {
        canvas_primpantallaintermedia.SetActive(false);
        MostrarPantalla(8);
    }

    private void MostrarPantalla(int numero)
    {
        pantalla1.SetActive(numero == 1);
        pantalla2.SetActive(numero == 2);
        pantalla3.SetActive(numero == 3);
        pantalla4.SetActive(numero == 4);
        pantalla5.SetActive(numero == 5);
        pantalla_eventos.SetActive(numero == 6);
        pantalla_eventoInicial.SetActive(numero == 7);
        canvas_segpantallaintermedia.SetActive(numero == 8);
    }
}

