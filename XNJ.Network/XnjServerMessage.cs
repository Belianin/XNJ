namespace XNJ.Network
{
    public class XnjServerMessage
    {
        public virtual string Type { get; set; }
    }

    public class PlayerServerMessage : XnjServerMessage
    {
        public override string Type => "Player";
        public Player Player { get; set; }
    }
}