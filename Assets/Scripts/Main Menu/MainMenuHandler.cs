using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] NetworkRunnerHandler _networkHandler;

    [Header("Panels")]
    [SerializeField] GameObject _initialPanel;
    [SerializeField] SessionListHandler _sessionListHandler;
    [SerializeField] GameObject _hostGamePanel;
    [SerializeField] GameObject _statusPanel;

    [Header("Buttons")]
    [SerializeField] Button _joinLobbyBTN;
    [SerializeField] Button _openHostPanelBTN;
    [SerializeField] Button _hostGameBTN;

    [Header("Inputfields")]
    [SerializeField] InputField _hostSessionName;

    [Header("Texts")]
    [SerializeField] Text _statusText;


    void Start()
    {
        //A cada boton que tenemos le agregamos por codigo el metodo que deberian ejecutar cuando son clickeados

        _joinLobbyBTN.onClick.AddListener(BTN_JoinLobby);

        _openHostPanelBTN.onClick.AddListener(BTN_ShowHostPanel);

        _hostGameBTN.onClick.AddListener(BTN_CreateGameSession);

        //Cuando el Network Runner se termine de conectar a un Lobby
        //Le decimos mediante la suscripcion al evento que apague el Panel de Estado y prenda el Browser

        _networkHandler.OnJoinedLobby += () =>
        {
            _statusPanel.SetActive(false);
            _sessionListHandler.gameObject.SetActive(true);
        };
    }

    #region Buttons Methods


    //Cuando se clickea en el boton de entrar a un lobby:
    void BTN_JoinLobby()
    {
        //Le pedimos al network handler que genere ese ingreso al Lobby mediante el NetworkRunner
        _networkHandler.JoinLobby();

        //Apagamos el panel inicial que contenia el boton de entrar a un lobby
        _initialPanel.SetActive(false);

        //Prendemos el panel de estado
        _statusPanel.SetActive(true);

        _statusText.text = "Joining Lobby...";
    }

    //Cuando se clickea en el boton de mostrar el menu para crear una sala:
    void BTN_ShowHostPanel()
    {
        //Apagamos el Browser de sesiones
        _sessionListHandler.gameObject.SetActive(false);

        //Prendemos el panel necesario
        _hostGamePanel.SetActive(true);
    }

    //Cuando se clickea en el boton de crear una sala
    void BTN_CreateGameSession()
    {
        //Le pedimos al network handler que cree la sesion en la que vamos a ser Host
        _networkHandler.CreateSession(_hostSessionName.text, "CardGame");
    }

    #endregion


}
