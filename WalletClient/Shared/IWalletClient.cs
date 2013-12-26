using System;
using System.Collections.Generic;
using WalletClient.Shared.Model;
using WalletClient.Shared.Model.RawTransactions;

namespace WalletClient.Shared
{
    public interface IWalletClient
    {
        string AddMultiSigAddress(int required, IEnumerable<string> keys, string account);
        void AddNode(string node, AddNodeAction action);

        Transaction GetTransaction(string transactionId);
        List<UnspentTransaction> ListUnspentTransactions(int minConfirmations, int maxConfirmations);
        void SetTransactionFee(decimal amount);
        string GetAccount(string address);
        decimal GetBalance(string account, int minConfirmations);
        Block GetBlock(string hash);
        string GetBlockHash(int index);
        int GetBlockCount();
        int GetConnectionCount();
        string SendToAddress(string toAddress, decimal amount, string comment, string toComment);
        string SendMany(string fromAccount, IDictionary<string, decimal> toAccounts, int minConfirmations, string comment);
        IEnumerable<AccountInfo> ListAccounts(int minConfirmations);
        string GetNewAddress(string account);
        void LockWallet();
        void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan);
        AddressValidation ValidateAddress(string address);
        void EncryptWallet(string passPhrase);
        IEnumerable<WalletTransaction> ListTransactions(string account, int count, int startingIndex);
        bool Move(string fromAddress, string toAddress, decimal amount, int minConfirmations, string comment);
    }
}
