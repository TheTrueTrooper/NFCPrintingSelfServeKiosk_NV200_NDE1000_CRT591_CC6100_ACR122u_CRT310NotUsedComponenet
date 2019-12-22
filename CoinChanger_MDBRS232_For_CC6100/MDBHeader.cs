using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100
{
    public enum MDBDataSource
    {
        NotSet,
        CoinChanger,
        BillAcceptor
    }

    public class MDBHeader
    {
        public bool Activity { get; private set; } = false;
        public bool CoinChangerIsAttached { get; private set; } = false;
        public bool BillAcceptorIsAttached { get; private set; } = false;
        public MDBDataSource DataSource { get; private set; } = MDBDataSource.NotSet;


        public MDBHeader(byte Decode, MDBDataSource DataRecivedFrom = MDBDataSource.CoinChanger)
        {
            if ((Decode & 0x80) != 0)
            {
                Activity = true;
                byte DataSource = (byte)(Decode & 0x70);

                if (DataRecivedFrom == MDBDataSource.CoinChanger)
                {
                    CoinChangerIsAttached = true;
                    if ((Decode & 0x01) != 0)
                        BillAcceptorIsAttached = true;
                }
                else if (DataRecivedFrom == MDBDataSource.BillAcceptor)
                {
                    BillAcceptorIsAttached = true;
                    if ((Decode & 0x01) != 0)
                        CoinChangerIsAttached = true;
                }
            }
            else
            {

            }
        }
    }
}
