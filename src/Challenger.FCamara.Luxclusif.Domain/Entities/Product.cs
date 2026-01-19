using Challenger.FCamara.Luxclusif.Domain.Entities.Base;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;

namespace Challenger.FCamara.Luxclusif.Domain.Entities;

public sealed class Product : BaseEntity
{
    public Guid SupplierId { get; private set; }
    public Supplier Supplier { get; private set; }

    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }

    public string Description { get; private set; }

    // Acquisition costs
    public decimal AcquisitionCostInSupplierCurrency { get; private set; }

    public decimal AcquisitionCostInUSD { get; private set; }

    // Dates
    public DateTime AcquireDate { get; private set; }

    public DateTime? SoldDate { get; private set; }
    public DateTime? CancelDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    // Status
    public ProductStatus Status { get; private set; }

    // WMS Integration
    public string? WmsProductId { get; private set; }

    private Product()
    { }

    public Product(
        Guid supplierId,
        Guid categoryId,
        string description,
        decimal acquisitionCostInSupplierCurrency,
        decimal acquisitionCostInUSD) : this()
    {
        ValidateDescription(description);
        ValidateCosts(acquisitionCostInSupplierCurrency, acquisitionCostInUSD);

        SupplierId = supplierId;
        CategoryId = categoryId;
        Description = description;
        AcquisitionCostInSupplierCurrency = acquisitionCostInSupplierCurrency;
        AcquisitionCostInUSD = acquisitionCostInUSD;
        AcquireDate = DateTime.UtcNow;
        Status = ProductStatus.Created;
    }

    public void SetWmsProductId(string wmsProductId)
    {
        if (string.IsNullOrWhiteSpace(wmsProductId))
            throw new ArgumentException("O ID do produto WMS não pode estar vazio.", nameof(wmsProductId));

        WmsProductId = wmsProductId;
    }

    public void MarkAsSold()
    {
        if (Status == ProductStatus.Cancelled)
            throw new InvalidOperationException("Produtos cancelados não podem ser vendidos.");

        if (Status == ProductStatus.Returned)
            throw new InvalidOperationException("Produtos devolvidos não podem ser vendidos.");

        Status = ProductStatus.Sold;
        SoldDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsCancelled()
    {
        if (Status != ProductStatus.Created && Status != ProductStatus.Sold)
            throw new InvalidOperationException($"Não é possível cancelar um produto com status {Status}");

        Status = ProductStatus.Cancelled;
        CancelDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsReturned()
    {
        if (Status != ProductStatus.Sold)
            throw new InvalidOperationException("Somente produtos vendidos podem ser devolvidos.");

        Status = ProductStatus.Returned;
        ReturnDate = DateTime.UtcNow;
        SetUpdatedAt();
    }

    private static void ValidateDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição não pode ficar vazia", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("A descrição não pode exceder 500 caracteres.", nameof(description));
    }

    private static void ValidateCosts(decimal supplierCost, decimal usdCost)
    {
        if (supplierCost <= 0)
            throw new ArgumentException("O custo de aquisição na moeda do fornecedor deve ser maior que zero.", nameof(supplierCost));

        if (usdCost <= 0)
            throw new ArgumentException("O custo de aquisição em USD deve ser maior que zero.", nameof(usdCost));
    }
}