using System;

namespace CardReader_CRT_591
{
    /// <summary>
    /// A interface for all cards to implement. Mostly to allow referencing and type casting via basic enum tree and to allow for deactivates to be more general
    /// </summary>
    public interface CRT591_ICard : IDisposable
    {
        /// <summary>
        /// indicates the family of the card
        /// </summary>
        CRT591_CardTypes CardBaseType { get; }

        /// <summary>
        /// Indicates if the card is active or not.
        /// </summary>
        bool Active { get; }
    }
}
