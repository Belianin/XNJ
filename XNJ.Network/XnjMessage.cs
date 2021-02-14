namespace XNJ.Network
{
    public class XnjMessage
    {
        public virtual string Type { get; set; }
    }

    public class PlayerMessage : XnjMessage
    {
        public override string Type => "Player";
        public Player Player { get; set; }
    }
}