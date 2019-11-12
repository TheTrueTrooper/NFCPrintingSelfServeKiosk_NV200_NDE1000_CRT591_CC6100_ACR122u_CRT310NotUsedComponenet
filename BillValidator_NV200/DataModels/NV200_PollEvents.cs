namespace BillValidator_NV200
{
    /// <summary>
    /// Any time you poll the machine will return events that have happened inbetween polling
    /// ex a bill was put in then the bill was rejected or accepted
    /// these codes may or may not have a channel
    /// </summary>
    public class NV200_PollEvents
    {
        /// <summary>
        /// (optional) the channel that the bill was on in relation to the event
        /// </summary>
        public byte? Channel { get; internal set; } = null;
        /// <summary>
        /// The events type as an enum
        /// </summary>
        public NV200_PollStatusFlags EventType { get; internal set; }
    }
}
