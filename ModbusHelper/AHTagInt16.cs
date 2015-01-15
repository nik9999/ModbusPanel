using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AHTagInt16 : RWTag<Int16>
    {
        public AHTagInt16(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

        protected override Int16 readValueFromDevice()
        {
            Int16 val = default(Int16);
            
            if (modbusDevice is IModbusMaster)
                val = (Int16)(modbusDevice as IModbusMaster).ReadHoldingRegisters(deviceAddress, tagAddress, 1)[0];

            return val;
        }

        protected override void writeValueToDevice(Int16 val)
        {
            if (modbusDevice is IModbusMaster)
                (modbusDevice as IModbusMaster).WriteSingleRegister(deviceAddress, tagAddress, (ushort)val);
        }
    }
}
