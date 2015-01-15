using Modbus.Device;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public class ModbusDeviceManager : IDisposable
    {
        public event EventHandler<EventArgs> TransactionCompleted;

        private int sleepInterval = 100;

        private readonly CancellationTokenSource feedCancellationTokenSource = new CancellationTokenSource();
        private readonly Task feedTask;

        ConcurrentDictionary<IConnector, ConcurrentBag<ITag>> connectors = new ConcurrentDictionary<IConnector, ConcurrentBag<ITag>>();

        private static readonly ModbusDeviceManager instance = new ModbusDeviceManager();

        private ModbusDeviceManager()
        {
            if (LicenseManager.CurrentContext.UsageMode == LicenseUsageMode.Runtime)
            {

                feedTask = Task.Factory.StartNew(() =>
                    {
                        while (!feedCancellationTokenSource.IsCancellationRequested)
                        {
                            if (connectors.Count > 0)
                            {
                                Task[] tasks = new Task[connectors.Count];


                                int i = 0;

                                foreach (var con in connectors)
                                {

                                    tasks[i] = Task.Factory.StartNew(() =>
                                        {

                                            foreach (var tag in con.Value)
                                            {

                                                if (tag != null)
                                                {
                                                    tag.SetModbusDevice();
                                                    tag.Take(this.sleepInterval);
                                                    if (tag.GetModbusDevice() == null)
                                                    {
                                                        tag.ResetModbusDevice();
                                                    }

                                                }
                                                if (feedCancellationTokenSource.IsCancellationRequested) break;
                                            }

                                            
                                        });
                                }

                                i++;
                                Task.WaitAll(tasks);
                            };

                            RaiseTransactionCompleted();
                        }

                    });

            }
        }

        public static ModbusDeviceManager Instance
        {
            get
            {
                return instance;
            }
        }

        public void SetSleepInterval(int sleepInterval)
        {
            this.sleepInterval = sleepInterval;
        }


        public T AddTag<T>(IConnector connector, byte deviceAddress, ushort tagAddress) where T : Tag
        {
            var tag = (T)Activator.CreateInstance(typeof(T), connector, deviceAddress, tagAddress);


            connectors.TryAdd(connector, new ConcurrentBag<ITag>());
            connectors[connector].Add(tag as ITag);

            return tag;
        }

        protected void RaiseTransactionCompleted()
        {
            OnTransactionCompletedEvent(new EventArgs());
        }

        protected virtual void OnTransactionCompletedEvent(EventArgs e)
        {
            EventHandler<EventArgs> handler = TransactionCompleted;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                feedCancellationTokenSource.Cancel();
                feedTask.Wait();

                feedCancellationTokenSource.Dispose();
                feedTask.Dispose();
            }
        }

    }

}
