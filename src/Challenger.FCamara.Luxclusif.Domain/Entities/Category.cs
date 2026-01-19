using Challenger.FCamara.Luxclusif.Domain.Entities.Base;

namespace Challenger.FCamara.Luxclusif.Domain.Entities;

public sealed class Category : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Shortcode { get; private set; } = string.Empty;

    // Self-referencing relationship (parent category)
    public Guid? ParentCategoryId { get; private set; }

    public Category? ParentCategory { get; private set; }

    // Navigation properties
    public ICollection<Category> SubCategories { get; private set; }

    public ICollection<Product> Products { get; private set; }

    private Category()
    {
        SubCategories = [];
        Products = [];
    }

    private Category(string name, string shortcode, Guid? parentCategoryId = null) : this()
    {
        Name = name;
        Shortcode = shortcode.ToUpperInvariant();
        ParentCategoryId = parentCategoryId;
    }

    public static Category Create(string name, string shortcode, Guid? parentCategoryId = null)
    {
        return new Category(name, shortcode, parentCategoryId);
    }

    public void Update(string name, string shortcode, Guid? parentCategoryId)
    {
        // TODO: Previne referencia circular
        if (parentCategoryId.HasValue && parentCategoryId.Value == Id)
            throw new InvalidOperationException("Uma categoria não pode ser sua própria categoria pai.");

        Name = name;
        Shortcode = shortcode.ToUpperInvariant();
        ParentCategoryId = parentCategoryId;
        SetUpdatedAt();
    }
}