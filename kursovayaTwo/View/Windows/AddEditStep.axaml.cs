using Avalonia.Controls;
using Avalonia.Interactivity;
using kursovayaTwo.Models;
using kursovayaTwo.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovayaTwo.View.Windows;

public partial class AddEditStep : Window
{
    public TechStep TechStep { get; set; }

    public List<Equipment> Equipment { get; set; }

    private TechStep editCopy;

    private CardService service;
    private GetListsService listsService;

    public AddEditStep(TechStep techStep)
    {
        InitializeComponent();

        TechStep = techStep;

        editCopy = new TechStep
        {
            StepId = techStep.StepId,
            CardId = techStep.CardId,
            StepNumber = techStep.StepNumber,
            Name = techStep.Name,
            Description = techStep.Description,
            EquipmentId = techStep.EquipmentId,
            DurationMin = techStep.DurationMin,
            IsCritical = techStep.IsCritical,
            ParamsNote = techStep.ParamsNote
        };

        btn.Content = TechStep.Name != null ? "Сохранить" : "Создать";

        DataContext = editCopy;

        service = new CardService();
        listsService = new GetListsService();

        Equipment = Task.Run(() => listsService.GetEquipment()).Result;

        _ = Load();
    }

    private async Task Load()
    {
        List<TechStep> spisok = await listsService.GetStep();
    }

    private void btn_Click(object? sender, RoutedEventArgs e)
    {
        TechStep.StepId = editCopy.StepId;
        TechStep.CardId = editCopy.CardId;
        TechStep.StepNumber = editCopy.StepNumber;
        TechStep.Name = editCopy.Name;
        TechStep.Description = editCopy.Description;
        TechStep.EquipmentId = editCopy.EquipmentId;
        TechStep.DurationMin = editCopy.DurationMin;
        TechStep.IsCritical = editCopy.IsCritical;
        TechStep.ParamsNote = editCopy.ParamsNote;

        Close(true);
    }

    private void CheckBox_Checked(object? sender, RoutedEventArgs e)
    {
        editCopy.IsCritical = true;
    }

    private void CheckBox_Unchecked(object? sender, RoutedEventArgs e)
    {
        editCopy.IsCritical = false;
    }
}