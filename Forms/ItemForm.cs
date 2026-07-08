using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ITInventoryManager.Models;

namespace ITInventoryManager.Forms;

public class ItemForm : Form
{
    private readonly TextBox _assetTagBox = new();
    private readonly ComboBox _equipmentTypeBox = new();
    private readonly TextBox _manufacturerBox = new();
    private readonly TextBox _modelBox = new();
    private readonly TextBox _serialNumberBox = new();
    private readonly TextBox _assignedToBox = new();
    private readonly ComboBox _statusBox = new();
    private readonly TextBox _locationBox = new();
    private readonly TextBox _notesBox = new();

    public InventoryItem Item { get; private set; }

    public ItemForm()
    {
        Item = new InventoryItem();
        InitializeForm("Add Inventory Item");
    }

    public ItemForm(InventoryItem existingItem)
    {
        Item = Clone(existingItem);
        InitializeForm("Edit Inventory Item");
        LoadItemToForm();
    }

    private void InitializeForm(string title)
    {
        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ClientSize = new Size(560, 560);
        Font = new Font("Segoe UI", 10);

        var mainPanel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 2,
            RowCount = 11
        };

        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        for (var i = 0; i < 10; i++)
        {
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, i == 8 ? 90 : 42));
        }

        mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));

        ConfigureInputs();

        AddField(mainPanel, 0, "Asset Tag *", _assetTagBox);
        AddField(mainPanel, 1, "Equipment Type *", _equipmentTypeBox);
        AddField(mainPanel, 2, "Manufacturer", _manufacturerBox);
        AddField(mainPanel, 3, "Model", _modelBox);
        AddField(mainPanel, 4, "Serial Number", _serialNumberBox);
        AddField(mainPanel, 5, "Assigned To", _assignedToBox);
        AddField(mainPanel, 6, "Status *", _statusBox);
        AddField(mainPanel, 7, "Location", _locationBox);
        AddField(mainPanel, 8, "Notes", _notesBox);

        var requiredNote = new Label
        {
            Text = "* Required fields",
            Dock = DockStyle.Fill,
            ForeColor = Color.DimGray,
            TextAlign = ContentAlignment.MiddleLeft
        };
        mainPanel.Controls.Add(requiredNote, 1, 9);

        var buttonPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft
        };

        var saveButton = new Button
        {
            Text = "Save",
            Width = 100,
            Height = 34,
            DialogResult = DialogResult.None
        };
        saveButton.Click += (_, _) => SaveItem();

        var cancelButton = new Button
        {
            Text = "Cancel",
            Width = 100,
            Height = 34,
            DialogResult = DialogResult.Cancel
        };

        buttonPanel.Controls.Add(saveButton);
        buttonPanel.Controls.Add(cancelButton);
        mainPanel.Controls.Add(buttonPanel, 1, 10);

        AcceptButton = saveButton;
        CancelButton = cancelButton;
        Controls.Add(mainPanel);
    }

    private void ConfigureInputs()
    {
        foreach (var textBox in new[]
        {
            _assetTagBox,
            _manufacturerBox,
            _modelBox,
            _serialNumberBox,
            _assignedToBox,
            _locationBox
        })
        {
            textBox.Dock = DockStyle.Fill;
        }

        _equipmentTypeBox.Dock = DockStyle.Fill;
        _equipmentTypeBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _equipmentTypeBox.Items.AddRange(EquipmentTypes.All);
        _equipmentTypeBox.SelectedIndex = 0;

        _statusBox.Dock = DockStyle.Fill;
        _statusBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusBox.Items.AddRange(InventoryStatuses.All);
        _statusBox.SelectedItem = InventoryStatuses.InStock;

        _notesBox.Dock = DockStyle.Fill;
        _notesBox.Multiline = true;
        _notesBox.ScrollBars = ScrollBars.Vertical;
    }

    private static void AddField(TableLayoutPanel panel, int row, string labelText, Control input)
    {
        var label = new Label
        {
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        panel.Controls.Add(label, 0, row);
        panel.Controls.Add(input, 1, row);
    }

    private void LoadItemToForm()
    {
        _assetTagBox.Text = Item.AssetTag;
        _equipmentTypeBox.SelectedItem = EquipmentTypes.All.Contains(Item.EquipmentType) ? Item.EquipmentType : "Other";
        _manufacturerBox.Text = Item.Manufacturer;
        _modelBox.Text = Item.Model;
        _serialNumberBox.Text = Item.SerialNumber;
        _assignedToBox.Text = Item.AssignedTo;
        _statusBox.SelectedItem = InventoryStatuses.All.Contains(Item.Status) ? Item.Status : InventoryStatuses.InStock;
        _locationBox.Text = Item.Location;
        _notesBox.Text = Item.Notes;
    }

    private void SaveItem()
    {
        if (!ValidateForm())
        {
            return;
        }

        Item.AssetTag = _assetTagBox.Text.Trim();
        Item.EquipmentType = _equipmentTypeBox.SelectedItem?.ToString() ?? "Other";
        Item.Manufacturer = _manufacturerBox.Text.Trim();
        Item.Model = _modelBox.Text.Trim();
        Item.SerialNumber = _serialNumberBox.Text.Trim();
        Item.AssignedTo = _assignedToBox.Text.Trim();
        Item.Status = _statusBox.SelectedItem?.ToString() ?? InventoryStatuses.InStock;
        Item.Location = _locationBox.Text.Trim();
        Item.Notes = _notesBox.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }

    private bool ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(_assetTagBox.Text))
        {
            ShowValidationMessage("Asset Tag is required.");
            _assetTagBox.Focus();
            return false;
        }

        if (_equipmentTypeBox.SelectedItem is null)
        {
            ShowValidationMessage("Equipment Type is required.");
            _equipmentTypeBox.Focus();
            return false;
        }

        if (_statusBox.SelectedItem is null)
        {
            ShowValidationMessage("Status is required.");
            _statusBox.Focus();
            return false;
        }

        if (_statusBox.SelectedItem.ToString() == InventoryStatuses.InUse && string.IsNullOrWhiteSpace(_assignedToBox.Text))
        {
            ShowValidationMessage("Assigned To is required when status is In Use.");
            _assignedToBox.Focus();
            return false;
        }

        return true;
    }

    private static void ShowValidationMessage(string message)
    {
        MessageBox.Show(message, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static InventoryItem Clone(InventoryItem item)
    {
        return new InventoryItem
        {
            Id = item.Id,
            AssetTag = item.AssetTag,
            EquipmentType = item.EquipmentType,
            Manufacturer = item.Manufacturer,
            Model = item.Model,
            SerialNumber = item.SerialNumber,
            AssignedTo = item.AssignedTo,
            Status = item.Status,
            Location = item.Location,
            Notes = item.Notes,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };
    }
}
