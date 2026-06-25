using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class AddEditTest : Window
{
    public LabTests Test { get; set; }
    public LabTests EditCopy { get; set; }

    public List<User> Users { get; set; }

    private GetListsService service;
    private LabTestService testService;

    public AddEditTest(LabTests test)
    {
        InitializeComponent();

        Test = test;

        EditCopy = new LabTests
        {
            TestId = test.TestId,
            BatchId = test.BatchId,
            MatBatchId = test.MatBatchId,
            TestType = test.TestType,
            Status = test.Status,
            AssignedTo = test.AssignedTo,
            StartedAt = test.StartedAt,
            CompletedAt = test.CompletedAt,
            ResultsText = test.ResultsText,
            OverallResult = test.OverallResult,
            DecisionBy = test.DecisionBy,
            DecisionAt = test.DecisionAt,
            CreatedAt = test.TestId != 0 ? test.CreatedAt : DateTime.Now,
            Priority = test.Priority,
            Comment = test.Comment,
            ControlledParameters = test.ControlledParameters
        };

        btn.Content = Test.TestId != 0 ? "Сохранить" : "Создать";

        service = new GetListsService();
        testService = new LabTestService();

        Users = Task.Run(() => service.GetAllUsers()).Result;

        if (string.IsNullOrEmpty(EditCopy.Status))
            EditCopy.Status = "pending";

        DataContext = this;
    }

    private async void Button_ClickAsync(object? sender, RoutedEventArgs e)
    {
        //if (string.IsNullOrWhiteSpace(EditCopy.TestType))
        //{
        //    MessageBox.Show("Укажите тип испытания");
        //    return;
        //}

        //if (!EditCopy.AssignedTo.HasValue)
        //{
        //    MessageBox.Show("Укажите исполнителя");
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(EditCopy.ControlledParameters))
        //{
        //    MessageBox.Show("Укажите контролируемые параметры");
        //    return;
        //}

        Test.TestId = EditCopy.TestId;
        Test.BatchId = EditCopy.BatchId;
        Test.MatBatchId = EditCopy.MatBatchId;
        Test.TestType = EditCopy.TestType;
        Test.Status = EditCopy.Status;
        Test.AssignedTo = EditCopy.AssignedTo;
        Test.StartedAt = EditCopy.StartedAt;
        Test.CompletedAt = EditCopy.CompletedAt;
        Test.ResultsText = EditCopy.ResultsText;
        Test.OverallResult = EditCopy.OverallResult;
        Test.DecisionBy = EditCopy.DecisionBy;
        Test.DecisionAt = EditCopy.DecisionAt;
        Test.CreatedAt = EditCopy.CreatedAt;
        Test.Priority = EditCopy.Priority;
        Test.Comment = EditCopy.Comment;
        Test.ControlledParameters = EditCopy.ControlledParameters;

        if (Test.TestId == 0)
            await testService.AddTest(Test);
        else
            await testService.EditTest(Test);

        Close(true);
    }
}