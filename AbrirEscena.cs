using UnityEngine;
using UnityEngine.SceneManagement;

public class AbrirEscena : MonoBehaviour
{
    public int nivel;
    void Start()
    {
        
    }

    public void AbrirLaEscena() 
    {
        SceneManager.LoadScene("Nivel " + nivel.ToString());
    
    }
}
