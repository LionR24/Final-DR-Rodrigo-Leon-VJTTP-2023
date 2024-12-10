using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputHandler : MonoBehaviour
{
    Unit _model;

    float _moveInput;
    bool _isJumpPressed;
    bool _isFirePressed;
   

    NetworkInputData _inputData;

    private void Start()
    {
        _model = GetComponent<Unit>();

        _inputData = new NetworkInputData();

        //if (_model.HasInputAuthority)
        //{
        //    //Llamar al UpdateManager y suscribirle FakeUpdate
        //}
    }

    private void Update()
    {
        if (!_model.HasInputAuthority) return;

        //_moveInput = Input.GetAxis("Horizontal");

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    _isJumpPressed = true;
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    _isFirePressed = true;
        //}

        if(Input.GetKeyDown(KeyCode.K))
        {
            GameManager.instance.ChangeTurn(_model);
        }
    }

    public NetworkInputData GetNetworkInputs()
    {
        _inputData.movementInput = _moveInput;

        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;

        _inputData.isJumpPressed = _isJumpPressed;
        _isJumpPressed = false;

        return _inputData;
    }
}
