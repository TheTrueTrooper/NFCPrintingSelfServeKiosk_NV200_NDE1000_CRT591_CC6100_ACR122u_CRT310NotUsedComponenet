using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100.MDBEnums
{
    /// <summary>
    /// Upon startup one of these values below may be sent to the PC – These are the VMC  Commands. 
    /// </summary>
    enum CC6100MDB_MDBStartStatuses
    {
        Reset,
        Status,
        TubeStatus,
        Poll,
        CoinType,
        Dispense
    }
}
