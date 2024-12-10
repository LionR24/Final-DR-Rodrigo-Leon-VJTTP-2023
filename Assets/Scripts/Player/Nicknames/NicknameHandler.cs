using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NicknameHandler : MonoBehaviour
{
    public static NicknameHandler Instance { get; private set; }

    [SerializeField] NicknameText _nicknamePrefab;

    List<NicknameText> _allNicknames;

    void Awake()
    {
        if (Instance) Destroy(gameObject);
        else Instance = this;

        _allNicknames = new List<NicknameText>();
    }

    public NicknameText AddNickname(NetworkPlayer owner)
    {
        NicknameText newNickname = Instantiate(_nicknamePrefab, transform).
                                   SetOwner(owner);

        _allNicknames.Add(newNickname);

        owner.OnLeft += () =>
        {
            _allNicknames.Remove(newNickname);
            Destroy(newNickname.gameObject);
        };

        return newNickname;
    }

    private void LateUpdate()
    {
        foreach (var nickname in _allNicknames)
        {
            //nickname.UpdatePosition();
        }
    }
}
