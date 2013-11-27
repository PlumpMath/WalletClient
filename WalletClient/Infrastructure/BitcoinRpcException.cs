using System;
using WalletClient.Shared.Model;

namespace WalletClient.Infrastructure
{
    public class BitcoinRpcException : Exception
    {
        public BitcoinError Error {get; private set;}

        public BitcoinRpcException(BitcoinError bitcoinError) : base(bitcoinError.Message)
        {
            Error = bitcoinError;
        }

        public BitcoinRpcException(BitcoinError bitcoinError, Exception innerException): base(bitcoinError.Message, innerException)
        {
            Error = bitcoinError;
        }
    }
}
