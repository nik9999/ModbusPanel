using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public interface IConnector
    {
        ModbusDevice SetConnection();
        void ResetConnection();
    }
}
