﻿namespace BillValidator_NV200
{
    internal enum NV200_Commands : byte
    {
        SSP_CMD_RESET = 0x01,
        SSP_CMD_SET_CHANNEL_INHIBITS = 0x02,
        SSP_CMD_DISPLAY_ON = 0x03,
        SSP_CMD_DISPLAY_OFF = 0x04,
        SSP_CMD_SETUP_REQUEST = 0x05,
        SSP_CMD_HOST_PROTOCOL_VERSION = 0x06,
        SSP_CMD_POLL = 0x07,
        SSP_CMD_REJECT_BANKNOTE = 0x08,
        SSP_CMD_DISABLE = 0x09,
        SSP_CMD_ENABLE = 0x0A,
        SSP_CMD_GET_SERIAL_NUMBER = 0x0C,
        SSP_CMD_UNIT_DATA = 0x0D,
        SSP_CMD_CHANNEL_VALUE_REQUEST = 0x0E,
        SSP_CMD_CHANNEL_SECURITY_DATA = 0x0F,
        SSP_CMD_CHANNEL_RE_TEACH_DATA = 0x10,
        SSP_CMD_SYNC = 0x11,
        SSP_CMD_LAST_REJECT_CODE = 0x17,
        SSP_CMD_HOLD = 0x18,
        SSP_CMD_GET_FIRMWARE_VERSION = 0x20,
        SSP_CMD_GET_DATASET_VERSION = 0x21,
        SSP_CMD_GET_ALL_LEVELS = 0x22,
        SSP_CMD_GET_BAR_CODE_READER_CONFIGURATION = 0x23,
        SSP_CMD_SET_BAR_CODE_CONFIGURATION = 0x24,
        SSP_CMD_GET_BAR_CODE_INHIBIT_STATUS = 0x25,
        SSP_CMD_SET_BAR_CODE_INHIBIT_STATUS = 0x26,
        SSP_CMD_GET_BAR_CODE_DATA = 0x27,
        SSP_CMD_SET_REFILL_MODE = 0x30,
        SSP_CMD_PAYOUT_AMOUNT = 0x33,
        SSP_CMD_SET_DENOMINATION_LEVEL = 0x34,
        SSP_CMD_GET_DENOMINATION_LEVEL = 0x35,
        SSP_CMD_COMMUNICATION_PASS_THROUGH = 0x37,
        SSP_CMD_HALT_PAYOUT = 0x38,
        SSP_CMD_SET_DENOMINATION_ROUTE = 0x3B,
        SSP_CMD_GET_DENOMINATION_ROUTE = 0x3C,
        SSP_CMD_FLOAT_AMOUNT = 0x3D,
        SSP_CMD_GET_MINIMUM_PAYOUT = 0x3E,
        SSP_CMD_EMPTY_ALL = 0x3F,
        SSP_CMD_SET_COIN_MECH_INHIBITS = 0x40,
        SSP_CMD_GET_NOTE_POSITIONS = 0x41,
        SSP_CMD_PAYOUT_NOTE = 0x42,
        SSP_CMD_STACK_NOTE = 0x43,
        SSP_CMD_FLOAT_BY_DENOMINATION = 0x44,
        SSP_CMD_SET_VALUE_REPORTING_TYPE = 0x45,
        SSP_CMD_PAYOUT_BY_DENOMINATION = 0x46,
        SSP_CMD_SET_COIN_MECH_GLOBAL_INHIBIT = 0x49,
        SSP_CMD_SET_GENERATOR = 0x4A,
        SSP_CMD_SET_MODULUS = 0x4B,
        SSP_CMD_REQUEST_KEY_EXCHANGE = 0x4C,
        SSP_CMD_SET_BAUD_RATE = 0x4D,
        SSP_CMD_GET_BUILD_REVISION = 0x4F,
        SSP_CMD_SET_HOPPER_OPTIONS = 0x50,
        SSP_CMD_GET_HOPPER_OPTIONS = 0x51,
        SSP_CMD_SMART_EMPTY = 0x52,
        SSP_CMD_CASHBOX_PAYOUT_OPERATION_DATA = 0x53,
        SSP_CMD_CONFIGURE_BEZEL = 0x54,
        SSP_CMD_POLL_WITH_ACK = 0x56,
        SSP_CMD_EVENT_ACK = 0x57,
        SSP_CMD_GET_COUNTERS = 0x58,
        SSP_CMD_RESET_COUNTERS = 0x59,
        SSP_CMD_COIN_MECH_OPTIONS = 0x5A,
        SSP_CMD_DISABLE_PAYOUT_DEVICE = 0x5B,
        SSP_CMD_ENABLE_PAYOUT_DEVICE = 0x5C,
        SSP_CMD_SET_FIXED_ENCRYPTION_KEY = 0x60,
        SSP_CMD_RESET_FIXED_ENCRYPTION_KEY = 0x61,
        SSP_CMD_REQUEST_TEBS_BARCODE = 0x65,
        SSP_CMD_REQUEST_TEBS_LOG = 0x66,
        SSP_CMD_TEBS_UNLOCK_ENABLE = 0x67,
        SSP_CMD_TEBS_UNLOCK_DISABLE = 0x68
    }
}