using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ITInventoryManager.Models;

namespace ITInventoryManager.Services;

public class InventoryRepository
{
    private readonly string _dataFilePath;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public InventoryRepository()
    {
        var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(dataDirectory);
        _dataFilePath = Path.Combine(dataDirectory, "inventory-data.json");
    }

    public List<InventoryItem> Load()
    {
        if (!File.Exists(_dataFilePath))
        {
            var sampleItems = CreateSampleItems();
            Save(sampleItems);
            return sampleItems;
        }

        var json = File.ReadAllText(_dataFilePath);
        if (string.IsNullOrWhiteSpace(json))
        {
            return [];
        }

        return JsonSerializer.Deserialize<List<InventoryItem>>(json, _jsonOptions) ?? [];
    }

    public void Save(List<InventoryItem> items)
    {
        var sortedItems = items
            .OrderBy(i => i.AssetTag)
            .ThenBy(i => i.EquipmentType)
            .ToList();

        var json = JsonSerializer.Serialize(sortedItems, _jsonOptions);
        File.WriteAllText(_dataFilePath, json);
    }

    private static List<InventoryItem> CreateSampleItems()
    {
        return
        [
            new InventoryItem
            {
                AssetTag = "PC-001",
                EquipmentType = "Laptop",
                Manufacturer = "Dell",
                Model = "Latitude 5420",
                SerialNumber = "DL5420-1001",
                AssignedTo = "David Cohen",
                Status = InventoryStatuses.InUse,
                Location = "Engineering",
                Notes = "Used by R&D team"
            },
            new InventoryItem
            {
                AssetTag = "MN-001",
                EquipmentType = "Monitor",
                Manufacturer = "HP",
                Model = "E24 G4",
                SerialNumber = "HP24-3001",
                AssignedTo = "David Cohen",
                Status = InventoryStatuses.InUse,
                Location = "Engineering",
                Notes = "External monitor"
            },
            new InventoryItem
            {
                AssetTag = "PC-002",
                EquipmentType = "Desktop",
                Manufacturer = "Lenovo",
                Model = "ThinkCentre M70q",
                SerialNumber = "LN-M70-2002",
                AssignedTo = string.Empty,
                Status = InventoryStatuses.InStock,
                Location = "IT Storage",
                Notes = "Ready for onboarding"
            },
            new InventoryItem
            {
                AssetTag = "PR-001",
                EquipmentType = "Printer",
                Manufacturer = "Brother",
                Model = "HL-L2370DN",
                SerialNumber = "BR-2370-8101",
                AssignedTo = string.Empty,
                Status = InventoryStatuses.Faulty,
                Location = "Office",
                Notes = "Paper jam issue"
            },
            new InventoryItem
            {
                AssetTag = "NW-001",
                EquipmentType = "Network Equipment",
                Manufacturer = "Cisco",
                Model = "CBS350-24T",
                SerialNumber = "CS350-5511",
                AssignedTo = string.Empty,
                Status = InventoryStatuses.InStock,
                Location = "Server Room",
                Notes = "Spare switch"
            }
        ];
    }
}
