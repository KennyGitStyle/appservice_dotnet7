using System.ComponentModel.DataAnnotations;

namespace Northwind.Console.HierarchyMapping
{
    public abstract class Person
    {
        public int Id { get; set; }
        [StringLength(40)]
        public required string? Name { get; set; }
    }
}
