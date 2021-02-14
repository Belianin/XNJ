using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XNJ.Network
{
    class ServerMessageJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(XnjServerMessage).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var discriminator = (string)obj["Type"];

            XnjServerMessage item;
            switch (discriminator)
            {
                case "Player":
                    item = new PlayerServerMessage();
                    break;
                default:
                    throw new NotImplementedException();
            }

            serializer.Populate(obj.CreateReader(), item);

            return item;

        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
    }
}