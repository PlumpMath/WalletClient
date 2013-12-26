using System;
using System.Linq;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WalletClient.Bitcoind;

namespace WalletClient.Tests
{
    [TestClass]
    public class BitcoinDTests
    {
        private const string UserName = "name";
        private const string Password = "pass";
        private const string Url = "http://127.0.0.1:18332";

        private Uri uri;
        private NetworkCredential credential;
        private BitcoindClient client;

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
            client = new BitcoindClient(uri, credential);
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


        [TestMethod]
        public void CanStopServer()
        {
            client.Stop();
        }

        [TestMethod]
        public void CanChangeWalletPassphrase()
        {
            client.ChangeWalletPassphrase("foo", "foo2");
        }

        [TestMethod]
        public void CanGetRawTransactionHex()
        {
            var result = client.GetRawTransactionHex("092fe4f122a75c3da4bc07ea701baa121a860a670c8e17d79b130e4ebc41b3cc");
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanGetRawTransaction()
        {
            var result = client.GetRawTransaction("13dffdaef097881acfe9bdb5e6338192242d80161ffec264ee61cf23bc9a1164");
            Console.WriteLine(result.ToJsonString());
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanDecodeRawTransactionHex()
        {
            var result = client.GetRawTransactionHex("092fe4f122a75c3da4bc07ea701baa121a860a670c8e17d79b130e4ebc41b3cc");
            Assert.IsNotNull(result);
            var rawTransaction = client.DecodeRawTransaction(result);
            Assert.IsNotNull(rawTransaction);
        }
        
    }
}
