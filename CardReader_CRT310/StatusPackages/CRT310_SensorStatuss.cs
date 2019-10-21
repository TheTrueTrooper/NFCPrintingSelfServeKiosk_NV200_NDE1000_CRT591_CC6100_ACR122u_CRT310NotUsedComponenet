using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT310
{
    public class CRT310_SensorStatuss
    {
        public CRT310_SensorStatus[] SensorStatuss { private set; get; }
        public CRT310_ShutterStatus SutterStatus { private set; get; }
        public CRT310_SwitchStatus SwitchStatus { private set; get; }

        internal CRT310_SensorStatuss(CRT310_SensorStatus[] SensorStatuss, CRT310_ShutterStatus SutterStatus, CRT310_SwitchStatus SwitchStatus)
        {
            this.SensorStatuss = SensorStatuss;
            this.SutterStatus = SutterStatus;
            this.SwitchStatus = SwitchStatus;
        }
    }
}
