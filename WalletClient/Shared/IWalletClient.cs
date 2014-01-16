using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WalletClient.Shared.Model;
using WalletClient.Shared.Model.RawTransactions;

namespace WalletClient.Shared
{
    public interface IWalletClient
    {
        Uri Uri { get; set; }
        NetworkCredential Credentials { get; set; }

        string AddMultiSigAddress(int required, IEnumerable<string> keys, string account);
        Task<string> AddMultiSigAddressAsync(int required, IEnumerable<string> keys, string account);
        void AddNode(string node, AddNodeAction action);
        Task AddNodeAsync(string node, AddNodeAction action);
        Transaction GetTransaction(string transactionId);
        Task<Transaction> GetTransactionAsync(string transactionId);
        IEnumerable<UnspentTransaction> ListUnspentTransactions(int minConfirmations, int maxConfirmations);
        Task<IEnumerable<UnspentTransaction>> ListUnspentTransactionsAsync(int minConfirmations, int maxConfirmations);
        void SetTransactionFee(decimal amount);
        Task SetTransactionFeeAsync(decimal amount);
        string GetAccount(string address);
        Task<string> GetAccountAsync(string address);
        IEnumerable<string> GetAddressesByAccount(string account);
        Task<IEnumerable<string>> GetAddressesByAccountAsync(string account);
        string GetAccountAddress(string account);
        Task<string> GetAccountAddressAsync(string account);
        decimal GetBalance(string account, int minConfirmations);
        Task<decimal> GetBalanceAsync(string account, int minConfirmations);
        Block GetBlock(string hash);
        Task<Block> GetBlockAsync(string hash);
        string GetBlockHash(int index);
        Task<string> GetBlockHashAsync(int index);
        Int32 GetBlockCount();
        Task<Int32> GetBlockCountAsync();
        int GetConnectionCount();
        Task<Int32> GetConnectionCountAsync();
        string SendToAddress(string toAddress, decimal amount, string comment, string toComment);
        Task<string> SendToAddressAsync(string toAddress, decimal amount, string comment, string toComment);
        string SendMany(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations, string comment);
        Task<string> SendManyAsync(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations, string comment);
        IEnumerable<AccountInfo> ListAccounts(int minConfirmations);
        Task<IEnumerable<AccountInfo>> ListAccountsAsync(int minConfirmations);
        string GetNewAddress(string account);
        Task<string> GetNewAddressAsync(string account);
        void LockWallet();
        Task LockWalletAsync();
        void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan);
        Task SetWalletPassphraseAsync(string passPhrase, TimeSpan timeSpan);
        AddressValidation ValidateAddress(string address);
        Task<AddressValidation> ValidateAddressAsync(string address);
        void EncryptWallet(string passPhrase);
        Task EncryptWalletAsync(string passPhrase);
        IEnumerable<WalletTransaction> ListTransactions(string account, int count, int startingIndex);
        Task<IEnumerable<WalletTransaction>> ListTransactionsAsync(string account, int count, int startingIndex);
        bool Move(string fromAddress, string toAddress, decimal amount, int minConfirmations, string comment);
        Task<bool> MoveAsync(string fromAddress, string toAddress, decimal amount, int minConfirmations, string comment);
    }
}
