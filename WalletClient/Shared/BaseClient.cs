using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
        public Uri Uri { get; set; }
        public NetworkCredential Credentials { get; set; }

        private int id = 0;
        private const string UserAgent = "WalletClient/.NET 0.1";

        protected BaseClient()
        {
            //If you are using a self-signed certificate on the bitcoind server, retain the line below, otherwise you'll generate an exception as the certificate cannot be truested.
            //If you are using a purchased or trusted certificate, you can comment out this line.
            ServicePointManager.ServerCertificateValidationCallback +=
                (s, cert, chain, sslPolicyErrors) => true;
        }

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

        async protected Task<string> MakeRequestAsync(string jsonRequest)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Uri);
            webRequest.Credentials = Credentials;

            webRequest.ContentType = "application/json";
            webRequest.Method = "POST";
            webRequest.UserAgent = UserAgent;
            webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Credentials.UserName, Credentials.Password))));

            byte[] byteArray = Encoding.UTF8.GetBytes(jsonRequest);
            webRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = await webRequest.GetRequestStreamAsync())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (WebResponse webResponse = await webRequest.GetResponseAsync())
                {
                    using (Stream str = webResponse.GetResponseStream())
                    {
                        if (str != null)
                        {
                            using (StreamReader sr = new StreamReader(str))
                            {
                                var data = await sr.ReadToEndAsync();
                                return data;
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

        async protected Task<T> RpcRequestAsync<T>(WalletRequest walletRequest)
        {
            id++;
            walletRequest.UpdateId(id);
            string jsonRequest = await JsonConvert.SerializeObjectAsync(walletRequest);
            string result = await MakeRequestAsync(jsonRequest);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters = new JsonConverter[] { new UnixDateTimeConverter(), new JsonEnumTypeConverter<TransactionCategory>() };
            WalletResponse<T> walletResponse = await JsonConvert.DeserializeObjectAsync<WalletResponse<T>>(result, settings);

            if (walletResponse.Error != null)
            {
                throw new BitcoinRpcException(walletResponse.Error);
            }
            return walletResponse.Result;
        }

        protected T RpcRequest<T>(WalletRequest walletRequest)
        {
            id++;
            walletRequest.UpdateId(id);
            string jsonRequest = JsonConvert.SerializeObject(walletRequest);
            string result = MakeRequest(jsonRequest);
            JsonSerializerSettings settings = new JsonSerializerSettings();          
            settings.Converters = new JsonConverter[]{new UnixDateTimeConverter(), new JsonEnumTypeConverter<TransactionCategory>() };
            WalletResponse<T> walletResponse = JsonConvert.DeserializeObject<WalletResponse<T>>(result, settings);
            
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

        async public Task<string> AddMultiSigAddressAsync(int required, IEnumerable<string> keys, string account = "")
        {
            WalletRequest walletRequest = new WalletRequest("addmultisigaddress", new List<object>() { required, keys, account });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public void AddNode(string node, AddNodeAction action)
        {
            WalletRequest walletRequest = new WalletRequest("addnode", new List<object>() { node, action.ToString().ToLowerInvariant() });
            RpcRequest<object>(walletRequest);
        }

        async public Task AddNodeAsync(string node, AddNodeAction action)
        {
            WalletRequest walletRequest = new WalletRequest("addnode", new List<object>() { node, action.ToString().ToLowerInvariant() });
            await RpcRequestAsync<object>(walletRequest);
        }

        public Transaction GetTransaction(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("gettransaction", new List<object>() { transactionId });
            return RpcRequest<Transaction>(walletRequest);
        }
        async public Task<Transaction> GetTransactionAsync(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("gettransaction", new List<object>() { transactionId });
            return await RpcRequestAsync<Transaction>(walletRequest);
        }

        public IEnumerable<UnspentTransaction> ListUnspentTransactions(int minConfirmations = 1, int maxConfirmations = 999999)
        {
            WalletRequest walletRequest = new WalletRequest("listunspent", new List<object>() { minConfirmations, maxConfirmations });
            return RpcRequest<IEnumerable<UnspentTransaction>>(walletRequest);
        }
        async public Task<IEnumerable<UnspentTransaction>> ListUnspentTransactionsAsync(int minConfirmations = 1, int maxConfirmations = 999999)
        {
            WalletRequest walletRequest = new WalletRequest("listunspent", new List<object>() { minConfirmations, maxConfirmations });
            return await RpcRequestAsync<IEnumerable<UnspentTransaction>>(walletRequest);
        }
        
        public void SetTransactionFee(decimal amount)
        {
            WalletRequest walletRequest = new WalletRequest("settxfee", new List<object>() { Math.Round(amount, 8) });
            RpcRequest<string>(walletRequest);
        }
        async public Task SetTransactionFeeAsync(decimal amount)
        {
            WalletRequest walletRequest = new WalletRequest("settxfee", new List<object>() { Math.Round(amount, 8) });
            await RpcRequestAsync<string>(walletRequest);
        }

        public string GetAccount(string address)
        {
            WalletRequest walletRequest = new WalletRequest("getaccount", new List<object>() { address });
            return RpcRequest<string>(walletRequest);
        }
        async public Task<string> GetAccountAsync(string address)
        {
            WalletRequest walletRequest = new WalletRequest("getaccount", new List<object>() { address });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public decimal GetBalance(string account = "*", int minConfirmations = 1)
        {
            WalletRequest walletRequest = new WalletRequest("getbalance", new List<object>() { account, minConfirmations });
            return RpcRequest<decimal>(walletRequest);
        }

        async public Task<decimal> GetBalanceAsync(string account = "*", int minConfirmations = 1)
        {
            WalletRequest walletRequest = new WalletRequest("getbalance", new List<object>() { account, minConfirmations });
            return await RpcRequestAsync<decimal>(walletRequest);
        }

        public Block GetBlock(string hash)
        {
            WalletRequest walletRequest = new WalletRequest("getblock", new List<object>() { hash });
            return RpcRequest<Block>(walletRequest);            
        }

        async public Task<Block> GetBlockAsync(string hash)
        {
            WalletRequest walletRequest = new WalletRequest("getblock", new List<object>() { hash });
            return await RpcRequestAsync<Block>(walletRequest);
        }

        public string GetBlockHash(int index)
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount", new List<object>(){index});
            return RpcRequest<string>(walletRequest);
        }
        async public Task<string> GetBlockHashAsync(int index)
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount", new List<object>() { index });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public Int32 GetBlockCount()
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount");
            return RpcRequest<Int32>(walletRequest);
        }
        async public Task<Int32> GetBlockCountAsync()
        {
            WalletRequest walletRequest = new WalletRequest("getblockcount");
            return await RpcRequestAsync<Int32>(walletRequest);
        }

        public Int32 GetConnectionCount()
        {
            WalletRequest walletRequest = new WalletRequest("getconnectioncount");
            return RpcRequest<Int32>(walletRequest); 
        }

        async public Task<Int32> GetConnectionCountAsync()
        {
            WalletRequest walletRequest = new WalletRequest("getconnectioncount");
            return await RpcRequestAsync<Int32>(walletRequest);
        }

        public string SendToAddress(string address, decimal amount, string comment = "", string visibleComment = "")
        {
            WalletRequest walletRequest = new WalletRequest("sendtoaddress", new List<object>() { address, Math.Round(amount, 8), comment, visibleComment });
            return RpcRequest<string>(walletRequest);
        }

        async public Task<string> SendToAddressAsync(string address, decimal amount, string comment = "", string visibleComment = "")
        {
            WalletRequest walletRequest = new WalletRequest("sendtoaddress", new List<object>() { address, Math.Round(amount, 8), comment, visibleComment });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public string SendFrom(string fromAccount, string toAddress, decimal amount, int minConfirmations = 1, string comment = "", string toComment = "")
        {
            WalletRequest walletRequest = new WalletRequest("sendfrom",
                                                           new object[] { fromAccount, toAddress, Math.Round(amount, 8), minConfirmations, comment, toComment });
            return RpcRequest<string>(walletRequest); 
        }
        

        async public Task<string> SendFromAsync(string fromAccount, string toAddress, decimal amount, int minConfirmations = 1, string comment = "", string toComment = "")
        {
            WalletRequest walletRequest = new WalletRequest("sendfrom",
                                                           new object[] { fromAccount, toAddress, Math.Round(amount, 8), minConfirmations, comment, toComment });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public string SendMany(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations = 1, string comment = "")
        {

            WalletRequest walletRequest = new WalletRequest("sendmany",
                                                            new object[] { fromAccount, toAccounts, minConfirmations, comment });
            return RpcRequest<string>(walletRequest);                                            
        }

        async public Task<string> SendManyAsync(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations = 1, string comment = "")
        {

            WalletRequest walletRequest = new WalletRequest("sendmany",
                                                            new object[] { fromAccount, toAccounts, minConfirmations, comment });
            return await RpcRequestAsync<string>(walletRequest);
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
                foreach (JToken token in container.Children())
                {
                    if (!(token is JProperty)) continue;
                    JProperty child = (JProperty) token;
                    AccountInfo account = new AccountInfo();
                    account.Name = child.Name;
                    account.Balance = child.Value.ToObject<decimal>();
                    accounts.Add(account);
                }
            }
            return accounts;
        }

        async public Task<IEnumerable<AccountInfo>> ListAccountsAsync(int minConfirmations = 1)
        {
            WalletRequest walletRequest = new WalletRequest("listaccounts", new List<object>() { minConfirmations });

            List<AccountInfo> accounts = null;
            var response = await MakeRequestAsync(walletRequest.ToJsonString());
            var accountObj = JObject.Parse(response).SelectToken("result");
            if (accountObj is JContainer)
            {
                JContainer container = (JContainer)accountObj;
                accounts = new List<AccountInfo>();
                foreach (JToken token in container.Children())
                {
                    if (!(token is JProperty)) continue;
                    JProperty child = (JProperty)token;
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

        async public Task<string> GetNewAddressAsync(string account = "")
        {
            WalletRequest walletRequest = new WalletRequest("getnewaddress", new List<object>() { account });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public void LockWallet()
        {
            var response = MakeRequest("walletlock");
            //{"result":null,"error":{"code":-15,"message":"Error: running with an unencrypted wallet, but walletlock was called."},"id":"1"}
            Console.WriteLine(response);
        }
        async public Task LockWalletAsync()
        {
            var response = await MakeRequestAsync("walletlock");
            //{"result":null,"error":{"code":-15,"message":"Error: running with an unencrypted wallet, but walletlock was called."},"id":"1"}
            Console.WriteLine(response);
        }

        public void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrase", new List<object>() { passPhrase, Convert.ToInt32(timeSpan.TotalSeconds) });
            RpcRequest<string>(walletRequest);            
        }
        async public Task SetWalletPassphraseAsync(string passPhrase, TimeSpan timeSpan)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrase", new List<object>() { passPhrase, Convert.ToInt32(timeSpan.TotalSeconds) });
            await RpcRequestAsync<string>(walletRequest);
        }

        public AddressValidation ValidateAddress(string address)
        {
            WalletRequest walletRequest = new WalletRequest("validateaddress", new List<object>() { address });
            return RpcRequest<AddressValidation>(walletRequest);
        }

        async public Task<AddressValidation> ValidateAddressAsync(string address)
        {
            WalletRequest walletRequest = new WalletRequest("validateaddress", new List<object>() { address });
            return await RpcRequestAsync<AddressValidation>(walletRequest);
        }

        public void EncryptWallet(string passPhrase)
        {
            //{"result":"wallet encrypted; Bitcoin server stopping, restart to run with encrypted wallet. The keypool has been flushed, you need to make a new backup.","error":null,"id":"1"}
            WalletRequest walletRequest = new WalletRequest("encryptwallet", new List<object>() { passPhrase });
            RpcRequest<AddressValidation>(walletRequest);
        }

        async public Task EncryptWalletAsync(string passPhrase)
        {
            //{"result":"wallet encrypted; Bitcoin server stopping, restart to run with encrypted wallet. The keypool has been flushed, you need to make a new backup.","error":null,"id":"1"}
            WalletRequest walletRequest = new WalletRequest("encryptwallet", new List<object>() { passPhrase });
            await RpcRequestAsync<AddressValidation>(walletRequest);
        }

        public IEnumerable<WalletTransaction> ListTransactions(string account = "*", int count = 10, int startingIndex = 0)
        {
            WalletRequest walletRequest = new WalletRequest("listtransactions", new List<object>() { account, count, startingIndex });
            return RpcRequest<IEnumerable<WalletTransaction>>(walletRequest);  
        }

        async public Task<IEnumerable<WalletTransaction>> ListTransactionsAsync(string account = "*", int count = 10, int startingIndex = 0)
        {
            WalletRequest walletRequest = new WalletRequest("listtransactions", new List<object>() { account, count, startingIndex });
            return await RpcRequestAsync<IEnumerable<WalletTransaction>>(walletRequest);
        }

        public bool Move(string fromAddress, string toAddress, decimal amount, int minConfirmations = 1, string comment = "")
        {
            WalletRequest walletRequest = new WalletRequest("move", new List<object>() { fromAddress, toAddress, Math.Round(amount, 8), minConfirmations, comment });
            return RpcRequest<bool>(walletRequest);  
        }

        async public Task<bool> MoveAsync(string fromAddress, string toAddress, decimal amount, int minConfirmations = 1, string comment = "")
        {
            WalletRequest walletRequest = new WalletRequest("move", new List<object>() { fromAddress, toAddress, Math.Round(amount, 8), minConfirmations, comment });
            return await RpcRequestAsync<bool>(walletRequest);
        }

        public string GetAccountAddress(string account)
        {
            WalletRequest walletRequest = new WalletRequest("getaccountaddress", new List<object>() { account });
            return RpcRequest<string>(walletRequest);  
        }

        async public Task<string> GetAccountAddressAsync(string account)
        {
            WalletRequest walletRequest = new WalletRequest("getaccountaddress", new List<object>() { account });
            return await RpcRequestAsync<string>(walletRequest);
        }

        public IEnumerable<string> GetAddressesByAccount(string account)
        {
            WalletRequest walletRequest = new WalletRequest("getaddressesbyaccount", new List<object>() { account });
            return RpcRequest<IEnumerable<string>>(walletRequest); 
        }

        async public Task<IEnumerable<string>> GetAddressesByAccountAsync(string account)
        {
            WalletRequest walletRequest = new WalletRequest("getaddressesbyaccount", new List<object>() { account });
            return await RpcRequestAsync<IEnumerable<string>>(walletRequest);
        }
    }
}
