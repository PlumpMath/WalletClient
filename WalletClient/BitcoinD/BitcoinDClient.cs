using System;
using System.Collections.Generic;
using System.Net;
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

        public WalletInfo GetWalletInfo()
        {
            WalletRequest walletRequest = new WalletRequest("getinfo");
            return RpcRequest<WalletInfo>(walletRequest);        
        }

        public void Stop()
        {
            WalletRequest walletRequest = new WalletRequest("stop");
            RpcRequest<string>(walletRequest);
            //{"result":"Bitcoin server stopping","error":null,"id":"1"}
        }

        public void ChangeWalletPassphrase(string oldPassPhrase, string newPassPhrase)
        {
            WalletRequest walletRequest = new WalletRequest("walletpassphrasechange", new List<object>() { oldPassPhrase, newPassPhrase });
            RpcRequest<string>(walletRequest);
            //{"result":null,"error":{"code":-14,"message":"Error: The wallet passphrase entered was incorrect."},"id":"1"}
        }


        public string CreateRawTransaction(IEnumerable<object> inputs, IDictionary<string, decimal> outputs)
        {
            throw new NotImplementedException();
        }

        public RawTransaction DecodeRawTransaction(string transactionHex)
        {
            WalletRequest walletRequest = new WalletRequest("decoderawtransaction", new List<object>() { transactionHex });
            return RpcRequest<RawTransaction>(walletRequest);
        }

        public RawTransactionInfo GetRawTransaction(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 1 });
            return RpcRequest<RawTransactionInfo>(walletRequest);
        }

        public string GetRawTransactionHex(string transactionId)
        {
            WalletRequest walletRequest = new WalletRequest("getrawtransaction", new List<object>() { transactionId, 0 });
            return RpcRequest<string>(walletRequest);
        }
    }
}
