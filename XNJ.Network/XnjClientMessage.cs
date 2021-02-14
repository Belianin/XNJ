namespace XNJ.Network
{
    public class XnjClientMessage
    {
        public virtual string Type { get; set; }
    }

    public class XnjClientMoveMessage : XnjClientMessage
    {
        public override string Type => "Move";
        public int X { get; set; }
        public int Y { get; set; }
    }
}