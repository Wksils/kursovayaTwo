using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models.ReportRow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace kursovayaTwo.View.Windows
{
    public partial class ReportWindow : Window
    {
        private string _reportType;
        private object _data;

        public ReportWindow(string title, string reportType, object data)
        {
            InitializeComponent();

            Title = title;
            _reportType = reportType;
            _data = data;

            SetupTable();
        }

        private void SetupTable()
        {
            switch (_reportType)
            {
                case "batch":
                    //dataGrid.ItemsSource = (List<BatchRecortRow>)_data;
                    break;

                case "deviation":
                    //dataGrid.ItemsSource = (List<DeviationReportRow>)_data;
                    break;

                case "recipe":
                    //dataGrid.ItemsSource = (List<RecipeReportRow>)_data;
                    break;

                case "lab":
                    //dataGrid.ItemsSource = (List<LabBlockReportRow>)_data;
                    break;

                case "extruder":
                    //dataGrid.ItemsSource = (List<ExtruderReportRow>)_data;
                    break;
            }
        }

        private void Export_Click(object? sender, RoutedEventArgs e)
        {
            var fileName = $"report_{DateTime.Today:yyyy-MM-dd}.csv";

            var sb = new StringBuilder();

            switch (_reportType)
            {
                case "batch":
                    sb.AppendLine("Номер партии;Продукт;Дата;Статус;Отклонения;Решение лаб.");
                    foreach (var r in (List<BatchRecortRow>)_data)
                        sb.AppendLine($"{r.BatchNumber};{r.ProductName};{r.CreatedAt};{r.Status};{r.HasDeviations};{r.QaDecision}");
                    break;

                case "deviation":
                    sb.AppendLine("Партия;Шаг;Фактические параметры;Статус");
                    foreach (var r in (List<DeviationReportRow>)_data)
                        sb.AppendLine($"{r.BatchNumber};{r.StepName};{r.ActualParams};{r.Status}");
                    break;

                case "recipe":
                    sb.AppendLine("Продукт;Версия рецептуры;Кол-во партий;Объём");
                    foreach (var r in (List<RecipeReportRow>)_data)
                        sb.AppendLine($"{r.ProductName};{r.RecipeVersion};{r.BatchCount};{r.TotalVolume}");
                    break;

                case "lab":
                    sb.AppendLine("Партия;Продукт;Дата блокировки;Причина;Ответственный");
                    foreach (var r in (List<LabBlockReportRow>)_data)
                        sb.AppendLine($"{r.BatchNumber};{r.ProductName};{r.DecisionAt};{r.ResultsText};{r.ResponsibleName}");
                    break;

                case "extruder":
                    sb.AppendLine("Партия;Зона;Параметр;Плановое;Фактическое;Отклонение;Время;Тип");
                    foreach (var r in (List<ExtruderReportRow>)_data)
                        sb.AppendLine($"{r.BatchNumber};{r.Zone};{r.ParameterName};{r.TargetValue};{r.ActualValue};{r.Deviation};{r.RecordedAt};{r.EventType}");
                    break;
            }

            File.WriteAllText(fileName, sb.ToString(), Encoding.UTF8);

            // Avalonia message box (без WPF MessageBox)
            var msg = new Window
            {
                Width = 300,
                Height = 120,
                Content = new TextBlock
                {
                    Text = "Файл сохранён!",
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
                }
            };

            msg.Show();
        }
    }
}