using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModbusHelper
{
    public class TagChangedEventArgs : EventArgs
    {
        public TagChangedEventArgs(TagQuality quality)
        {
            this.quality = quality;
        }

        private TagQuality quality;
        public TagQuality Quality
        {
            get { return quality; }

        }
    }
}
