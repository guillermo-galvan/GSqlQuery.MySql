using Xunit;

namespace GSqlQuery.MySql.Test
{
    [CollectionDefinition("GlobalTestServer", DisableParallelization = true)]
    public class GlobalTestServer : ICollectionFixture<GlobalFixture>
    {
    }
}
