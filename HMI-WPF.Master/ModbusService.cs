using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using ModbusHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HMI_WPF.Master
{
    [Export]
    public class ModbusService
    {
        private ILoggerFacade logger;
        private IEventAggregator eventAggregator;

        [Export("DigitalInputTag")]
        public DITag DigitalInputTag;

        [Export("DigitalCoilTag")]
        public DCTag DigitalCoilTag;

        // Input Registers
        [Export("InputRegisterTagUInt16")]
        public AITagUInt16 InputRegisterTagUInt16;

        [Export("InputRegisterTagInt16")]
        public AITagInt16 InputRegisterTagInt16;

        [Export("InputRegisterTagUInt32")]
        public AITagUInt32 InputRegisterTagUInt32;

        [Export("InputRegisterTagInt32")]
        public AITagInt32 InputRegisterTagInt32;

        [Export("InputRegisterTagFloat")]
        public AITagFloat InputRegisterTagFloat;

        // Holding Registers
        [Export("HoldingRegisterTagUInt16")]
        public AHTagUInt16 HoldingRegisterTagUInt16;

        [Export("HoldingRegisterTagInt16")]
        public AHTagInt16 HoldingRegisterTagInt16;

        [Export("HoldingRegisterTagUInt32")]
        public AHTagUInt32 HoldingRegisterTagUInt32;

        [Export("HoldingRegisterTagInt32")]
        public AHTagInt32 HoldingRegisterTagInt32;

        [Export("HoldingRegisterTagFloat")]
        public AHTagFloat HoldingRegisterTagFloat;

        [ImportingConstructor]
        public ModbusService(IEventAggregator eventAggregator, ILoggerFacade logger)
        {
            this.logger = logger;
            this.eventAggregator = eventAggregator;

            ModbusDeviceManager.Instance.SetSleepInterval(250);

            EthernetConnector ec1 = new EthernetConnector("127.0.0.1", 502, EthernetMode.TCP);
            DigitalInputTag = ModbusDeviceManager.Instance.AddTag<DITag>(ec1, 1, 1);
            
            DigitalCoilTag = ModbusDeviceManager.Instance.AddTag<DCTag>(ec1, 1, 1);
            
            InputRegisterTagUInt16 = ModbusDeviceManager.Instance.AddTag<AITagUInt16>(ec1, 1, 1);
            InputRegisterTagInt16 = ModbusDeviceManager.Instance.AddTag<AITagInt16>(ec1, 1, 2);

            InputRegisterTagUInt32 = ModbusDeviceManager.Instance.AddTag<AITagUInt32>(ec1, 1, 3);
            InputRegisterTagInt32 = ModbusDeviceManager.Instance.AddTag<AITagInt32>(ec1, 1, 5);

            InputRegisterTagFloat = ModbusDeviceManager.Instance.AddTag<AITagFloat>(ec1, 1, 7);
            

            HoldingRegisterTagUInt16 = ModbusDeviceManager.Instance.AddTag<AHTagUInt16>(ec1, 1, 1);
            HoldingRegisterTagUInt16.Value = (UInt16)DateTime.Now.Second;

            HoldingRegisterTagInt16 = ModbusDeviceManager.Instance.AddTag<AHTagInt16>(ec1, 1, 2);
            HoldingRegisterTagInt16.Value = (Int16)(DateTime.Now.Second*10 + DateTime.Now.Second);

            HoldingRegisterTagUInt32 = ModbusDeviceManager.Instance.AddTag<AHTagUInt32>(ec1, 1, 3);
            HoldingRegisterTagInt32 = ModbusDeviceManager.Instance.AddTag<AHTagInt32>(ec1, 1, 5);
            HoldingRegisterTagInt32.Value = -33;

            HoldingRegisterTagFloat = ModbusDeviceManager.Instance.AddTag<AHTagFloat>(ec1, 1, 7);
            HoldingRegisterTagFloat.Value = (float)-0.26;
            
        }
    }
}
