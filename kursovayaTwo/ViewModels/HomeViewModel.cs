using CommunityToolkit.Mvvm.ComponentModel;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.ViewModel
{
    public partial class HomeViewModel: ObservableObject    
    {
        private GetListsService listsService;
        [ObservableProperty]
        private int tests;
        [ObservableProperty]
        private int productionInProgress;
        [ObservableProperty]
        private int productionActive;
        [ObservableProperty]
        private int badProduction;
        [ObservableProperty]
        private int activeRecipe;
        [ObservableProperty]
        private int activeCards;
        [ObservableProperty]
        private string lastStartProduction;
        [ObservableProperty]
        private string lastEndProduction;
        [ObservableProperty]
        private string lastBadProduction;
        [ObservableProperty]
        private string lastBlockProduction;

        
        public HomeViewModel()
        {
            listsService = new GetListsService();
            tests = new ObservableCollection<LabTests>(getTests()).Where(n => n.Status == "completed" && n.OverallResult == null).Count();
            productionInProgress = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.Status == "in_progress").Count();
            productionActive = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.Status == "in_progress" || n.Status == "planned").Count();
            badProduction = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.QaDecision == "rejected").Count();
            activeRecipe = new ObservableCollection<Recipe>(getRecipes()).Where(n => n.IsActive == true && n.Status == "approved").Count();
            activeCards = new ObservableCollection<TechCard>(getCards()).Where(n => n.IsActive == true && n.Status == "approved").Count();
            lastStartProduction =  new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.Status == "in_progress").Select(n => n.CreatedAt).DefaultIfEmpty().Min().ToShortDateString();
            lastEndProduction = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.Status == "completed").Select(n => n.CreatedAt).DefaultIfEmpty().Min().ToShortDateString();
            lastBadProduction = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.QaDecision == "rejected").Select(n => n.CreatedAt).DefaultIfEmpty().Min().ToShortDateString();
            lastBlockProduction = new ObservableCollection<ProductionBatch>(getProductions()).Where(n => n.Status == "rejected").Select(n => n.CreatedAt).DefaultIfEmpty().Min().ToShortDateString();
        }
        private List<LabTests> getTests()
        {
            Task<List<LabTests>> task = Task.Run(()=> listsService.getTests());
            return task.Result;
        }
        private List<ProductionBatch> getProductions()
        {
            Task<List<ProductionBatch>> task = Task.Run(() => listsService.getBatches());
            return task.Result;

        }
        private List<Recipe> getRecipes()
        {
            Task<List<Recipe>> task = Task.Run(() => listsService.getRecipes());
            return task.Result;
        }
        private List<TechCard> getCards()
        {
            Task<List<TechCard>> task = Task.Run(() => listsService.getCards());
            return task.Result;
        }

    }
}
