using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReader_CRT_591
{
    /// <summary>
    /// The status of the sensors
    /// </summary>
    public enum CRT591_SensorStatus
    {
        /// <summary>
        /// We arnt sure what happened but this is an invalid return or the Sensor is reserved
        /// </summary>
        UnknownValueOrReservedS8,
        /// <summary>
        /// There is no card pressent where this sensor is
        /// </summary>
        NoCard = 0x30,
        /// <summary>
        /// There is a card pressent where this sensor is
        /// </summary>
        WithCard = 0x31
    }
}
