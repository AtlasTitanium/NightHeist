using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class ServerEventManager
{
    public delegate void Function(object caller, DataStreamReader stream, ref DataStreamReader.Context context, NetworkConnection source);

    public static readonly Dictionary<ServerEvent, Function> ServerEvents = new Dictionary<ServerEvent, Function>(){
        {ServerEvent.PING, Ping},
        {ServerEvent.Box_Jump, BoxJump}
    };

    public static void Ping(object caller, DataStreamReader stream, ref DataStreamReader.Context context, NetworkConnection source) {
        Server server = caller as Server;
        server.RecievePing();
    }

    public static void BoxJump(object caller, DataStreamReader stream, ref DataStreamReader.Context context, NetworkConnection source) {
        Server server = caller as Server;
        server.BoxJump();
    }
}
