using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletClient.Infrastructure
{
    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class WalletRequest
    {
        string jsonrpc = "2.0";
        int id = 1;
        string method;
        
        [JsonProperty(PropertyName="params", NullValueHandling = NullValueHandling.Ignore)]
        IList<Object> requestParams = null;

        public WalletRequest(string method, IList<object> requestParams = null, int id = 1)
        {
                this.method = method;
                this.requestParams = requestParams;
                this.id = id;
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void UpdateId(int newId)
        {
            id = newId;
        }
    }
}
