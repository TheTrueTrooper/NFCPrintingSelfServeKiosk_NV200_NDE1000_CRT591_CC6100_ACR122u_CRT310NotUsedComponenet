namespace BillValidator_NV200
{
    public enum NV200_RejectionCodes : byte
    {
        NoteAccepted = 0x00,
        NoteLengthIncorrect = 0x01,
        InvalidNote1 = 0x02,
        InvalidNote2 = 0x03,
        InvalidNote3 = 0x04,
        InvalidNote4 = 0x05,
        ChannelInhibited = 0x06,
        SecondNoteInserted = 0x07,
        HostRejecteNote = 0x08,
        InvalidNote5 = 0x09,
        InvalidNoteRead = 0x0A,
        NoteTooLong = 0x0B,
        ValidatorDisabled = 0x0C,
        MechanismSlowOrStalled = 0x0D,
        StrimmingAttempt = 0x0E,
        FraudChannelReject = 0x0F,
        NoNotesInserted = 0x10,
        PeakDetectFail = 0x11,
        TwistedNoteDetected = 0x12,
        EscrowTimeOut = 0x13,
        BarCodeScanFail = 0x14,
        InvalidNote6 = 0x15,
        InvalidNote7 = 0x16,
        InvalidNote8 = 0x17,
        InvalidNote9 = 0x18,
        IncorrectNoteWidth = 0x19,
        NoteTooShort = 0x1A
    }
}
