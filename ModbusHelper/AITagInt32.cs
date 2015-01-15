using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AITagInt32 : ROTag<Int32>
    {
        public RegisterOrder TagRegisterOrder;

        public AITagInt32(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

        protected override Int32 readValueFromDevice()
        {
            Int32 val = default(Int32);

            ushort[] reg = new ushort[2];

            if (modbusDevice is IModbusMaster)
                reg = (modbusDevice as IModbusMaster).ReadInputRegisters(deviceAddress, tagAddress, 2);

            if (TagRegisterOrder == RegisterOrder.BE)
                val = BitConverter.ToInt32(BitConverter.GetBytes(reg[0]).Concat(BitConverter.GetBytes(reg[1])).ToArray(), 0);
            else
                val = BitConverter.ToInt32(BitConverter.GetBytes(reg[1]).Concat(BitConverter.GetBytes(reg[0])).ToArray(), 0);

            return val;
        }
    }
}
