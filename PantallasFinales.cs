using UnityEngine;

public class PantallasFinales : MonoBehaviour
{
    public GameObject pantallaFinal1;
    public GameObject pantallaFinal2;
    public GameObject pantallaFinal3;
    public GameObject pantallaFinal4;
    public GameObject canvasFinal;

    private int pantallaActual = 0;

    public void MostrarSiguientePantalla()
    {
        pantallaActual++;

        pantallaFinal1.SetActive(false);
        pantallaFinal2.SetActive(false);
        pantallaFinal3.SetActive(false);
        pantallaFinal4.SetActive(false);

        switch (pantallaActual)
        {
            case 1:
                pantallaFinal1.SetActive(true);
                break;
            case 2:
                canvasFinal.SetActive(true);
                pantallaFinal2.SetActive(true);
                break;
            case 3:
                pantallaFinal3.SetActive(true);
                break;
            case 4:
                pantallaFinal4.SetActive(true);
                break;
            case 5:
                SalirDelJuego();
                break;
        }
    }

    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

