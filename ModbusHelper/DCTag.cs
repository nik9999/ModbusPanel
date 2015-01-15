using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class DCTag : RWTag<bool>
    {
        public DCTag(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

        protected override bool readValueFromDevice()
        {
            bool val = default(bool);

            if (modbusDevice is IModbusMaster)
                val = (modbusDevice as IModbusMaster).ReadCoils(deviceAddress, tagAddress, 1)[0];

            return val;
        }

        protected override void writeValueToDevice(bool val)
        {
            if (modbusDevice is IModbusMaster)
                (modbusDevice as IModbusMaster).WriteSingleCoil(deviceAddress, tagAddress, val);
        }
    }
}
