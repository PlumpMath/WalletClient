using System.Collections.Generic;
using WalletClient.Bitcoind.Model;
using WalletClient.Shared;
using WalletClient.Shared.Model.RawTransactions;

namespace WalletClient.Bitcoind
{
    public interface IBitcoindClient : IWalletClient
    {
        /// <summary>
        /// Backs up the wallet to another location.
        /// </summary>
        /// <param name="destination">A full path in unix format, such as ""d:/wallet-backup/" or a file name such as "backupwallet.dat"</param>
        void BackupWallet(string destination);

        /// <summary>
        /// Gets summary information about your wallet and the Bitcoin system
        /// </summary>
        /// <returns>A populated <see cref="WalletInfo"/> class.</returns>
        WalletInfo GetWalletInfo();
        
        /// <summary>
        /// Changes the encryption 
        /// </summary>
        /// <param name="oldPassPhrase"></param>
        /// <param name="newPassPhrase"></param>
        void ChangeWalletPassphrase(string oldPassPhrase, string newPassPhrase);
        
        /// <summary>
        /// Stops the Bitcoind server
        /// </summary>
        void Stop();


        string CreateRawTransaction(IEnumerable<object> inputs, IDictionary<string, decimal> outputs);
        RawTransaction DecodeRawTransaction(string transactionHex);
        RawTransactionInfo GetRawTransaction(string transactionId);
        string GetRawTransactionHex(string transactionId);
    }
}
