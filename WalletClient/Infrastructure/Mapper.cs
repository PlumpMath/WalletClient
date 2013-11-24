using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WalletClient.Shared.Model;

namespace WalletClient.Infrastructure
{
    public static class Mapper<T>
    {
        public static List<T> MapCollectionFromJson(string json, string token = "result")
        {
            var list = new List<T>();
            var jObject = JObject.Parse(json);

            var allTokens = jObject.SelectToken(token);
            foreach (var tkn in allTokens)
                list.Add(Mapper<T>.MapFromJson(tkn.ToString(), null));

            return list;
        }

        public static T MapFromJson(string json, string parentToken = "result")
        {
            var jsonToParse = string.IsNullOrEmpty(parentToken) ? json : JObject.Parse(json).SelectToken(parentToken).ToString();

            return JsonConvert.DeserializeObject<T>(jsonToParse, new UnixDateTimeConverter(), new JsonEnumTypeConverter<TransactionCategory>());
        }
    }
}
