using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ITInventoryManager.Models;
using ITInventoryManager.Services;

namespace ITInventoryManager.Forms;

public class MainForm : Form
{
    private readonly InventoryRepository _repository = new();
    private readonly List<InventoryItem> _items;

    private readonly DataGridView _grid = new();
    private readonly TextBox _searchBox = new();
    private readonly ComboBox _statusFilter = new();
    private readonly ComboBox _typeFilter = new();
    private readonly Label _totalLabel = new();
    private readonly Label _inUseLabel = new();
    private readonly Label _inStockLabel = new();
    private readonly Label _faultyLabel = new();

    public MainForm()
    {
        _items = _repository.Load();

        Text = "IT Inventory Manager";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1100, 700);
        Font = new Font("Segoe UI", 10);

        BuildLayout();
        ConfigureGrid();
        RefreshGrid();
    }

    private void BuildLayout()
    {
        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 4,
            ColumnCount = 1,
            Padding = new Padding(16),
        };

        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var titlePanel = new Panel { Dock = DockStyle.Fill };
        var title = new Label
        {
            Text = "IT Inventory Manager",
            AutoSize = true,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            Location = new Point(0, 0)
        };

        var subtitle = new Label
        {
            Text = "Simple tool for managing company IT equipment",
            AutoSize = true,
            ForeColor = Color.DimGray,
            Location = new Point(3, 40)
        };

        titlePanel.Controls.Add(title);
        titlePanel.Controls.Add(subtitle);

        var summaryPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        _totalLabel.BorderStyle = BorderStyle.FixedSingle;
        _inUseLabel.BorderStyle = BorderStyle.FixedSingle;
        _inStockLabel.BorderStyle = BorderStyle.FixedSingle;
        _faultyLabel.BorderStyle = BorderStyle.FixedSingle;

        foreach (var label in new[] { _totalLabel, _inUseLabel, _inStockLabel, _faultyLabel })
        {
            label.AutoSize = false;
            label.Width = 170;
            label.Height = 46;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Margin = new Padding(0, 0, 12, 0);
            label.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            summaryPanel.Controls.Add(label);
        }

        var filterPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 8,
            RowCount = 1,
        };

        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        filterPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));

        var searchLabel = new Label
        {
            Text = "Search:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _searchBox.Dock = DockStyle.Fill;
        _searchBox.PlaceholderText = "Asset tag, employee, serial, model, manufacturer...";
        _searchBox.TextChanged += (_, _) => RefreshGrid();

        var statusLabel = new Label
        {
            Text = "Status:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _statusFilter.Dock = DockStyle.Fill;
        _statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusFilter.Items.Add("All");
        _statusFilter.Items.AddRange(InventoryStatuses.All);
        _statusFilter.SelectedIndex = 0;
        _statusFilter.SelectedIndexChanged += (_, _) => RefreshGrid();

        var typeLabel = new Label
        {
            Text = "Type:",
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        _typeFilter.Dock = DockStyle.Fill;
        _typeFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeFilter.Items.Add("All");
        _typeFilter.Items.AddRange(EquipmentTypes.All);
        _typeFilter.SelectedIndex = 0;
        _typeFilter.SelectedIndexChanged += (_, _) => RefreshGrid();

        var clearButton = new Button
        {
            Text = "Clear",
            Dock = DockStyle.Fill
        };
        clearButton.Click += (_, _) => ClearFilters();

        var refreshButton = new Button
        {
            Text = "Refresh",
            Dock = DockStyle.Fill
        };
        refreshButton.Click += (_, _) => RefreshGrid();

        filterPanel.Controls.Add(searchLabel, 0, 0);
        filterPanel.Controls.Add(_searchBox, 1, 0);
        filterPanel.Controls.Add(statusLabel, 2, 0);
        filterPanel.Controls.Add(_statusFilter, 3, 0);
        filterPanel.Controls.Add(typeLabel, 4, 0);
        filterPanel.Controls.Add(_typeFilter, 5, 0);
        filterPanel.Controls.Add(clearButton, 6, 0);
        filterPanel.Controls.Add(refreshButton, 7, 0);

        var gridPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 2,
            ColumnCount = 1
        };
        gridPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        gridPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var actionPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        var addButton = CreateActionButton("Add Item");
        var editButton = CreateActionButton("Edit Selected");
        var deleteButton = CreateActionButton("Delete Selected");

        addButton.Click += (_, _) => AddItem();
        editButton.Click += (_, _) => EditSelectedItem();
        deleteButton.Click += (_, _) => DeleteSelectedItem();

        actionPanel.Controls.Add(addButton);
        actionPanel.Controls.Add(editButton);
        actionPanel.Controls.Add(deleteButton);

        _grid.Dock = DockStyle.Fill;
        gridPanel.Controls.Add(actionPanel, 0, 0);
        gridPanel.Controls.Add(_grid, 0, 1);

        mainPanel.Controls.Add(titlePanel, 0, 0);
        mainPanel.Controls.Add(summaryPanel, 0, 1);
        mainPanel.Controls.Add(filterPanel, 0, 2);
        mainPanel.Controls.Add(gridPanel, 0, 3);

        Controls.Add(mainPanel);
    }

    private static Button CreateActionButton(string text)
    {
        return new Button
        {
            Text = text,
            Width = 140,
            Height = 36,
            Margin = new Padding(0, 6, 12, 0)
        };
    }

    private void ConfigureGrid()
    {
        _grid.AutoGenerateColumns = false;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.RowHeadersVisible = false;
        _grid.DoubleClick += (_, _) => EditSelectedItem();

        AddColumn("AssetTag", "Asset Tag", 90);
        AddColumn("EquipmentType", "Type", 120);
        AddColumn("Manufacturer", "Manufacturer", 120);
        AddColumn("Model", "Model", 150);
        AddColumn("SerialNumber", "Serial Number", 140);
        AddColumn("AssignedTo", "Assigned To", 140);
        AddColumn("Status", "Status", 100);
        AddColumn("Location", "Location", 120);
        AddColumn("UpdatedAt", "Updated", 120, "g");
    }

    private void AddColumn(string propertyName, string headerText, int width, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            DataPropertyName = propertyName,
            HeaderText = headerText,
            MinimumWidth = width,
            FillWeight = width
        };

        if (!string.IsNullOrWhiteSpace(format))
        {
            column.DefaultCellStyle.Format = format;
        }

        _grid.Columns.Add(column);
    }

    private void RefreshGrid()
    {
        var filteredItems = ApplyFilters().ToList();
        _grid.DataSource = null;
        _grid.DataSource = filteredItems;
        UpdateSummary();
    }

    private IEnumerable<InventoryItem> ApplyFilters()
    {
        var query = _searchBox.Text.Trim().ToLowerInvariant();
        var selectedStatus = _statusFilter.SelectedItem?.ToString() ?? "All";
        var selectedType = _typeFilter.SelectedItem?.ToString() ?? "All";

        return _items.Where(item =>
        {
            var matchesSearch = string.IsNullOrWhiteSpace(query)
                || item.AssetTag.ToLowerInvariant().Contains(query)
                || item.EquipmentType.ToLowerInvariant().Contains(query)
                || item.Manufacturer.ToLowerInvariant().Contains(query)
                || item.Model.ToLowerInvariant().Contains(query)
                || item.SerialNumber.ToLowerInvariant().Contains(query)
                || item.AssignedTo.ToLowerInvariant().Contains(query)
                || item.Status.ToLowerInvariant().Contains(query)
                || item.Location.ToLowerInvariant().Contains(query)
                || item.Notes.ToLowerInvariant().Contains(query);

            var matchesStatus = selectedStatus == "All" || item.Status == selectedStatus;
            var matchesType = selectedType == "All" || item.EquipmentType == selectedType;

            return matchesSearch && matchesStatus && matchesType;
        });
    }

    private void UpdateSummary()
    {
        _totalLabel.Text = $"Total Items\n{_items.Count}";
        _inUseLabel.Text = $"In Use\n{_items.Count(i => i.Status == InventoryStatuses.InUse)}";
        _inStockLabel.Text = $"In Stock\n{_items.Count(i => i.Status == InventoryStatuses.InStock)}";
        _faultyLabel.Text = $"Faulty\n{_items.Count(i => i.Status == InventoryStatuses.Faulty)}";
    }

    private void ClearFilters()
    {
        _searchBox.Clear();
        _statusFilter.SelectedIndex = 0;
        _typeFilter.SelectedIndex = 0;
        RefreshGrid();
    }

    private void AddItem()
    {
        using var form = new ItemForm();
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var item = form.Item;
        item.CreatedAt = DateTime.Now;
        item.UpdatedAt = DateTime.Now;

        _items.Add(item);
        SaveAndRefresh();
    }

    private void EditSelectedItem()
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem is null)
        {
            MessageBox.Show("Please select an item to edit.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new ItemForm(selectedItem);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        var updatedItem = form.Item;
        selectedItem.AssetTag = updatedItem.AssetTag;
        selectedItem.EquipmentType = updatedItem.EquipmentType;
        selectedItem.Manufacturer = updatedItem.Manufacturer;
        selectedItem.Model = updatedItem.Model;
        selectedItem.SerialNumber = updatedItem.SerialNumber;
        selectedItem.AssignedTo = updatedItem.AssignedTo;
        selectedItem.Status = updatedItem.Status;
        selectedItem.Location = updatedItem.Location;
        selectedItem.Notes = updatedItem.Notes;
        selectedItem.UpdatedAt = DateTime.Now;

        SaveAndRefresh();
    }

    private void DeleteSelectedItem()
    {
        var selectedItem = GetSelectedItem();
        if (selectedItem is null)
        {
            MessageBox.Show("Please select an item to delete.", "No item selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Delete item {selectedItem.AssetTag}?",
            "Confirm delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
        {
            return;
        }

        _items.RemoveAll(i => i.Id == selectedItem.Id);
        SaveAndRefresh();
    }

    private InventoryItem? GetSelectedItem()
    {
        if (_grid.CurrentRow?.DataBoundItem is not InventoryItem selected)
        {
            return null;
        }

        return _items.FirstOrDefault(i => i.Id == selected.Id);
    }

    private void SaveAndRefresh()
    {
        _repository.Save(_items);
        RefreshGrid();
    }
}
