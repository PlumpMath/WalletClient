using System;
using System.Collections.Generic;
using System.Net;
using WalletClient.BlockChainInfo.Model;
using WalletClient.Infrastructure;
using WalletClient.Shared;
using WalletClient.Shared.Model;

namespace WalletClient.BlockChainInfo
{
    public class BlockChainInfoClient : BaseClient, IBlockChainInfoClient
    {
        
        public BlockChainInfoClient(){}

        public BlockChainInfoClient(Uri uri)
        {
            Uri = uri;
        }

        public BlockChainInfoClient(Uri uri, NetworkCredential credentials)
        {
            Uri = uri;
            Credentials = credentials;
        }

        
        public WalletInfo GetWalletInfo()
        {
            var response = GetString("getinfo");
            return Mapper<WalletInfo>.MapFromJson(response);      
        }
    }
}
