using WalletClient.BlockChainInfo.Model;
using WalletClient.Shared;
using WalletClient.Shared.Model;

namespace WalletClient.BlockChainInfo
{
    public interface IBlockChainInfoClient : IWalletClient
    {
        WalletInfo GetWalletInfo();
        
    }
}
