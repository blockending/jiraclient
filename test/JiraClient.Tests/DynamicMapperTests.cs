using System.Collections.Generic;
using JiraClient.Mapping;
using Xunit;

public class DynamicMapperTests
{
    private class Source
    {
        public string Id { get; set; } = "";
        public Inner Inner { get; set; } = new();
    }

    private class Inner
    {
        public string Name { get; set; } = "";
    }

    private class Target
    {
        public int Identifier { get; set; }
        public string InnerName { get; set; } = "";
    }

    private class SourceCollection
    {
        public List<Item> Items { get; set; } = new();
    }

    private class Item
    {
        public string Value { get; set; } = "";
    }

    private class TargetCollection
    {
        public List<string> Values { get; set; } = new();
    }

    [Fact]
    public void Map_HandlesNestedPropertiesAndTypeConversion()
    {
        var source = new Source { Id = "42", Inner = new Inner { Name = "test" } };
        var target = new Target();
        var mapping = new Dictionary<string, string>
        {
            ["Identifier"] = "Id",
            ["InnerName"] = "Inner.Name"
        };

        DynamicMapper.Map(source, target, mapping);

        Assert.Equal(42, target.Identifier);
        Assert.Equal("test", target.InnerName);
    }

    [Fact]
    public void Map_HandlesCollections()
    {
        var source = new SourceCollection
        {
            Items = new List<Item> { new Item { Value = "a" }, new Item { Value = "b" } }
        };
        var target = new TargetCollection();
        var mapping = new Dictionary<string, string>
        {
            ["Values"] = "Items.Value"
        };

        DynamicMapper.Map(source, target, mapping);

        Assert.Equal(new[] { "a", "b" }, target.Values);
    }
}
