using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class MaterialBatchInfo : Window
{
    public MaterialBatch Batch { get; set; }

    public RawMaterial Material { get; set; }

    public List<LabTests> Tests { get; set; }

    public List<User> Users { get; set; }

    private GetListsService service;
    private BatchService batchService;

    public MaterialBatchInfo(MaterialBatch batch)
    {
        InitializeComponent();

        Batch = batch;

        service = new GetListsService();
        batchService = new BatchService();

        var materials = Task.Run(() => service.GetRawMaterials()).Result;
        Material = materials.FirstOrDefault(m => m.MaterialId == batch.MaterialId)!;

        var allTests = Task.Run(() => service.GetLabTests()).Result;
        Tests = allTests.Where(t => t.MatBatchId == batch.BatchId).ToList();

        Users = Task.Run(() => service.GetAllUsers()).Result;

        DataContext = this;
    }

    private async void ConfirmDecision_ClickAsync(object? sender, RoutedEventArgs e)
    {
        //if (Tests == null || !Tests.Any())
        //{
        //    MessageBox.Show("Невозможно принять решение: у партии нет испытаний");
        //    return;
        //}

        //var unfinished = Tests.FirstOrDefault(t =>
        //    t.Status != "completed" && t.Status != "cancelled");

        //if (unfinished != null)
        //{
        //    MessageBox.Show("Невозможно принять решение: испытание не завершено");
        //    return;
        //}

        //var lastTest = Tests.OrderByDescending(t => t.CreatedAt).FirstOrDefault();

        //if (lastTest == null || lastTest.Status != "completed")
        //{
        //    MessageBox.Show("Невозможно принять решение: испытание не завершено");
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(lastTest.ResultsText) ||
        //    string.IsNullOrWhiteSpace(lastTest.OverallResult))
        //{
        //    MessageBox.Show("Не заполнены обязательные результаты анализа");
        //    return;
        //}

        var selectedItem = decisionTypeBox.SelectedItem as ComboBoxItem;

        //if (selectedItem == null)
        //{
        //    MessageBox.Show("Выберите тип решения");
        //    return;
        //}

        string decision = selectedItem.Tag?.ToString();

        string comment = commentBox.Text;
        string reason = reasonBox.Text;

        if (decision == "rejected" && string.IsNullOrWhiteSpace(comment))
        {
            //MessageBox.Show("При блокировке партии комментарий обязателен");
            return;
        }

        Batch.QaDecision = decision;
        Batch.DecisionAt = DateTime.Now;
        Batch.DecisionBy = Users.FirstOrDefault(p =>
            p.Login == RegisterUser.usernsme)!.UserId;

        Batch.DecisionComment = comment;
        Batch.DecisionReason = decision == "rejected" ? reason : null;
        Batch.Status = decision;

        await batchService.EditBatch(Batch);

        //MessageBox.Show("Решение сохранено");

        Close();
    }
}