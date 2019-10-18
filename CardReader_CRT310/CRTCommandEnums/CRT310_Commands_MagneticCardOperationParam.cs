namespace CardReader_CRT_310
{
    enum CRT310_Commands_MagneticCardOperationParam
    {
        OnlyUploadISO1TrackData = 0x31,
        OnlyUploadISO2TrackData = 0x32,
        OnlyUploadISO3TrackData = 0x33,
        ClearDataInMagneticRegisterWithoutMoving = 0x36,
        OnlyCheckDataStatusOfMagneticRegister = 0x37,
        ReadISO1TrackDataAsBinary = 0x51,
        ReadISO2TrackDataAsBinary = 0x52,
        ReadISO3TrackDataAsBinary = 0x53
    }
}
