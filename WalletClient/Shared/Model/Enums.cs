
namespace WalletClient.Shared.Model
{
    public enum TransactionCategory
    {
        NotSet,
        Move,
        Send,
        Receive
    }

    public enum AddNodeAction
    {
        Add,
        Remove,
        OneTry
    }
}
