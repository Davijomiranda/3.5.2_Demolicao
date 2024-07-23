using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class SocketServer : MonoBehaviour
{
    private TcpListener _server;
    private bool _isRunning;
    public UnityEvent playGame;
    private void Start()
    {
        StartServer();
    }

    void StartServer()
    {
        _server = new TcpListener(IPAddress.Any, 5005);
        _server.Start();
        _isRunning = true;
        Debug.Log("Servidor de socket iniciado...");
        ListenForClients();
    }

    async void ListenForClients()
    {
        while (_isRunning)
        {
            var client = await _server.AcceptTcpClientAsync();
            var stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log("Mensagem recebida do script Python: " + message);
            //Aqui vai montar o avatar e depois iniciar o jogo
            var appConfig = GetComponent<AppConfig>();
            appConfig.SetIdBracelet(int.Parse(message));
            print(appConfig.idBracelet);
            var getter = GetComponent<GetterFromJson>();
            var braceletModel = getter.LoadJsonBracelet();
            appConfig.braceletModel = braceletModel;
            getter.TranslateTexts();
            playGame.Invoke();
            client.Close();
            if (Input.GetKeyDown(KeyCode.P))
            {
                client = await _server.AcceptTcpClientAsync();
                stream = client.GetStream();
                buffer = new byte[1024];
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Debug.Log("Mensagem recebida do script Python: " + message);
                //Aqui vai montar o avatar e depois iniciar o jogo
                appConfig = GetComponent<AppConfig>();
                appConfig.SetIdBracelet(50);
                print(appConfig.idBracelet);
                getter = GetComponent<GetterFromJson>();
                braceletModel = getter.LoadJsonBracelet();
                appConfig.braceletModel = braceletModel;
                getter.TranslateTexts();
                playGame.Invoke();
                client.Close();
            }
        }
    }

    void OnApplicationQuit()
    {
        _isRunning = false;
        _server.Stop();
    }

    public void ResetServer()
    {
        _isRunning = false;
        _server.Stop();
    }
}