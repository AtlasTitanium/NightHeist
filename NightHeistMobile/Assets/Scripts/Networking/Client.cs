using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Networking.Transport;

public class Client : MonoBehaviour
{
    public UdpNetworkDriver m_Driver;
    public NetworkConnection m_Connection;
    public string ip;

    void Start () {
        m_Driver = new UdpNetworkDriver(new INetworkParameter[0]);
        m_Connection = default;

        var endpoint = NetworkEndPoint.Parse(ip, 9000);
        m_Connection = m_Driver.Connect(endpoint);
    }

    public void OnDestroy() { 
        m_Driver.Dispose();
    }

    void FixedUpdate() { 
        m_Driver.ScheduleUpdate().Complete();
        if (!m_Connection.IsCreated){
            return;
        }
        WorkClient();
    }

    #region Client Functions

    private void WorkClient(){
        DataStreamReader stream;
        NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != NetworkEvent.Type.Empty) {
            switch (cmd){
                case NetworkEvent.Type.Connect:
                    Debug.Log("Client Connect");
                break;
                
                case NetworkEvent.Type.Data:
                    HandleData(stream);
                break;

                case NetworkEvent.Type.Disconnect:
                    DisconnectClient();
                break;
            }
        }
    }
    
    private void HandleData(DataStreamReader stream){
        var readerCtx = default(DataStreamReader.Context);
        ClientEvent eventName = (ClientEvent)stream.ReadUInt(ref readerCtx);
        ClientEventManager.ClientEvents[eventName](this, stream, ref readerCtx, m_Connection);
    }
    
    private void DisconnectClient(){
        m_Connection = default(NetworkConnection);
    }
    #endregion
    

    #region ping
    public void PingServer(){
        using (var writer = new DataStreamWriter(8, Allocator.Temp)) {
            writer.Write((uint)ServerEvent.PING);
            m_Connection.Send(m_Driver, writer);
        }
    }

    public void RecievePing(){
        PingServer();
    }
    #endregion
    
    public void ButtonClicked() {
        using (var writer = new DataStreamWriter(8, Allocator.Temp)) {
            writer.Write((uint)ServerEvent.Box_Jump);
            m_Connection.Send(m_Driver, writer);
        }
    }
}
