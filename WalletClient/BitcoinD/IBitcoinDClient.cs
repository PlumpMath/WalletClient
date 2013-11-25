using WalletClient.BitcoinD.Model;
using WalletClient.Shared;
using WalletClient.Shared.Model;

namespace WalletClient.BitcoinD
{
    public interface IBitcoinDClient : IWalletClient
    {
        WalletInfo GetWalletInfo();
        void ChangeWalletPassphrase(string oldPassPhrase, string newPassPhrase);
        void Stop();
    }
}
