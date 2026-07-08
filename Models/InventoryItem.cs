using System;

namespace ITInventoryManager.Models;

public class InventoryItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AssetTag { get; set; } = string.Empty;
    public string EquipmentType { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    public string Status { get; set; } = InventoryStatuses.InStock;
    public string Location { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}

public static class InventoryStatuses
{
    public const string InStock = "In Stock";
    public const string InUse = "In Use";
    public const string Faulty = "Faulty";

    public static readonly string[] All =
    [
        InStock,
        InUse,
        Faulty
    ];
}

public static class EquipmentTypes
{
    public static readonly string[] All =
    [
        "Laptop",
        "Desktop",
        "Monitor",
        "Keyboard",
        "Mouse",
        "Printer",
        "Network Equipment",
        "Other"
    ];
}
