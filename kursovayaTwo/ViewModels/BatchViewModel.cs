using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using kursovayaTwo.View.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.ViewModel
{
    public partial class BatchViewModel : ObservableObject
    {
        private GetListsService listsService;
        private BatchService service;
        private List<MaterialBatch> batches;
        [ObservableProperty]
        private ObservableCollection<string> codeList;
        [ObservableProperty]
        private ObservableCollection<RawMaterial> materialList;
        [ObservableProperty]
        private string selectedCode;
        [ObservableProperty]
        private bool isFilterOpen;
        [ObservableProperty]
        private ObservableCollection<MaterialBatch> batchRows;
        [ObservableProperty]
        private int? selectedMaterialId;
        public BatchViewModel()
        {
            listsService = new GetListsService();
            service = new BatchService();
            Load();
        }
        public List<FilterOption> StatusFilters { get; set; } = new()
        {
            new FilterOption { Value = "quarantine" },
            new FilterOption { Value = "approved" },
            new FilterOption { Value = "rejected" },
            new FilterOption { Value = "used" }
        };
        public List<FilterOption> CategoryFilters { get; set; } = new();
        private void Load()
        {
            batches = Task.Run(() => listsService.GetMaterialBatches()).Result;
            var materials = Task.Run(() => listsService.GetRawMaterials()).Result;
            CodeList = new ObservableCollection<string>(batches.Select(r => r.BatchNumber).Distinct());
            MaterialList = new ObservableCollection<RawMaterial>(materials);
            BatchRows = new ObservableCollection<MaterialBatch>(batches);
            CategoryFilters = materials.Select(m => m.Category).Distinct()
        .Select(c => new FilterOption { Value = c }).ToList();
            OnPropertyChanged(nameof(CategoryFilters));
        }
        partial void OnSelectedCodeChanged(string value) => ApplyFilter();
        partial void OnSelectedMaterialIdChanged(int? value)
        {
            ApplyFilter();
        }
        private void ApplyFilter()
        {
            var materials = Task.Run(() => listsService.GetRawMaterials()).Result;
            var filtered = batches.AsEnumerable();
            if (!string.IsNullOrEmpty(SelectedCode))
                filtered = filtered.Where(r => r.BatchNumber == SelectedCode);
            if (SelectedMaterialId.HasValue)
                filtered = filtered.Where(x => x.MaterialId == SelectedMaterialId.Value);
            var checkedStatuses = StatusFilters.Where(f => f.IsChacked).Select(f => f.Value).ToList();
            if (checkedStatuses.Any())
                filtered = filtered.Where(b => checkedStatuses.Contains(b.Status));

            var checkedCategories = CategoryFilters.Where(f => f.IsChacked).Select(f => f.Value).ToList();
            if (checkedCategories.Any())
                filtered = filtered.Where(b =>
                {
                    var mat = materials.FirstOrDefault(m => m.MaterialId == b.MaterialId);
                    return mat != null && checkedCategories.Contains(mat.Category);
                });
            BatchRows = new ObservableCollection<MaterialBatch>(filtered.ToList());
        }
        [RelayCommand]
        private void ToggleFilter()
        {
            isFilterOpen = !isFilterOpen;
            OnPropertyChanged(nameof(IsFilterOpen));
        }
        [RelayCommand]
        private void ApplyFilterOptions()
        {
            isFilterOpen = false;
            OnPropertyChanged(nameof(IsFilterOpen));
            ApplyFilter();
        }
        [RelayCommand]
        private async Task Add()
        {
            try
            {
                AddEditBatch window = new AddEditBatch(new MaterialBatch());
                //if (window.ShowDialog() == true)
                //{
                //    MaterialBatch batch = window.Batch;
                //    await service.AddBatch(batch);
                //}
            }
            finally
            {
                Load();
            }
        }
        [RelayCommand]
        private async Task Edit(object param)
        {
            MaterialBatch batch = (MaterialBatch)param;
            AddEditBatch window = new AddEditBatch(batch);
            //if (window.ShowDialog() == true)
            //{
            //    await service.EditBatch(batch);
            //    Load();
            //}
        }
        [RelayCommand]
        private void OpenTest(object param)
        {
            if (param is not MaterialBatch batch) return;

            var allTests = Task.Run(() => listsService.GetLabTests()).Result;
            var batchTests = allTests.Where(t => t.MatBatchId == batch.BatchId).ToList();

            var unfinished = batchTests.FirstOrDefault(t => t.Status != "completed" && t.Status != "cancelled");

            //if (unfinished != null)
            //{
            //    var result = MessageBox.Show(
            //        "У этой партии уже есть незавершённое испытание. Открыть его?",
            //        "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            //    if (result == MessageBoxResult.Yes)
            //    {
            //        var window = new AddEditTest(unfinished);
            //        window.ShowDialog();
            //    }
            //    return;
            //}

            var newTest = new LabTests { MatBatchId = batch.BatchId };
            var addWindow = new AddEditTest(newTest);
            if (addWindow.ShowDialog() == true)
            {
                Load(); 
            }
        }
        [RelayCommand]
        private void OpenCard(object param)
        {
            if (param is not MaterialBatch batch) return;

            var window = new MaterialBatchInfo(batch);
            window.ShowDialog();
            Load(); 
        }
    }
}
