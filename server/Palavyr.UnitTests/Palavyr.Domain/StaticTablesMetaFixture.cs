using System.Linq;
using NUnit.Framework;
using Palavyr.Core.Models.Configuration.Schemas;

namespace PalavyrServer.Tests.Palavyr.Domain
{
    [TestFixture]
    public class StaticTablesMetaFixture
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BindTemplateList_BindsAllProperties()
        {
            var testAccountId = "test-account";
            var testAreaId = "test-areaId";

            var testStaticTablesMetas = StaticTablesMeta.CreateDefaultMetas(testAreaId, testAccountId);
            
            var result = StaticTablesMeta.BindTemplateList(testStaticTablesMetas, "test-account").First();
            
            Assert.AreEqual(result.AccountId, testAccountId);
            Assert.AreEqual(result.AreaIdentifier, testAreaId);
            Assert.AreEqual(result.Description, "Default Description");
            Assert.AreEqual(result.TableOrder, 0);
        }
    }
}