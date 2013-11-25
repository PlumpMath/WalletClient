using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WalletClient.BitcoinD;
using WalletClient.Shared.Model;

namespace WalletClient.Tests
{
    [TestClass]
    public class SharedTests
    {
        private const string UserName = "name";
        private const string Password = "pass";
        private const string Url = "http://127.0.0.1:18332";

        private Uri uri;
        private NetworkCredential credential;
        private BitcoinDClient client;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize()
        {
            uri = new Uri(Url);
            credential = new NetworkCredential(UserName, Password);
            client = new BitcoinDClient(uri, credential);
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void CanGetTransaction()
        {
            var transaction = client.GetTransaction("366a85cfd395da9b85ce59092dffdd0fa9db105b7dfe1d97802e8a3d9729eedc");
            Assert.IsNotNull(transaction, "Transaction is null");
        }

        [TestMethod]
        public void CanSetTransactionFee()
        {
            client.SetTransactionFee(0.0002m);
            var info = client.GetWalletInfo();
            Assert.AreEqual(0.0002, info.Fee);
        }

        [TestMethod]
        public void CanGetAccount()
        {
            string account = client.GetAccount("myPMSUyNVxouBXU6vxivb3yr145NkxrGNu");
            Assert.IsNotNull(account);
        }

        [TestMethod]
        public void CanGetAccountBalance()
        {
            decimal account = client.GetBalance();
            Console.WriteLine(account);
        }

        [TestMethod]
        public void CanGetAccountBalanceForAllAccounts()
        {
            decimal account = client.GetBalance();
            Console.WriteLine(account);
        }

        [TestMethod]
        public void CanListAccounts()
        {
            var accounts = client.ListAccounts(0);
            Assert.IsNotNull(accounts, "Accounts was null");
            Assert.IsTrue(accounts.Any(), "Accounts returned no items");
        }

        [TestMethod]
        public void CanGetConnectionCount()
        {
            var val = client.GetConnectionCount();
            Assert.IsTrue(val > 0, "Connection count was zero");
        }

        [TestMethod]
        public void CanGetBlockCount()
        {
            var val = client.GetBlockCount();
            Assert.IsTrue(val > 0, "Block count was zero");
        }

        [TestMethod]
        public void CanGetBlock()
        {
            var block = client.GetBlock("00000000002eb8feb3cd64d3307a44d538e061b1b63886fa8e2ecf3086259a00");
            Assert.IsNotNull(block, "Block is null");
        }

        [TestMethod]
        public void CanGetBlockHash()
        {
            var block = client.GetBlockHash(100);
            Assert.IsNotNull(block, "Block hash is null");
        }

        [TestMethod]
        public void CanSendCoins()
        {
            var transactionId = client.SendToAddress("myAdTd18SVnGjx9hVq4n5REvDnPZpR1o6c", 0.1m);
            Assert.IsNotNull(transactionId, "TransactionId is null");
        }

        [TestMethod]
        public void CanSendMany()
        {
            IDictionary<string, decimal> payees = new Dictionary<string, decimal>();
            payees.Add( "myAdTd18SVnGjx9hVq4n5REvDnPZpR1o6c", 0.1m );
            var transactionId = client.SendMany("", payees);
            Assert.IsNotNull(transactionId, "TransactionId is null");
        }

        [TestMethod]
        public void CantSendLotsOfCoins()
        {
            var transactionId = client.SendToAddress("myAdTd18SVnGjx9hVq4n5REvDnPZpR1o6c", 100000000);
            Assert.IsNotNull(transactionId, "TransactionId is null");
        }

        [TestMethod]
        public void CanGetNewAddress()
        {
            var address = client.GetNewAddress();
            Assert.IsNotNull(address, "Address is null");
        }

        [TestMethod]
        public void CanGetNewAddressForAccount()
        {
            var address = client.GetNewAddress("Spending money");
            Assert.IsNotNull(address, "Address is null");
        }

        [TestMethod]
        public void CanGetNewAddressCreatesAccount()
        {
            var newAccount = Guid.NewGuid().ToString();
            var address = client.GetNewAddress(newAccount);
            Assert.IsNotNull(address, "Address is null");
            var accounts = client.ListAccounts(0);
            Assert.IsNotNull(accounts.FirstOrDefault(x => x.Name == newAccount), "New account doesn't exist");
        }

        [TestMethod]
        public void CanLockWallet()
        {
            client.LockWallet();
        }

        [TestMethod]
        public void CanEncryptWallet()
        {
            client.EncryptWallet("foo");
        }

        [TestMethod]
        public void CanGetTransactions()
        {
            var transactions = client.ListTransactions();
            Assert.IsTrue(transactions.Any(), "No transactions found");
        }

        [TestMethod]
        public void CanValidateAddress()
        {
            var validation = client.ValidateAddress("myAdTd18SVnGjx9hVq4n5REvDnPZpR1o6c");
            Assert.IsNotNull(validation);
            Assert.IsTrue(validation.IsValid);
            validation = client.ValidateAddress("1234");
            Assert.IsNotNull(validation);
            Assert.IsFalse(validation.IsValid);
        }

        [TestMethod]
        public void CanSetWalletPassphrase()
        {
            client.SetWalletPassphrase("foo2", TimeSpan.FromSeconds(60));
        }

        [TestMethod]
        public void CanMoveFunds()
        {
            var result = client.Move("myPMSUyNVxouBXU6vxivb3yr145NkxrGNu", "myPMSUyNVxouBXU6vxivb3yr145NkxrGNu", 0.01m);
            Assert.IsTrue(result);
        }

    }
}
