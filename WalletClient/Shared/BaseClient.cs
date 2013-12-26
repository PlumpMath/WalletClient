using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using WalletClient.Infrastructure;
using WalletClient.Shared.Model;
using WalletClient.Shared.Model.RawTransactions;

namespace WalletClient.Shared
{
    public abstract class BaseClient : IWalletClient
    {
        public Uri Uri;
        public NetworkCredential Credentials;
        private int id = 0;
        private const string UserAgent = "WalletClient/.NET 0.1";

        protected string MakeRequest(string jsonRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Credentials = Credentials;

            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            webRequest.UserAgent = UserAgent;
            webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Credentials.UserName, Credentials.Password))));

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);
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
                using (HttpWebResponse response = (HttpWebResponse)ex.Response)
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

        protected T RpcRequest<T>(WalletRequest walletRequest)
        {
            id++;
            walletRequest.UpdateId(id);
            string jsonRequest = JsonConvert.SerializeObject(walletRequest);
            string result = MakeRequest(jsonRequest);
            WalletResponse<T> walletResponse = JsonConvert.DeserializeObject<WalletResponse<T>>(result, new UnixDateTimeConverter(), new JsonEnumTypeConverter<TransactionCategory>());
            
            if (walletResponse.Error != null)
            {
                throw new BitcoinRpcException(walletResponse.Error);
            }
            return walletResponse.Result;
        }

        public string AddMultiSigAddress(int required, IEnumerable<string> keys, string account = "")
        {
            WalletRequest walletRequest = new WalletRequest("addmultisigaddress", new List<object>() { required, keys, account });
            return RpcRequest<string>(walletRequest);
        }

        public void AddNode(string node, AddNodeAction action)
        {
            WalletRequest walletRequest = new WalletRequest("addnode", new List<object>() { node, action.ToString().ToLowerInvariant() });
            RpcRequest<object>(walletRequest);
        }

        public Transaction GetTransaction(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("gettransaction", new List<object>() { transactionId });
            return RpcRequest<Transaction>(walletRequest);
        }

        public List<UnspentTransaction> ListUnspentTransactions(int minConfirmations = 1, int maxConfirmations = 999999)
        {
            WalletRequest walletRequest = new WalletRequest("listunspent", new List<object>() { minConfirmations, maxConfirmations });
            return RpcRequest<List<UnspentTransaction>>(walletRequest);
        }

        public void SetTransactionFee(decimal amount)
        {
            WalletRequest walletRequest = new WalletRequest("settxfee", new List<object>() { Math.Round(amount, 8) });
            RpcRequest<string>(walletRequest);
        }

        public string GetAccount(string address)
        {
            WalletRequest walletRequest = new WalletRequest("getaccount", new List<object>() { address });
            return RpcRequest<string>(walletRequest);
        }

        public decimal GetBalance(string account = "*", int minConfirmations = 1)
        {
            WalletRequest walletRequest = new WalletRequest("getbalance", new List<object>() { account, minConfirmations });
            return RpcRequest<decimal>(walletRequest);

        }

        public Block GetBlock(string hash)
        {
            WalletRequest walletRequest = new WalletRequest("getblock", new List<object>() { hash });
            return RpcRequest<Block>(walletRequest);            
        }

        public string GetBlockHash(int index)
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount", new List<object>(){index});
            return RpcRequest<string>(walletRequest);
        }

        public int GetBlockCount()
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount");
            return RpcRequest<int>(walletRequest);
        }

        public int GetConnectionCount()
        {
            WalletRequest walletRequest = new WalletRequest("getconnectioncount");
            return RpcRequest<int>(walletRequest); 
        }

        public string SendToAddress(string address, decimal amount, string comment = "", string visibleComment = "")
        {
            WalletRequest walletRequest = new WalletRequest("sendtoaddress", new List<object>() { address, Math.Round(amount, 8), comment, visibleComment });
            return RpcRequest<string>(walletRequest);
        }

        public string SendMany(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations = 1, string comment = "")
        {

            WalletRequest walletRequest = new WalletRequest("sendmany",
                                                            new object[] { fromAccount, toAccounts, minConfirmations, comment });
            return RpcRequest<string>(walletRequest);                                            
        }

        public IEnumerable<AccountInfo> ListAccounts(int minConfirmations = 1)
        {
            WalletRequest walletRequest = new WalletRequest("listaccounts", new List<object>() { minConfirmations });
            
            List<AccountInfo> accounts = null;
            var response = MakeRequest(walletRequest.ToJsonString());
            var accountObj = JObject.Parse(response).SelectToken("result");
            if (accountObj is JContainer)
            {
                JContainer container = (JContainer)accountObj;
                accounts = new List<AccountInfo>();
                foreach (JProperty child in container.Children())
                {
                    AccountInfo account = new AccountInfo();
                    account.Name = child.Name;
                    account.Balance = child.Value.ToObject<decimal>();
                    accounts.Add(account);
                }
            }
            return accounts;
        }

        public string GetNewAddress(string account = "")
        {
            WalletRequest walletRequest = new WalletRequest("getnewaddress", new List<object>() { account });
            return RpcRequest<string>(walletRequest);
        }

        public void LockWallet()
        {
            var response = MakeRequest("walletlock");
            //{"result":null,"error":{"code":-15,"message":"Error: running with an unencrypted wallet, but walletlock was called."},"id":"1"}
            Console.WriteLine(response);
        }

        public void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrase", new List<object>() { passPhrase, Convert.ToInt32(timeSpan.TotalSeconds) });
            RpcRequest<string>(walletRequest);            
        }

        public AddressValidation ValidateAddress(string address)
        {
            WalletRequest walletRequest = new WalletRequest("validateaddress", new List<object>() { address });
            return RpcRequest<AddressValidation>(walletRequest);
        }

        public void EncryptWallet(string passPhrase)
        {
            //{"result":"wallet encrypted; Bitcoin server stopping, restart to run with encrypted wallet. The keypool has been flushed, you need to make a new backup.","error":null,"id":"1"}
            WalletRequest walletRequest = new WalletRequest("encryptwallet", new List<object>() { passPhrase });
            RpcRequest<AddressValidation>(walletRequest);
        }

        public IEnumerable<WalletTransaction> ListTransactions(string account = "*", int count = 10, int startingIndex = 0)
        {
            WalletRequest walletRequest = new WalletRequest("listtransactions", new List<object>() { account, count, startingIndex });
            return RpcRequest<IEnumerable<WalletTransaction>>(walletRequest);  
        }

        public bool Move(string fromAddress, string toAddress, decimal amount, int minConfirmations = 1, string comment = "")
        {
            WalletRequest walletRequest = new WalletRequest("move", new List<object>() { fromAddress, toAddress, Math.Round(amount, 8), minConfirmations, comment });
            return RpcRequest<bool>(walletRequest);  
        }
    }
}
