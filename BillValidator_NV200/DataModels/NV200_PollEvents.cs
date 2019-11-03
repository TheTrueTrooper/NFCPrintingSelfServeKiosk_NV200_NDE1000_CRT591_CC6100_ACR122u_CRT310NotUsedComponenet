namespace BillValidator_NV200
{
    public class NV200_PollEvents
    {
        public NV200_ChannelFlags? Channel { get; internal set; } = null;
        public NV200_PollStatusFlags EventType { get; internal set; }
    }
}
