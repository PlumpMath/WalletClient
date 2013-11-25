using WalletClient.Shared.Model;

namespace WalletClient.Infrastructure
{
    public class WalletResponse<T>
    {
        public T Result { get; set; }
        public BitcoinError Error { get; set; }
        public int Id { get; set; }

    }
}
