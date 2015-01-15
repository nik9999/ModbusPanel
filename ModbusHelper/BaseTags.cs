using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public enum TakeStrategy
    {
        ReadOnly,
        WriteRead
    }

    public abstract class ROTag<T> : Tag, ITag
    {
        private T oldValue;

        protected T value;
        public T Value
        {
            get { return this.value; }
        }

        public ROTag(IConnector connector, byte deviceAddress, ushort dataAddress)
        {
            this.tagAddress = dataAddress;
            this.deviceAddress = deviceAddress;
            this.Connector = connector;

            this.value = default(T);
            this.oldValue = this.value;
        }

        protected bool tagChanged(T newVal)
        {
            bool eq = EqualityComparer<T>.Default.Equals(oldValue, newVal);
            if (!eq)
            {
                oldValue = newVal;
            }

            return !eq;
        }

        protected abstract T readValueFromDevice();
        void ITag.Take(int sleepInterval)
        {
            this.dateTimeStamp = DateTime.Now;
            T val = default(T);
            TagQuality firstTimeQuality = this.quality;

            if (modbusDevice != null)
            {
                try
                {
                    val = readValueFromDevice();
                    this.quality = TagQuality.Good;
                }
                catch
                {
                    this.quality = TagQuality.Bad;
                    modbusDevice.Dispose();
                    modbusDevice = null;
                }
            }
            else
            {
                this.quality = TagQuality.Bad;
            }

            this.value = val;

            if (firstTimeQuality == TagQuality.Uncertain)
            {
                tagChanged(val);
                RaiseTagChanged(this.quality);
            }
            else
            {
                if (this.quality == TagQuality.Good)
                {
                    if (tagChanged(val))
                    {
                        RaiseTagChanged(this.quality);
                    }
                }
                else RaiseTagChanged(this.quality);
            }

            Thread.Sleep(sleepInterval);
        }
    }

    public abstract class RWTag<T> : Tag, ITag
    {
        private T oldValue;

        protected TakeStrategy takeStrategy;
        protected T value;
        public T Value
        {
            get { return this.value; }
            set
            {
                if (!EqualityComparer<T>.Default.Equals(this.value, value))
                {
                    this.takeStrategy = TakeStrategy.WriteRead;
                    this.value = value;
                }
                
            }
        }

        public RWTag(IConnector connector, byte deviceAddress, ushort dataAddress)
        {
            this.tagAddress = dataAddress;
            this.deviceAddress = deviceAddress;
            this.Connector = connector;

            takeStrategy = TakeStrategy.ReadOnly;
            this.value = default(T);
            this.oldValue = this.value;
        }

        protected bool tagChanged(T newVal) 
        {
            

            bool eq =  EqualityComparer<T>.Default.Equals(oldValue, newVal);

            

            if (!eq)
            {
                oldValue = newVal;
            }

            return !eq;
        }

        protected abstract T readValueFromDevice();
        protected abstract void writeValueToDevice(T val);

        void ITag.Take(int sleepInterval)
        {
            this.dateTimeStamp = DateTime.Now;
            T val = default(T);
            TagQuality firstTimeQuality = this.quality;

            if (modbusDevice != null)
            {
                    try
                    {
                        if (takeStrategy == TakeStrategy.ReadOnly)
                            val = readValueFromDevice();
                        else
                        {
                            writeValueToDevice(this.value);
                            takeStrategy = TakeStrategy.ReadOnly;

                            Thread.Sleep(sleepInterval);

                            val = readValueFromDevice();
                        }

                        this.quality = TagQuality.Good;
                    }
                    catch
                    {
                        this.quality = TagQuality.Bad;
                        modbusDevice.Dispose();
                        modbusDevice = null;
                    }
            }
            else
            {
                this.quality = TagQuality.Bad;
            }

            this.value = val;

            if (firstTimeQuality == TagQuality.Uncertain)
            {
                tagChanged(val);
                RaiseTagChanged(this.quality);
            }
            else
            {
                if (this.quality == TagQuality.Good)
                {
                    if (tagChanged(val))
                    {
                        RaiseTagChanged(this.quality);
                    }
                }
                else RaiseTagChanged(this.quality);
            }

            Thread.Sleep(sleepInterval);
        }
    }
}
