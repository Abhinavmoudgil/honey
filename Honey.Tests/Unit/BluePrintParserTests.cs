namespace Honey.Tests.Unit
{
    using System.Linq;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BluePrintParserTests
    {
        [TestMethod]
        public void ParseBluePrint_BasicBluePrintWith3Actions_ShouldParseAndReturnBom()
        {            
            var parser = new BluePrintParser();
            
            var resources = parser.Parse(@"GET /product/{id}
	            < 200
	            < Accept: application/json
	            DELETE /product/{id}
	            < 200
	            < Accept: application/json
	            GET /order/{id}
	            < 200
	            < Accept: application/json
            ");

            resources.SelectMany(resourceGroup => resourceGroup).Should().HaveCount(3);
        }

        [TestMethod]
        public void ParseBluePrint_BluePrintActionWithParameters_ShouldParseAndReturnBom()
        {            
            var parser = new BluePrintParser();

            var resources = parser.Parse(@"
< Accept: application/json
GET /product{?category,priceFrom,priceTo}
< 200
< Accept: application/json");

            resources.SelectMany(resourceGroup => resourceGroup).Should().HaveCount(1);

            resources.SelectMany(resourceGroup => resourceGroup).First().Parameters.Should().NotBeNull();
        }        
    }
}