using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConnect
{
    bool CanConnectIn { get; set; }
    bool CanConnectOut { get; set; }

    bool ConnectIn(Node node);
    bool CheckConnectOut();
    bool ConnectOut(Node Node);
    void Disconnect(Node Node);
    void DisconnectIn(Node node);
}
