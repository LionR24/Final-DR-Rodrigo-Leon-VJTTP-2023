using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class SessionInfoItem : MonoBehaviour
{
    [SerializeField] Text _sessionNameText;
    [SerializeField] Text _playerCountText;
    [SerializeField] Button _joinButton;

    SessionInfo _sessionInfo;

    public event Action<SessionInfo> OnJoinSession;

    //Una vez recibimos la informacion de la sesion a la que este objeto va a pertenecer
    public void SetSessionInformation(SessionInfo session)
    {
        //La guardamos para usarla luego en caso de que se conecten a esta
        _sessionInfo = session;

        //Tomamos el nombre de la sesion
        _sessionNameText.text = _sessionInfo.Name;

        //Tomamos los players actuales y maximos que admite la sesion
        _playerCountText.text = $"{_sessionInfo.PlayerCount}/{_sessionInfo.MaxPlayers}";

        //En caso de que no haya vacantes para entrar a la sesion, no vamos a poner ver el boton de ingreso
        _joinButton.enabled = _sessionInfo.PlayerCount < _sessionInfo.MaxPlayers;
    }

    //Cuando el boton Join es clickeado
    public void OnClick()
    {
        //Se llama al evento y se pasa por parametro la sesion a la que pertenece el boton
        OnJoinSession?.Invoke(_sessionInfo);
    }
}
