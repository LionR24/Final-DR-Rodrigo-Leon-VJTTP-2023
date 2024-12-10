using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class SessionListHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkRunner;

    [SerializeField] Text _statusText;

    [SerializeField] SessionInfoItem _sessionPrefab;

    [SerializeField] VerticalLayoutGroup _verticalLayoutGroup;

    void OnEnable()
    {
        //Cuando se prende este panel, nos registramos al evento de sesiones que nos va a devolver todas las sesiones creadas en este Lobby
        _networkRunner.OnSessionListUpdate += ReceiveSessionList;
    }

    void OnDisable()
    {
        //Cuando se apaga este panel nos desuscribimos
        _networkRunner.OnSessionListUpdate -= ReceiveSessionList;
    }

    //Antes de modificar la lista de sesiones activa, limpiamos todas
    void ClearList()
    {
        foreach (Transform child in _verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        _statusText.gameObject.SetActive(false);
    }

    //Cuando nos llegan sesiones desde el evento:
    void ReceiveSessionList(List<SessionInfo> allSessions)
    {
        //Limpiamos la lista
        ClearList();

        //Si no hay ninguna sesion
        if (allSessions.Count == 0)
        {
            //Mostramos el mensaje por pantalla de que no se encontro ninguna
            NoSessionsFound();
        }
        else //Si hay sesiones en la lista
        {
            //Creamos un prefab para cada una
            foreach (var session in allSessions)
            {
                AddToList(session);
            }
        }
    }

    void NoSessionsFound()
    {
        _statusText.text = "No Sessions found";
        _statusText.gameObject.SetActive(true);
    }

    void AddToList(SessionInfo session)
    {
        var newSessionItem = Instantiate(_sessionPrefab, _verticalLayoutGroup.transform);
        newSessionItem.SetSessionInformation(session);
        newSessionItem.OnJoinSession += JoinSelectedSession;
    }

    //Cuando se clickee el boton de Join en una sesion del buscador,
    //se va a ejecutar este metodo ya que lo registramos al evento dentro del prefab creado
    void JoinSelectedSession(SessionInfo session)
    {
        //Le pedimos al network runner que nos conecte a la sesion elegida
        _networkRunner.JoinSession(session);
    }
}
