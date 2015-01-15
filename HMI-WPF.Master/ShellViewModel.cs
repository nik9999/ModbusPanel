using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using ModbusHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI_WPF.Master
{
    [Export]
    public class ShellViewModel
    {
        private int ii;
        public int I
        {
            get { return ii; }
            set{ ii = value; }
        }
        private ILoggerFacade logger;
        private ModbusService modbusService;
        private IEventAggregator eventAggregator;

        [Import("DigitalInputTag")]
        private DITag digitalInputTag;
        public DITag DigitalInputTag { get { return digitalInputTag; } }


        [Import("DigitalCoilTag")]
        public DCTag DigitalCoilTag { get;set; }

        [Import("InputRegisterTagUInt16")]
        private AITagUInt16 inputRegisterTagUInt16;
        public AITagUInt16 InputRegisterTagUInt16 { get { return inputRegisterTagUInt16; } }

        [Import("InputRegisterTagInt16")]
        private AITagInt16 inputRegisterTagInt16;
        public AITagInt16 InputRegisterTagInt16 { get { return inputRegisterTagInt16; } }

        [Import("InputRegisterTagUInt32")]
        private AITagUInt32 inputRegisterTagUInt32;
        public AITagUInt32 InputRegisterTagUInt32 { get { return inputRegisterTagUInt32; } }

        [Import("InputRegisterTagInt32")]
        private AITagInt32 inputRegisterTagInt32;
        public AITagInt32 InputRegisterTagInt32 { get { return inputRegisterTagInt32; } }

        [Import("InputRegisterTagFloat")]
        private AITagFloat inputRegisterTagFloat;
        public AITagFloat InputRegisterTagFloat { get { return inputRegisterTagFloat; } }

        [Import("HoldingRegisterTagUInt16")]
        public AHTagUInt16 HoldingRegisterTagUInt16 { get; set; }

        [Import("HoldingRegisterTagInt16")]
        public AHTagInt16 HoldingRegisterTagInt16 { get; set; }

        [Import("HoldingRegisterTagUInt32")]
        public AHTagUInt32 HoldingRegisterTagUInt32 { get; set; }

        [Import("HoldingRegisterTagInt32")]
        public AHTagInt32 HoldingRegisterTagInt32 { get; set; }

        [Import("HoldingRegisterTagFloat")]
        public AHTagFloat HoldingRegisterTagFloat { get; set; }


        [ImportingConstructor]
        public ShellViewModel(ILoggerFacade logger, IEventAggregator eventAggregator)
        {
            this.logger = logger;
            //this.modbusService = modbusService;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            HoldingRegisterTagUInt16.Value = (ushort)DateTime.Now.Second;
            //HoldingRegisterTagInt16.Value = (Int16)(DateTime.Now.Second * 10 + DateTime.Now.Second);

            DigitalCoilTag.Value = !DigitalCoilTag.Value;
        }
    }
}
