using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    internal interface ITag
    {
        void Take(int sleepInterval);
        ModbusDevice GetModbusDevice();
        void SetModbusDevice();
        void ResetModbusDevice();
    }
}
