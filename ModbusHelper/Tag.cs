using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public abstract class Tag : INotifyPropertyChanged
    {
        public event EventHandler<TagChangedEventArgs> TagChanged;

        protected DateTime dateTimeStamp;
        public DateTime DateTimeStamp { get { return dateTimeStamp; } }
        protected TagQuality quality;
        public TagQuality Quality { get { return quality; } }

        protected ushort tagAddress;
        public ushort TagAddress { get { return tagAddress; } }

        protected byte deviceAddress;
        public byte DeviceAddress { get { return deviceAddress; } }

        protected ModbusDevice modbusDevice;
        
        internal IConnector Connector;

        protected Tag()
        {
            quality = TagQuality.Uncertain;
            dateTimeStamp = DateTime.Now;
        }

        public ModbusDevice GetModbusDevice()
        {
            return modbusDevice;
        }

        public void SetModbusDevice()
        {
            if (Connector != null)
                this.modbusDevice = Connector.SetConnection();
            else
                this.modbusDevice = null;
        }

        public void ResetModbusDevice()
        {
            if (Connector != null)
                Connector.ResetConnection();
        }


        protected void RaiseTagChanged(TagQuality q)
        {
            NotifyPropertyChanged("Value");
            NotifyPropertyChanged("Quality");
            NotifyPropertyChanged("DateTimeStamp");

            OnRaiseTagChangedEvent(new TagChangedEventArgs(q));
        }

        protected virtual void OnRaiseTagChangedEvent(TagChangedEventArgs e)
        {
            EventHandler<TagChangedEventArgs> handler = TagChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName )
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
