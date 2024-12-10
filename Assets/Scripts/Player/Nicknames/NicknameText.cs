using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NicknameText : MonoBehaviour
{
    const float OFFSET_POSITION_Y = 2.5F;

    Transform _owner;

    Text _myText;

    public NicknameText SetOwner(NetworkPlayer owner)
    {
        _myText = GetComponent<Text>();

        _owner = owner.transform;

        return this;
    }

    public void UpdateNickname(string str)
    {
        _myText.text = str;
    }
    
    public void UpdatePosition()
    {
        transform.position = _owner.position + Vector3.up * OFFSET_POSITION_Y;
    }

}
