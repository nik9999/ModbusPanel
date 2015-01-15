using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AHTagUInt16 : RWTag<UInt16>
    {
        public AHTagUInt16(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

        protected override ushort readValueFromDevice()
        {
            ushort val = default(ushort);
            
            if (modbusDevice is IModbusMaster)
                val = (UInt16)(modbusDevice as IModbusMaster).ReadHoldingRegisters(deviceAddress, tagAddress, 1)[0];

            return val;
        }

        protected override void writeValueToDevice(ushort val)
        {
            if (modbusDevice is IModbusMaster)
                (modbusDevice as IModbusMaster).WriteSingleRegister(deviceAddress, tagAddress, val);
        }
    }
}
