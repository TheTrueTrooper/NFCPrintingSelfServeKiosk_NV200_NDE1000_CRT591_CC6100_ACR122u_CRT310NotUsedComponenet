using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinChanger_MDBRS232_For_CC6100.MDBEnums.CoinChangerEnums
{
    enum CC6100MDB_CoinChangerLevels
    {
        /// <summary>
        /// never released
        /// </summary>
        CoinChangerLevel1,
        /// <summary>
        /// This level is as follows
        ///  For level2 changers, 
        ///  VMC operation consists of monitoringinputs from the coin mechanism, accumulating credit, 
        ///  issuing acoinacceptancedisable command when appropriate, 
        ///  and issuing appropriate payout commands based on the VMCresident payout algorithms 
        ///  and escrow rules.
        /// </summary>
        CoinChangerLevel2,
        /// <summary>
        /// This level is as follows
        ///  For level3 changers,
        ///  Same as level2 but with the addition of the EXPANSIONcommand and its implications.
        ///     level2 summery:
        ///         "VMC operation consists of monitoringinputs from the coin mechanism, accumulating credit, 
        ///         issuing acoinacceptancedisable command when appropriate, 
        ///         and issuing appropriate payout commands based on the VMCresident payout algorithms 
        ///         and escrow rules."
        ///     level 3 additional extentions:
        ///     The VMC has the option of sending the EXPANSION command to the coinmechanism to determine the coin 
        ///     mechanism’smanufacturercode,serial number,model/tuningrevision,software version,
        ///     and optional features.Based on the optionalfeature information the VMC will determine the appropriateoperating mode 
        ///     (in other words,modes that both the coinmechanism and the VMC can support),   
        ///     enable anyappropriatecoin mechanism features by sending an appropriate featureenable command back to the coin mechanism,
        ///     and enter theproper operating mode.
        ///     This technique allows all VMCs and peripherals to accommodate existing feature capabilities and provides a means for
        ///     upgrading Level 3 equipment.
        /// </summary>
        CoinChangerLevel3
    }
}
