using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public sealed class AHTagInt32 : RWTag<Int32>
    {
        public RegisterOrder TagRegisterOrder;

        public AHTagInt32(IConnector connector, byte deviceAddress, ushort dataAddress) : base(connector, deviceAddress, dataAddress) { }

        protected override int readValueFromDevice()
        {
            Int32 val = default(Int32);

            ushort[] reg = new ushort[2];

            if (modbusDevice is IModbusMaster)
                reg = (modbusDevice as IModbusMaster).ReadHoldingRegisters(deviceAddress, tagAddress, 2);
            
            if (TagRegisterOrder == RegisterOrder.BE)
                val = BitConverter.ToInt32(BitConverter.GetBytes(reg[0]).Concat(BitConverter.GetBytes(reg[1])).ToArray(), 0);
            else
                val = BitConverter.ToInt32(BitConverter.GetBytes(reg[1]).Concat(BitConverter.GetBytes(reg[0])).ToArray(), 0);

            return val;
        }

        protected override void writeValueToDevice(int val)
        {
            byte[] tmp = BitConverter.GetBytes(val);
            ushort[] reg = new ushort[2];
            if (TagRegisterOrder == RegisterOrder.BE)
                reg = new ushort[] { BitConverter.ToUInt16(tmp, 0), BitConverter.ToUInt16(tmp, 2) };
            else
                reg = new ushort[] { BitConverter.ToUInt16(tmp, 2), BitConverter.ToUInt16(tmp, 0) };

            if (modbusDevice is IModbusMaster)
                (modbusDevice as IModbusMaster).WriteMultipleRegisters(deviceAddress, tagAddress, reg);
        }
    }
}
