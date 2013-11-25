﻿using System;
using System.Net;
using WalletClient.BlockChainInfo.Model;
using WalletClient.Infrastructure;
using WalletClient.Shared;

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
            WalletRequest walletRequest = new WalletRequest("getinfo");
            return RpcRequest<WalletInfo>(walletRequest);    
        }
    }
}
