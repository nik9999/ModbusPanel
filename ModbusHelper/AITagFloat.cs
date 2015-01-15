using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AITagFloat : ROTag<float>
    {
        public RegisterOrder TagRegisterOrder;

        public AITagFloat(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }


        protected override Single readValueFromDevice()
        {
            Single val = default(Single);

            ushort[] reg = new ushort[2];

            if (modbusDevice is IModbusMaster)
                reg = (modbusDevice as IModbusMaster).ReadInputRegisters(deviceAddress, tagAddress, 2);

            if (TagRegisterOrder == RegisterOrder.BE)
                val = BitConverter.ToSingle(BitConverter.GetBytes(reg[0]).Concat(BitConverter.GetBytes(reg[1])).ToArray(), 0);
            else
                val = BitConverter.ToSingle(BitConverter.GetBytes(reg[1]).Concat(BitConverter.GetBytes(reg[0])).ToArray(), 0);

            return val;
        }
    }
}
