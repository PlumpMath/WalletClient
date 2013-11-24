using System;
using System.Net;
using WalletClient.BitcoinD.Model;
using WalletClient.Infrastructure;
using WalletClient.Shared;
using WalletClient.Shared.Model;

namespace WalletClient.BitcoinD
{
    public class BitcoinDClient : BaseClient, IBitcoinDClient
    {
        public BitcoinDClient(){}

        public BitcoinDClient(Uri uri)
        {
            Uri = uri;
        }

        public BitcoinDClient(Uri uri, NetworkCredential credentials)
        {
            Uri = uri;
            Credentials = credentials;
        }

        
        public WalletInfo GetWalletInfo()
        {
            var response = GetString("getinfo");
            return Mapper<WalletInfo>.MapFromJson(response);      
        }

        public void Stop()
        {
            var response = GetString("stop");
            //{"result":"Bitcoin server stopping","error":null,"id":"1"}
        }

        public void ChangeWalletPassphrase(string oldPassPhrase, string newPassPhrase, out BitcoinError error)
        {
            var response = GetString("walletpassphrasechange", new object[] { oldPassPhrase, newPassPhrase });
            error = Mapper<BitcoinError>.MapFromJson(response, "error");
            //{"result":null,"error":{"code":-14,"message":"Error: The wallet passphrase entered was incorrect."},"id":"1"}
            Console.WriteLine(response);
        }
        
    }
}
