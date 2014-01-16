using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WalletClient.Bitcoind.Model;
using WalletClient.Infrastructure;
using WalletClient.Shared;
using WalletClient.Shared.Model.RawTransactions;

namespace WalletClient.Bitcoind
{
    public class BitcoindClient : BaseClient, IBitcoindClient
    {
        public BitcoindClient(){}

        public BitcoindClient(Uri uri)
        {
            Uri = uri;
        }

        public BitcoindClient(Uri uri, NetworkCredential credentials)
        {
            Uri = uri;
            Credentials = credentials;
        }


        public void BackupWallet(string destination)
        {
            WalletRequest walletRequest = new WalletRequest("backupwallet", new List<object>(){destination});
            RpcRequest<string>(walletRequest);
        }

        async public Task BackupWalletAsync(string destination)
        {
            WalletRequest walletRequest = new WalletRequest("backupwallet", new List<object>() { destination });
            await RpcRequestAsync<string>(walletRequest);
        }

        public WalletInfo GetWalletInfo()
        {
            WalletRequest walletRequest = new WalletRequest("getinfo");
            return RpcRequest<WalletInfo>(walletRequest);        
        }

        async public Task<WalletInfo> GetWalletInfoAsync()
        {
            WalletRequest walletRequest = new WalletRequest("getinfo");
            return await RpcRequestAsync<WalletInfo>(walletRequest);
        }

        public void Stop()
        {
            WalletRequest walletRequest = new WalletRequest("stop");
            RpcRequest<string>(walletRequest);
            //{"result":"Bitcoin server stopping","error":null,"id":"1"}
        }

        async public Task StopAsync()
        {
            WalletRequest walletRequest = new WalletRequest("stop");
            await RpcRequestAsync<string>(walletRequest);
            //{"result":"Bitcoin server stopping","error":null,"id":"1"}
        }

        public void ChangeWalletPassphrase(string oldPassPhrase, string newPassPhrase)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrasechange", new List<object>() { oldPassPhrase, newPassPhrase });
            RpcRequest<string>(walletRequest);
            //{"result":null,"error":{"code":-14,"message":"Error: The wallet passphrase entered was incorrect."},"id":"1"}
        }

        async public Task ChangeWalletPassphraseAsync(string oldPassPhrase, string newPassPhrase)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrasechange", new List<object>() { oldPassPhrase, newPassPhrase });
            await RpcRequestAsync<string>(walletRequest);
            //{"result":null,"error":{"code":-14,"message":"Error: The wallet passphrase entered was incorrect."},"id":"1"}
        }


        public string CreateRawTransaction(IEnumerable<object> inputs, IDictionary<string, decimal> outputs)
        {
            throw new NotImplementedException();
        }

        async public Task<string> CreateRawTransactionAsync(IEnumerable<object> inputs, IDictionary<string, decimal> outputs)
        {
            throw new NotImplementedException();
        }

        public RawTransaction DecodeRawTransaction(string transactionHex)
        {
            WalletRequest walletRequest = new WalletRequest("decoderawtransaction", new List<object>() { transactionHex });
            return RpcRequest<RawTransaction>(walletRequest);
        }

        async public Task<RawTransaction> DecodeRawTransactionAsync(string transactionHex)
        {
            WalletRequest walletRequest = new WalletRequest("decoderawtransaction", new List<object>() { transactionHex });
            return await RpcRequestAsync<RawTransaction>(walletRequest);
        }

        public RawTransactionInfo GetRawTransaction(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 1 });
            return RpcRequest<RawTransactionInfo>(walletRequest);
        }
        async public Task<RawTransactionInfo> GetRawTransactionAsync(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 1 });
            return await RpcRequestAsync<RawTransactionInfo>(walletRequest);
        }

        public string GetRawTransactionHex(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 0 });
            return RpcRequest<string>(walletRequest);
        }

        async public Task<string> GetRawTransactionHexAsync(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 0 });
            return await RpcRequestAsync<string>(walletRequest);
        }
    }
}
