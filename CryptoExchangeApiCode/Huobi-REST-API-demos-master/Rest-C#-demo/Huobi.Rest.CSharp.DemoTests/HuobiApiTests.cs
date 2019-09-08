using Huobi.Rest.CSharp.Demo;
using Huobi.Rest.CSharp.Demo.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Huobi.Rest.CSharp.Demo.Tests
{
    [TestClass()]
    public class HuobiApiTests
    {
        HuobiApi api = new HuobiApi("", "");
        [TestMethod()]
        public void GetAllAccountTest()
        {
            var result = api.GetAllAccount();
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void OrderPlaceTest()
        {
            OrderPlaceRequest req = new OrderPlaceRequest();
            var result = api.OrderPlace(req);
            Assert.IsNull(result);
        }
    }
}