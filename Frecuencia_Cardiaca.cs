using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using HypeRate;

public class Frecuencia_Cardiaca : MonoBehaviour
{
    public string websocketToken = "<Request your Websocket Token>";
    public string hyperateURL = "wss://app.hyperate.io/socket/websocket";
    public string hyperateID = "internal-testing";
    private string hr = "";

    [Serializable]
    public class HypeRateDataPackagePayload
    {
        public string hr;
        public string status;
    }

    [Serializable]
    public class HypeRateDataPackage
    {
        public string @event;
        public HypeRateDataPackagePayload payload;
    }

    public TMP_Text textBox;

    HypeRate.HypeRate hypeRateSocket;

    private List<int> listaFrecuencias = new List<int>();

    async void Start()
    {
        hypeRateSocket = HypeRate.HypeRate.GetInstance();
        await hypeRateSocket.ConnectToServer(websocketToken, hyperateURL);
        await hypeRateSocket.JoinHeartbeatChannel(hyperateID);
        hypeRateSocket.onMessageReceivedCallback = this.ChangeText;
    }

    private void Update()
    {
        textBox.text = hr;
    }

    private void ChangeText(string message)
    {
        HypeRateDataPackage datapackage = JsonUtility.FromJson<HypeRateDataPackage>(message);
        hr = datapackage.payload.hr;

        if (int.TryParse(hr, out int frecuenciaActual))
        {
            listaFrecuencias.Add(frecuenciaActual);
        }
    }

    private async void OnApplicationQuit()
    {
        await hypeRateSocket.CloseConnection();
    }


    public int ObtenerMinima()
    {
        return listaFrecuencias.Count > 0 ? Mathf.Min(listaFrecuencias.ToArray()) : 0;
    }

    public int ObtenerMaxima()
    {
        return listaFrecuencias.Count > 0 ? Mathf.Max(listaFrecuencias.ToArray()) : 0;
    }

    public float ObtenerMedia()
    {
        if (listaFrecuencias.Count == 0) return 0;

        float suma = 0;
        foreach (int valor in listaFrecuencias)
            suma += valor;

        return suma / listaFrecuencias.Count;
    }
}


