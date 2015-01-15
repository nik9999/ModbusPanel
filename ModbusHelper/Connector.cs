using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public class ConnectionEventArgs : EventArgs
    {
        public ConnectionEventArgs(bool  isConnected)
        {
            this.isConnected = isConnected;
        }
        private bool isConnected;

        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value; }
        }
    }

    public abstract class Connector : IConnector
    {
        public event EventHandler<ConnectionEventArgs> ConnectionCompleted;

        public abstract ModbusDevice SetConnection();

        public abstract void ResetConnection();

        protected virtual void OnRaiseConnectionCompleted(ConnectionEventArgs e)
        {
            EventHandler<ConnectionEventArgs> handler = ConnectionCompleted;

            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    public enum SerialMode
    {
        Ascii,
        Rtu
    }
    public sealed class SerialConnector : Connector
    {
        public SerialConnector(SerialPort port, SerialMode mode)
        {

        }

        public override ModbusDevice SetConnection()
        {
            return null;
        }

        public override void ResetConnection()
        { }
    }

    public enum EthernetMode
    {
        TCP,
        UDP
    }

    public sealed class EthernetConnector : Connector
    {
        private TcpClient client;
        private ModbusIpMaster master { get; set; }

        string ip;
        int port;
        EthernetMode mode;

        public EthernetConnector(string ip, int port, EthernetMode mode)
        {
            this.ip = ip;
            this.port = port;
            this.mode = mode;
        }

        public override ModbusDevice SetConnection()
        {
            if (master == null)
            {
                try
                {
                    client = new TcpClient(ip, port);
                    this.master = ModbusIpMaster.CreateIp(client);
                    OnRaiseConnectionCompleted(new ConnectionEventArgs(true));
                }
                catch 
                {
                    OnRaiseConnectionCompleted(new ConnectionEventArgs(false));
                }
            }
            return this.master;
        }

        public override void ResetConnection()
        {
            if (master != null) master.Dispose();
            master = null;
            client = null;
        }

    }
}
