using System.Collections.Generic;

namespace Palavyr.API.Controllers.Testing
{
    public class TestDataProvider
    {
        public TestDataProvider() { }

        public IReadOnlyCollection<string> ProvideData()
        {
            return new [] {"One", "Two", "Three"};
        }
    }
}