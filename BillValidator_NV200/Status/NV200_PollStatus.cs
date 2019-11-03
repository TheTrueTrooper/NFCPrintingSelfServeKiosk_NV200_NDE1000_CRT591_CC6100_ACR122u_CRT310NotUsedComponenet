﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillValidator_NV200
{
    enum NV200_PollStatusFlags : byte
    {
        Poll_Tebs_CASHBOX_OUT_OF_SERVICE = 0x90,
        Poll_Tebs_CASHBOX_TAMPER = 0x91,
        Poll_Tebs_CASHBOX_IN_SERVICE = 0x92,
        Poll_Tebs_CASHBOX_UNLOCK_ENABLED = 0x93,
        Poll_Jam_RECOVERY = 0xB0,
        Poll_Error_DURING_PAYOUT = 0xB1,
        Poll_Smart_EMPTYING = 0xB3,
        Poll_Smart_EMPTIED = 0xB4,
        Poll_Channel_DISABLE = 0xB5,
        Poll_Initialising = 0xB6,
        Poll_Coin_Mech_Error = 0xB7,
        Poll_Emptying = 0xC2,
        Poll_Eemptied = 0xC3,
        Poll_Coin_Mech_Jammed = 0xC4,
        Poll_Coin_Mech_ReturnPressed = 0xC5,
        Poll_Payout_OutOfService = 0xC6,
        Poll_Note_FloatRemoved = 0xC7,
        Poll_Note_FloatAttached = 0xC8,
        Poll_Note_Transfered_TO_STACKER = 0xC9,
        Poll_Note_Paid_Into_Stacker_AtPowerUp = 0xCA,
        Poll_Note_Paid_Into_Store_AtPowerUp = 0xCB,
        Poll_Note_Stacking = 0xCC,
        Poll_Note_Dispensed_AtPowerUp = 0xCD,
        Poll_Note_Held_InBezel = 0xCE,
        Poll_BarCodeTicket_Acknowledged = 0xD1,
        Poll_Dispensed = 0xD2,
        Poll_Jammed = 0xD5,
        Poll_Halted = 0xD6,
        Poll_Floating = 0xD7,
        Poll_Floated = 0xD8,
        Poll_TimeOut = 0xD9,
        Poll_Dispensing = 0xDA,
        Poll_Note_Stored_InPayout = 0xDB,
        Poll_Incomplete_Payout = 0xDC,
        Poll_Incomplete_Float = 0xDD,
        Poll_CashBox_Paid = 0xDE,
        Poll_Coin_Credit = 0xDF,
        Poll_Note_Path_Open = 0xE0,
        Poll_Note_Cleared_From_Front = 0xE1,
        Poll_Note_Cleared_To_CashBox = 0xE2,
        Poll_CashBox_Removed = 0xE3,
        Poll_CashBox_Replaced = 0xE4,
        Poll_BarCodeTicket_Validated = 0xE5,
        Poll_Fraud_Attempt = 0xE6,
        Poll_Stacker_Full = 0xE7,
        Poll_Disabled = 0xE8,
        Poll_Unsafe_Note_Jam = 0xE9,
        Poll_Safe_Note_Jam = 0xEA,
        Poll_Note_Stacked = 0xEB,
        Poll_Note_Rejected = 0xEC,
        Poll_Note_Rejecting = 0xED,
        Poll_Credit_Note = 0xEE,
        Poll_Read_Note = 0xEF,
        Poll_SlaveReset = 0xF1
    }
}
