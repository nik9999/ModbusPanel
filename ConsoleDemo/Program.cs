using Modbus.Device;
using ModbusHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDemo
{
    class Program
    {
        static DITag t,t1;
        static int i, j;

        static void Main(string[] args)
        {

            //TcpClient client = new TcpClient("127.0.0.1", 502);
            //ModbusIpMaster master = ModbusIpMaster.CreateIp(client);

            //TcpClient client1 = new TcpClient("127.0.0.1", 502);
            //ModbusIpMaster master1 = ModbusIpMaster.CreateIp(client);

            //Console.WriteLine(master == master1);

            //var a = ModbusDeviceManager.Instance.AddModbusDevice(() =>
            //{
            //    TcpClient client = new TcpClient("127.0.0.1", 502);
            //    return ModbusIpMaster.CreateIp(client);
            //}, new byte[] { 1 });

            //var b = ModbusDeviceManager.Instance.AddModbusDevice(() =>
            //{
            //    TcpClient client1 = new TcpClient("127.0.0.1", 503);
            //    return ModbusIpMaster.CreateIp(client1);
            //}, new byte[] { 1 });

            //Console.WriteLine(a+" "+b);

            //TcpClient client;
            //ModbusIpMaster master = null;

            //t = ModbusDeviceManager.Instance.AddTag<DITag>(1, 1, () =>
            //    {
            //        if (master == null)
            //        {
            //            try
            //            {
            //                client = new TcpClient("127.0.0.1", 502);
            //                master = ModbusIpMaster.CreateIp(client);
            //            }

            //            catch
            //            {
            //                Console.WriteLine("Dont connect to controller!");
            //            }
            //        }
            //        return master;
            //    }, () =>
            //    {
            //        if (master != null) master.Dispose();
            //        master = null;
            //        client = null;

            //    });
            //t.RaiseTagChangedEvent +=a_RaiseTagChangedEvent;
            EthernetConnector ec1 = new EthernetConnector("127.0.0.1", 502, EthernetMode.TCP);
            EthernetConnector ec2 = new EthernetConnector("127.0.0.1", 503, EthernetMode.TCP);

            ModbusDeviceManager.Instance.TransactionCompleted += Instance_RaiseTransactionCompletedEvent;

            t = ModbusDeviceManager.Instance.AddTag<DITag>(ec1, 1, 1);
            t.TagChanged += a_RaiseTagChangedEvent;
            t1 = ModbusDeviceManager.Instance.AddTag<DITag>(ec2, 1, 2);
            t1.TagChanged += a_RaiseTagChangedEvent1;

            Console.ReadLine();
        }

        static void Instance_RaiseTransactionCompletedEvent(object sender, EventArgs e)
        {
            Console.WriteLine("Completed");
        }

        private static void a_RaiseTagChangedEvent(object sender, TagChangedEventArgs e)
        {
            Console.WriteLine("tag_0 " + t.Value+" "+e.Quality+" "+i++);
        }

        private static void a_RaiseTagChangedEvent1(object sender, TagChangedEventArgs e)
        {
            Console.WriteLine("tag_1 " + t.Value + " " + e.Quality + " " + j++);
        }


        static ModbusDevice SetModbusDevice()
        {
            TcpClient client = new TcpClient("127.0.0.1", 502);
            return ModbusIpMaster.CreateIp(client);
        }

        static void resetModbusDevice()
        {

        }
    }

}
