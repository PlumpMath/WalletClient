WalletClient/.NET
=================

WalletClient/.NET is a .NET wrapper for Bitcoin's BitcoinD and BlockChain.io's online wallet.  Using WalletClient, you can perform most Bitcoin functions such as creating transactions
and reading the blockchain.

The library is divided into two namespaces: one for BitcoinD and one for BlockChain.io.  You should choose the namespace depending on what service you plan to use.  BlockChain.io is similar to BitcoinD, but
it omits several BitcoinD commands and in some cases has a slightly different result shape.  You can learn more about the BlockChain.io API on the [BlockChain developer page]{https://blockchain.info/api/json_rpc_api}.

Acknowledgements:
-----------------
I reviewed and borrowed from the following examples in building this library:
* [https://github.com/mb300sd/Bitcoin.NET] (https://github.com/mb300sd/Bitcoin.NET)
* [http://sourceforge.net/projects/bitnet/] (http://sourceforge.net/projects/bitnet/)
* [https://github.com/jaymedavis/stripe.net] (https://github.com/jaymedavis/stripe.net)
