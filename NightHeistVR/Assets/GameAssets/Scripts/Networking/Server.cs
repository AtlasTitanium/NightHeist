using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Unity.Networking.Transport; 
using Unity.Collections;

using UdpCNetworkDriver = Unity.Networking.Transport.UdpNetworkDriver;

public class Server : MonoBehaviour
{
    public UdpCNetworkDriver m_Driver;
    private NativeList<NetworkConnection> m_Connections;

    public Test testBox;

    void Start(){
        Debug.Log(IPManager.GetLocalIPAddress());

        m_Driver = new UdpCNetworkDriver(new INetworkParameter[0]);
        if (m_Driver.Bind(NetworkEndPoint.Parse(IPManager.GetLocalIPAddress(), 9000)) != 0) {
            Debug.Log("failed to bind to port....");
        }
        else {
            m_Driver.Listen();
        }
        m_Connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    public void OnDestroy(){
        m_Driver.Dispose();
        m_Connections.Dispose();
    }

    void FixedUpdate(){
        m_Driver.ScheduleUpdate().Complete();
        UpdateConnections();
        WorkServer();
        PingClients();     
    }
    
    private void WorkServer(){
        Debug.Log("getting data");
        DataStreamReader stream;
        for (int i = 0; i < m_Connections.Length; i++){
            if (!m_Connections[i].IsCreated)
                continue;

            NetworkEvent.Type cmd;
            while ((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != NetworkEvent.Type.Empty){
                switch (cmd){
                    case NetworkEvent.Type.Data:
                        Debug.Log("got data");
                        HandleData(stream, i);
                    break;

                    case NetworkEvent.Type.Disconnect:
                        PlayerDisconnect(i);
                    break;
                }
            }
        }
    }
    
    private void UpdateConnections(){
        // Clean up connections
        for (int i = 0; i < m_Connections.Length; i++){
            if (!m_Connections[i].IsCreated){
                m_Connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        // Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default(NetworkConnection)){
            m_Connections.Add(c);
            Debug.Log("Accepted a connection");
        }
    }
    
    private void HandleData(DataStreamReader stream, int connectionIndex){
        var readerCtx = default(DataStreamReader.Context);
        float info = stream.ReadFloat(ref readerCtx);
        Debug.Log(info);
        //ServerEvent eventName = (ServerEvent)stream.ReadUInt(ref readerCtx);
        //ServerEventManager.ServerEvents[eventName](this, stream, ref readerCtx, m_Connections[connectionIndex]);
    }
    
    private void PlayerDisconnect(int playerIndex){
        Debug.Log("Client disconnected from server");
        m_Connections[playerIndex] = default(NetworkConnection);
    }
   
    #region ping
    private void PingClients(){
        for (int i = 0; i < m_Connections.Length; i++){
            if (!m_Connections[i].IsCreated)
                continue;
            
            using (var writer = new DataStreamWriter(10, Allocator.Temp)) {
                writer.Write((uint)ClientEvent.PING);
                m_Connections[i].Send(m_Driver, writer);
            }
        }
    }

    public void RecievePing(){
        Debug.Log("pinged back");
    }
    
    #endregion

    public void BoxJump() {
        testBox.Jump();
    }
}
