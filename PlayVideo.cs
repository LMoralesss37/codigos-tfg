using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayVideoAfterDelay : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public float RetrasoAntesReproduccion = 10f;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer == null)
        {
            Debug.LogError("No se encontró un componente VideoPlayer en este GameObject.");
            return;
        }

        videoPlayer.playOnAwake = false;
        videoPlayer.Stop();
        videoPlayer.Prepare();

        videoPlayer.loopPointReached += OnVideoFinished;

        StartCoroutine(WaitAndPlay());
    }

    IEnumerator WaitAndPlay()
    {
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        yield return new WaitForSeconds(RetrasoAntesReproduccion);

        videoPlayer.Play();
        Debug.Log("Vídeo reproducido tras " + RetrasoAntesReproduccion + " segundos.");
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Vídeo terminado. Cargando escena 'Menu'...");
        SceneManager.LoadScene("Menu");
    }
}

