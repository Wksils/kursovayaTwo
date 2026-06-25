using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models.ReportRow;
using kursovayaTwo.Services;
using kursovayaTwo.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace kursovayaTwo.ViewModel
{
    public partial class ReportViewModel : ObservableObject
    {
        public List<ReportItem> ReportItems { get; set; }
        private GetListsService service;
        [ObservableProperty]
        private DateTime dateFrom = DateTime.Today.AddMonths(-1);

        [ObservableProperty]
        private DateTime dateTo = DateTime.Today;
        public ReportViewModel()
        {
            service = new GetListsService();
            service = new GetListsService();
            ReportItems = new List<ReportItem>
            {
                new ReportItem { Name = "Отчёт по партиям за период", Command = OpenBatchReportCommand },
                new ReportItem { Name = "Отчёт по отклонениям", Command = OpenDeviationReportCommand },
                new ReportItem { Name = "Отчёт по использованию рецептур", Command = OpenRecipeReportCommand },
                new ReportItem { Name = "Отчёт по событиям экструдера", Command = OpenExtruderReportCommand },
                new ReportItem { Name = "Отчёт по лабораторным блокировкам", Command = OpenLabBlockReportCommand }
            };
        }
        [RelayCommand]
        private async Task OpenBatchReport()
        {
            var rows = await BuildBatchReport();
            var window = new ReportWindow("Отчёты по партиям", "batch", rows);
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await window.ShowDialog(desktop?.MainWindow);
        }
        [RelayCommand]
        private async Task OpenDeviationReport()
        {
            var rows = await BuildDeviationReport();
            var window = new ReportWindow("Отчёт по отклонениям", "deviation", rows);
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await window.ShowDialog(desktop?.MainWindow);
        }
        [RelayCommand]
        private async Task OpenRecipeReport()
        {
            var rows = await BuildRecipeReport();
            var window = new ReportWindow("Отчёт по рецептурам", "recipe", rows);
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await window.ShowDialog(desktop?.MainWindow);

        }
        [RelayCommand]
        private async Task OpenLabBlockReport()
        {
            var rows = await BuildLabBlockReport();
            var window = new ReportWindow("Лабораторные блокировки", "lab", rows);
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await window.ShowDialog(desktop?.MainWindow);
        }

        [RelayCommand]
        private async Task OpenExtruderReport()
        {
            var rows = await BuildExtruderReport();
            var window = new ReportWindow("Отчёт по экструдеру", "extruder", rows);
            var desktop = App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            await window.ShowDialog(desktop?.MainWindow);
        }
        private async Task<List<BatchRecortRow>> BuildBatchReport()
        {
            var batches = Task.Run(() => service.getBatches()).Result;
            var products = Task.Run(() => service.GetProducts()).Result;
            var executions = Task.Run(() => service.GetStepExecutions()).Result;

            return batches
                .Where(b => b.CreatedAt.Date >= DateFrom.Date && b.CreatedAt.Date <= DateTo.Date)
                .Select(b => new BatchRecortRow
                {
                    BatchNumber = b.BatchNumber,
                    ProductName = products.FirstOrDefault(p => p.ProductId == b.ProductId)?.Name ?? "—",
                    CreatedAt = b.CreatedAt.ToShortDateString(),
                    Status = b.Status,
                    HasDeviations = executions.Any(e => e.BatchId == b.BatchId && e.Status == "failed") ? "Да" : "Нет",
                    QaDecision = b.QaDecision ?? "pending"
                }).ToList();
        }
        private async Task<List<DeviationReportRow>> BuildDeviationReport()
        {
            var batches = Task.Run(() => service.getBatches()).Result;
            var executions = Task.Run(() => service.GetStepExecutions()).Result;
            var batchIds = batches
                .Where(b => b.CreatedAt.Date >= DateFrom.Date && b.CreatedAt.Date <= DateTo.Date)
                .Select(b => b.BatchId).ToHashSet();

            return executions
                .Where(e => e.Status == "failed" && batchIds.Contains(e.BatchId))
                .Select(e => new DeviationReportRow
                {
                    BatchNumber = batches.FirstOrDefault(b => b.BatchId == e.BatchId)?.BatchNumber ?? "—",
                    StepName = e.StepId.ToString(),
                    ActualParams = e.ActualParams ?? "—",
                    Status = e.Status
                }).ToList();
        }
        private async Task<List<RecipeReportRow>> BuildRecipeReport()
        {
            var batches = Task.Run(() => service.getBatches()).Result;
            var products = Task.Run(() => service.GetProducts()).Result;
            var recipes = Task.Run(() => service.getRecipes()).Result;

            return batches
                .Where(b => b.CreatedAt.Date >= DateFrom.Date && b.CreatedAt.Date <= DateTo.Date)
                .GroupBy(b => b.RecipeId)
                .Select(g =>
                {
                    var recipe = recipes.FirstOrDefault(r => r.RecipeId == g.Key);
                    var product = products.FirstOrDefault(p => p.ProductId == (recipe?.ProductId ?? 0));
                    return new RecipeReportRow
                    {
                        ProductName = product?.Name ?? "—",
                        RecipeVersion = recipe?.Version ?? "—",
                        BatchCount = g.Count(),
                        TotalVolume = g.Sum(b => b.ActualQty ?? 0)
                    };
                }).ToList();
        }
        private async Task<List<LabBlockReportRow>> BuildLabBlockReport()
        {
            var batches = Task.Run(() => service.getBatches()).Result;
            var products = Task.Run(() => service.GetProducts()).Result;
            var users = Task.Run(() => service.GetAllUsers()).Result;
            var labTests = Task.Run(() => service.GetLabTests()).Result;
            var batchIds = batches
                .Where(b => b.CreatedAt.Date >= DateFrom.Date && b.CreatedAt.Date <= DateTo.Date)
                .Select(b => b.BatchId).ToHashSet();

            return labTests
                .Where(t => t.OverallResult == "fail" && batchIds.Contains((int)t.BatchId))
                .Select(t =>
                {
                    var batch = batches.FirstOrDefault(b => b.BatchId == t.BatchId);
                    var product = products.FirstOrDefault(p => p.ProductId == (batch?.ProductId ?? 0));
                    var user = users.FirstOrDefault(u => u.UserId == t.DecisionBy);
                    return new LabBlockReportRow
                    {
                        BatchNumber = batch?.BatchNumber ?? "—",
                        ProductName = product?.Name ?? "—",
                        DecisionAt = t.DecisionAt?.ToShortDateString() ?? "—",
                        ResultsText = t.ResultsText ?? "—",
                        ResponsibleName = user?.FullName ?? "—"
                    };
                }).ToList();
        }
        private async Task<List<ExtruderReportRow>> BuildExtruderReport()
        {
            var batches = Task.Run(() => service.getBatches()).Result;
            var executions = Task.Run(() => service.GetStepExecutions()).Result;
            var events = Task.Run(() => service.GetExtruderEvents()).Result;
            var batchIds = batches
                .Where(b => b.CreatedAt.Date >= DateFrom.Date && b.CreatedAt.Date <= DateTo.Date)
                .Select(b => b.BatchId).ToHashSet();
            var execIds = executions
                .Where(e => batchIds.Contains(e.BatchId))
                .Select(e => e.ExecutionId).ToHashSet();
            return events
            .Where(e => execIds.Contains(e.ExecutionId))
            .Select(e =>
            {
                var exec = executions.FirstOrDefault(x => x.ExecutionId == e.ExecutionId);
                var batch = batches.FirstOrDefault(b => b.BatchId == exec?.BatchId);
                var deviation = e.TargetValue.HasValue ? (e.ActualValue - e.TargetValue.Value).ToString() : "—";
                return new ExtruderReportRow
                {
                    BatchNumber = batch?.BatchNumber ?? "—",
                    Zone = e.Zone,
                    ParameterName = e.ParameterName ?? "—",
                    TargetValue = e.TargetValue,
                    ActualValue = e.ActualValue ?? 0,
                    Deviation = deviation,
                    RecordedAt = e.RecordedAt.ToString("dd.MM.yyyy HH:mm"),
                    EventType = e.EventType
                };
            }).ToList();
        }

    }
}
