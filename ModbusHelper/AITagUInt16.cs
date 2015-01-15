using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AITagUInt16 : ROTag<UInt16>
    {
        public AITagUInt16(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }


        protected override ushort readValueFromDevice()
        {
            ushort val = default(ushort);

            if (modbusDevice is IModbusMaster)
                val = (UInt16)(modbusDevice as IModbusMaster).ReadInputRegisters(deviceAddress, tagAddress, 1)[0];

            return val;
        }

    }
}
