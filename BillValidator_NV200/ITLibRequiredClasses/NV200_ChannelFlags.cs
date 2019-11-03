using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    [Flags]
    public enum NV200_ChannelFlags : ushort
    {
        Channel1 = 0x01,
        Channel2 = 0x02,
        Channel3 = 0x04,
        Channel4 = 0x08,
        Channel5 = 0x10,
        Channel6 = 0x20,
        Channel7 = 0x40,
        Channel8 = 0x80,
        Channel9 = 0x0100,
        Channel10 = 0x0200,
        Channel11 = 0x0400,
        Channel12 = 0x0800,
        Channel13 = 0x1000,
        Channel14 = 0x2000,
        Channel15 = 0x4000,
        Channel16 = 0x8000
    }
}
