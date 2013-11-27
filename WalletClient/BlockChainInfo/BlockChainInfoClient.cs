﻿using System;
using System.Collections.Generic;
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


        public void BackupWallet()
        {
            WalletRequest walletRequest = new WalletRequest("backupwallet");
            RpcRequest<string>(walletRequest);
        }

        public WalletInfo GetWalletInfo()
        {
            WalletRequest walletRequest = new WalletRequest("getinfo");
            return RpcRequest<WalletInfo>(walletRequest);    
        }
    }
}
