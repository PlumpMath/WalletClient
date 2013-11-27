using System;
using System.Collections.Generic;
using System.Net;
using WalletClient.Bitcoind.Model;
using WalletClient.Infrastructure;
using WalletClient.Shared;

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
    }
}
