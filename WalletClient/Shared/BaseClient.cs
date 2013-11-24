using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WalletClient.Infrastructure;
using WalletClient.Shared.Model;

namespace WalletClient.Shared
{
    public class BaseClient : IWalletClient
    {
        public Uri Uri;
        public NetworkCredential Credentials;
        private int id = 0;
        private const string UserAgent = "WalletClient/.NET 0.1";

        protected string GetString(string method, params object[] parameters)
        {
            id++;

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Credentials = Credentials;

            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            webRequest.UserAgent = UserAgent;
            webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Credentials.UserName, Credentials.Password))));

            JObject jsonObject = new JObject();
            jsonObject["jsonrpc"] = "2.0";
            jsonObject["id"] = id.ToString(CultureInfo.InvariantCulture);
            jsonObject["method"] = method;

            if (parameters != null && parameters.Length > 0)
            {
                JArray props = new JArray();
                foreach (var p in parameters)
                {                                 
                    if (p != null)
                        props.Add(new JValue(p));
                }
                jsonObject.Add(new JProperty("params", props));
            }

            string s = JsonConvert.SerializeObject(jsonObject);
            //s = @"{""jsonrpc"":""2.0"",""id"":""1"",""method"":""getbalance"",""params"":[,2]}";

            //s = @"{""jsonrpc"":""2.0"",""id"":""1"",""method"":""getbalance"",""params"":[,1]}";
            // serialize json for the request
            byte[] byteArray = Encoding.UTF8.GetBytes(s);
            webRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = webRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (WebResponse webResponse = webRequest.GetResponse())
                {
                    using (Stream str = webResponse.GetResponseStream())
                    {
                        if (str != null)
                        {
                            using (StreamReader sr = new StreamReader(str))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                        return null;
                    }
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = (HttpWebResponse) ex.Response)
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        if (response.StatusCode != HttpStatusCode.InternalServerError)
                        {
                            throw;
                        }
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
     

        public Transaction GetTransaction(string transactionId)
        {
            var response = GetString("gettransaction", transactionId);
            return Mapper<Transaction>.MapFromJson(response);
        }

        public void SetTransactionFee(double amount)
        {
            var response = GetString("settxfee", Math.Round(amount, 8));
            Console.WriteLine(response);
        }

        public string GetAccount(string address)
        {
            var response = GetString("getaccount", address);
            var account = JObject.Parse(response).SelectToken("result");
            return account != null ? account.ToString() : null;
        }

        public double GetBalance(string account = "*", int minConfirmations = 1)
        {
            string response;
            if (!string.IsNullOrEmpty(account) && minConfirmations != 1)
            {
                response = GetString("getbalance", account, minConfirmations);
            }
            else if (!string.IsNullOrEmpty(account))
            {
                response = GetString("getbalance", account);
            }
            else
            {
                response = GetString("getbalance");
            }

            var balance = Mapper<double>.MapFromJson(response);
            return balance;
        }

        public Block GetBlock(string hash)
        {
            var response = GetString("getblock", hash);
            var block = Mapper<Block>.MapFromJson(response);
            return block;
        }

        public string GetBlockHash(int index)
        {
            var response = GetString("getblockhash", index);
            var hash = JObject.Parse(response).SelectToken("result");
            return hash != null ? hash.ToString() : null;
        }

        public int GetBlockCount()
        {
            var response = GetString("getblockcount");
            var blockCount = Mapper<int>.MapFromJson(response);
            return blockCount;
        }

        public int GetConnectionCount()
        {
            var response = GetString("getconnectioncount");
            var connections = Mapper<int>.MapFromJson(response);
            return connections;
        }

        public string SendToAddress(string address, double amount, string comment = "", string visibleComment = "")
        {
            var response = GetString("sendtoaddress", new object[] { address, Math.Round(amount, 8), comment, visibleComment });
            var transactionId = JObject.Parse(response).SelectToken("result");
            return transactionId != null ? transactionId.ToString() : null;
        }

        public List<AccountInfo> GetAccounts(int minConfirmations = 1)
        {
            List<AccountInfo> accounts = null;
            var response = GetString("listaccounts", minConfirmations);
            var accountObj = JObject.Parse(response).SelectToken("result");
            if (accountObj is JContainer)
            {
                JContainer container = (JContainer)accountObj;
                accounts = new List<AccountInfo>();
                foreach (JProperty child in container.Children())
                {
                    AccountInfo account = new AccountInfo();
                    account.Name = child.Name;
                    account.Balance = child.Value.ToObject<double>();
                    accounts.Add(account);
                }
            }
            return accounts;
        }

        public string GetNewAddress(string account = "")
        {
            var response = GetString("getnewaddress", account);
            var address = JObject.Parse(response).SelectToken("result");
            return address != null ? address.ToString() : null;
        }

        public void LockWallet(out BitcoinError error)
        {
            var response = GetString("walletlock");
            error = Mapper<BitcoinError>.MapFromJson(response, "error");
            //{"result":null,"error":{"code":-15,"message":"Error: running with an unencrypted wallet, but walletlock was called."},"id":"1"}
            Console.WriteLine(response);
        }

        public void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan, out BitcoinError error)
        {
            var response = GetString("walletpassphrase", new object[] { passPhrase, Convert.ToInt32(timeSpan.TotalSeconds) });
            //{"result":null,"error":{"code":-15,"message":"Error: running with an unencrypted wallet, but walletpassphrase was called."},"id":"1"}
            error = Mapper<BitcoinError>.MapFromJson(response, "error");
            Console.WriteLine(error.Message);
        }

        public AddressValidation ValidateAddress(string address)
        {
            var response = GetString("validateaddress", address);
            return Mapper<AddressValidation>.MapFromJson(response);
        }

        public void EncryptWallet(string passPhrase, out BitcoinError error)
        {
            var response = GetString("encryptwallet", passPhrase);
            error = Mapper<BitcoinError>.MapFromJson(response, "error");
            //{"result":"wallet encrypted; Bitcoin server stopping, restart to run with encrypted wallet. The keypool has been flushed, you need to make a new backup.","error":null,"id":"1"}
            Console.WriteLine(response);
        }

        public List<WalletTransaction> ListTransactions(string account = "*", int count = 10, int startingIndex = 0)
        {
            var response = GetString("listtransactions", new object[] { account, count, startingIndex });
            var transactions = Mapper<WalletTransaction>.MapCollectionFromJson(response);
            return transactions;
        }
    }
}
