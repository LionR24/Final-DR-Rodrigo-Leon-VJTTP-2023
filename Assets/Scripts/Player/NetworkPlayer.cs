using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

[RequireComponent(typeof(CharacterInputHandler))]
public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    NicknameText _myNickname;

    //[Networked(OnChanged = nameof(OnNicknameChanged))]
    //NetworkString<_16> Nickname { get; set; }

    public event Action OnLeft = delegate { };

    public override void Spawned()
    {
        //_myNickname = NicknameHandler.Instance.AddNickname(this);

        Color skinColor = Color.white;

        //Si tengo autoridad de Input
        if (Object.HasInputAuthority)
        {
            Local = this;

            skinColor = Color.blue;

            //RPC_SetNickname("John Doe " + UnityEngine.Random.Range(1, 1001));
        }
        else if (Object.HasStateAuthority)
        {
            skinColor = Color.yellow;
        }
        else
        {
            skinColor = Color.red;
        }

        //GetComponentInChildren<SkinnedMeshRenderer>().material.color = skinColor;
    }


    public CharacterInputHandler GetInputHandler()
    {
        return GetComponent<CharacterInputHandler>();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnLeft();
    }
}
