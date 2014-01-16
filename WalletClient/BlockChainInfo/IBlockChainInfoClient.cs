using System.Threading.Tasks;
using WalletClient.BlockChainInfo.Model;
using WalletClient.Shared;

namespace WalletClient.BlockChainInfo
{
    public interface IBlockChainInfoClient : IWalletClient
    {
        /// <summary>
        /// Backs up your wallet file.  You must have previously set the destination in the blockinfo.io web site.
        /// </summary>
        void BackupWallet();

        Task BackupWalletAsync();

        /// <summary>
        /// Gets summary information about your wallet and the Bitcoin system
        /// </summary>
        /// <returns>A populated <see cref="WalletInfo"/> class.</returns>
        WalletInfo GetWalletInfo();

        Task<WalletInfo> GetWalletInfoAsync();
    }
}
