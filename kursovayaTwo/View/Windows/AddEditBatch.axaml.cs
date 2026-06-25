using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace kursovayaTwo.View.Windows;

public partial class AddEditBatch : Window
{
    public MaterialBatch Batch { get; set; }
    public MaterialBatch EditCopy { get; set; }

    public List<User> Users { get; set; }
    public List<LabTests> Tests { get; set; }
    public List<RawMaterial> Materials { get; set; }
    public List<UnitsOfMeasure> Uom { get; set; }

    private readonly GetListsService service;

    public AddEditBatch(MaterialBatch batch)
    {
        InitializeComponent();

        Batch = batch;

        EditCopy = new MaterialBatch
        {
            BatchId = batch.BatchId,
            MaterialId = batch.MaterialId,
            BatchNumber = batch.BatchNumber,
            Supplier = batch.Supplier,
            Quantity = batch.Quantity,
            UomId = batch.UomId,
            ManufactureDate = batch.ManufactureDate,
            ExpiryDate = batch.ExpiryDate,
            ReceivedAt = batch.ReceivedAt,
            Status = batch.Status,
            StorageLocation = batch.StorageLocation,
            QaDecision = batch.QaDecision,
            DecisionAt = batch.DecisionAt,
            DecisionBy = batch.DecisionBy,
            DecisionComment = batch.DecisionComment,
            DecisionReason = batch.DecisionReason
        };

        service = new GetListsService();

        Users = Task.Run(() => service.GetAllUsers()).Result;
        Materials = Task.Run(() => service.GetRawMaterials()).Result;
        Uom = Task.Run(() => service.GetUom()).Result;

        Tests = Task.Run(() => service.GetLabTests()).Result
            .Where(t => t.MatBatchId == Batch.BatchId)
            .ToList();

        DataContext = this;

        btn.Content = Batch.BatchId != null
            ? "Сохранить"
            : "Создать";

        _ = Load();
    }

    private async Task Load()
    {
        await service.GetMaterialBatches();
    }

    private void Button_Click(object? sender, RoutedEventArgs e)
    {
        Batch.BatchId = EditCopy.BatchId;
        Batch.MaterialId = EditCopy.MaterialId;
        Batch.BatchNumber = EditCopy.BatchNumber;
        Batch.Supplier = EditCopy.Supplier;
        Batch.Quantity = EditCopy.Quantity;
        Batch.UomId = EditCopy.UomId;
        Batch.ManufactureDate = EditCopy.ManufactureDate;
        Batch.ExpiryDate = EditCopy.ExpiryDate;
        Batch.ReceivedAt = EditCopy.ReceivedAt;
        Batch.Status = EditCopy.Status;
        Batch.StorageLocation = EditCopy.StorageLocation;
        Batch.QaDecision = EditCopy.QaDecision;
        Batch.DecisionAt = EditCopy.DecisionAt;
        Batch.DecisionBy = EditCopy.DecisionBy;
        Batch.DecisionComment = EditCopy.DecisionComment;
        Batch.DecisionReason = EditCopy.DecisionReason;

        Close(true);
    }

    public DateTime? ManufactureDateProxy
    {
        get => EditCopy.ManufactureDate?.ToDateTime(TimeOnly.MinValue);

        set => EditCopy.ManufactureDate = value.HasValue
            ? DateOnly.FromDateTime(value.Value)
            : null;
    }

    public DateTime? ExpiryDateProxy
    {
        get => EditCopy.ExpiryDate?.ToDateTime(TimeOnly.MinValue);

        set => EditCopy.ExpiryDate = value.HasValue
            ? DateOnly.FromDateTime(value.Value)
            : null;
    }

    private async void EditTest_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        if (button.Tag is not LabTests test)
            return;

        var window = new AddEditTest(test);

        var result = await window.ShowDialog<bool>(this);

        if (result)
        if (result)
        {
            RefreshTests();
        }
    }

    private void RefreshTests()
    {
        var allTests = Task.Run(() => service.GetLabTests()).Result;

        Tests = allTests
            .Where(t => t.MatBatchId == Batch.BatchId)
            .ToList();

        testsListView.ItemsSource = Tests;
    }
}