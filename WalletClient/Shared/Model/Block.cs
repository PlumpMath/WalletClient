using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WalletClient.Shared.Model
{
    public class Block
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("confirmations")]
        public int Confirmations { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("merkleroot")]
        public string MerkleRoot { get; set; }
        
        [JsonProperty("tx")]
        public List<string> TransactionIds { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("bits")]
        public string Bits { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("previousblockhash")]
        public string PreviousBlockHash { get; set; }

        [JsonProperty("nextblockhash")]
        public string NextBlockHash { get; set; }

        public Block()
        {
            TransactionIds = new List<string>();
        }
    }
}
