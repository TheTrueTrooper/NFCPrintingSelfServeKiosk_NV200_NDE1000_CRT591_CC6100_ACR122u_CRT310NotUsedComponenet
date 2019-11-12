using System;
namespace BillValidator_NV200
{
    /// <summary>
    /// Flags that are used to set a channel on (Xor with 0xFF or mask in some other way) 
    /// </summary>
    [Flags]
    public enum NV200_ChannelFlags : ushort
    {
        UnknowChannelOrStillReading = 0x00,
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
