using UnityEngine;

public class CambioEscena : MonoBehaviour
{
    public GameObject Activar; 
    public GameObject Inactivar; 

    public void ActivarTarea()
    {
        Inactivar.SetActive(false); 
        Activar.SetActive(true); 
    }
}
