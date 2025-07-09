using System;
using System.IO;
using UnityEngine;

public class EditarExcel : MonoBehaviour
{
    private string RutaArchivo;

    void Awake()
    {
        RutaArchivo = Path.Combine(Application.persistentDataPath, "datos.csv");

        if (!File.Exists(RutaArchivo))
        {
            CrearCabecera();
        }
    }

    void CrearCabecera()
    {
        string[] headers = {
            "ID", "Edad", "Altura", "Peso", "Articulación", "Descripción de la tarea",
            "Nivel de dificultad", "Hombro a rehabilitar", "FC mínima", "FC máxima", "FC media",
            "Fecha de conexión", "Hora de conexión", "Tiempo de juego", "% tarea completado",
            "Series", "Repeticiones completas", "Repeticiones rápidas", "Repeticiones bien", "Ajuste de nivel", "Dolor"
        };

        File.AppendAllText(RutaArchivo, string.Join(",", headers) + Environment.NewLine);
        Debug.Log("Cabecera creada en CSV");
    }

    public void AñadirFila(DatosExcel datos)
    {
        string[] fila = {
            datos.id.ToString(),
            datos.edad.ToString(),
            datos.altura.ToString(),
            datos.peso.ToString(),
            datos.articulacion.ToString(),
            datos.descripcionTarea.ToString(),
            datos.nivelDificultad.ToString(),
            datos.hombro,
            datos.fcMin.ToString(),
            datos.fcMax.ToString(),
            datos.fcMedia.ToString(),
            datos.fechaConexion.ToString("yyyy-MM-dd"),
            datos.horaConexion.ToString(),
            datos.tiempoJuego.ToString(),
            datos.tareaCompletada.ToString(),
            datos.series.ToString(),
            datos.repeticiones.ToString(),
            datos.ejercicioRapido.ToString(),
            datos.ejercicioBien.ToString(),
            datos.ajusteNivel,
            datos.dolor.ToString()
        };

        File.AppendAllText(RutaArchivo, string.Join(",", fila) + Environment.NewLine);
        Debug.Log("Fila añadida en CSV");
    }
}
