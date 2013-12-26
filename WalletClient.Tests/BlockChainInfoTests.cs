using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WalletClient.BlockChainInfo;

namespace WalletClient.Tests
{
    [TestClass]
    public class BlockChainInfoTests
    {

        private const string UserName = "name";
        private const string Password = "pass";
        private const string Url = "https://rpc.blockchain.info:443";

        private Uri uri;
        private NetworkCredential credential;
        private BlockChainInfoClient client;

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
            client = new BlockChainInfoClient(uri, credential);
        }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CanGetInfo()
        {
            var info = client.GetWalletInfo();
            Assert.IsNotNull(info, "Wallet info is null");
        }
        
    }
}
