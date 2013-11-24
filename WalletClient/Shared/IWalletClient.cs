using System;
using System.Collections.Generic;
using WalletClient.Shared.Model;

namespace WalletClient.Shared
{
    public interface IWalletClient
    {
        Transaction GetTransaction(string transactionId);
        void SetTransactionFee(double amount);
        string GetAccount(string address);
        double GetBalance(string account, int minConfirmations);
        Block GetBlock(string hash);
        string GetBlockHash(int index);
        int GetBlockCount();
        int GetConnectionCount();
        string SendToAddress(string address, double amount, string comment, string toComment);
        List<AccountInfo> GetAccounts(int minConfirmations);
        string GetNewAddress(string account);
        void LockWallet(out BitcoinError error);
        void SetWalletPassphrase(string passPhrase, TimeSpan timeSpan, out BitcoinError error);
        AddressValidation ValidateAddress(string address);
        void EncryptWallet(string passPhrase, out BitcoinError error);
        List<WalletTransaction> ListTransactions(string account, int count, int startingIndex);
    }
}
