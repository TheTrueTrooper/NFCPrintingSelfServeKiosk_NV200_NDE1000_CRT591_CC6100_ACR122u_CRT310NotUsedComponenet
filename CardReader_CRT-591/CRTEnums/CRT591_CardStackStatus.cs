﻿namespace CardReader_CRT_591
{
    /// <summary>
    /// A status Enum returned on reponses for the card stack
    /// </summary>
    public enum CRT591_CardStackStatus : byte
    {
        StackStatus_Unkown,
        StackStatus_NoCards = 0x30,
        StackStatus_FewCards = 0x31,
        StackStatus_FullOfCards = 0x32
    }
}
