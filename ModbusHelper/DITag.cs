using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class DITag : ROTag<bool>
    {

        public DITag(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

         protected override bool readValueFromDevice()
        {
            bool val = default(bool);

            if (modbusDevice is IModbusMaster)
                val = (modbusDevice as IModbusMaster).ReadInputs(deviceAddress, tagAddress, 1)[0];

            return val;
        }
    }
}
