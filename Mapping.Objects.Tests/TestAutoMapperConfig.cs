using AutoMapper;
using MappingObjects.Mappers;

namespace Mapping.Objects.Tests
{
    public class TestAutoMapperConfig
    {
        [Fact]
        public void TestSummaryMapping()
        {
            MapperConfiguration config =
                CartToSummaryMapper.GetMappingConfiguration();
            config.AssertConfigurationIsValid();
        }
    }
}