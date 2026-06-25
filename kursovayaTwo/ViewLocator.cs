using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using kursovayaTwo.ViewModel;

namespace kursovayaTwo
{
    /// <summary>
    /// Given a Views model, returns the corresponding Views if possible.
    /// </summary>
    [RequiresUnreferencedCode(
        "Default implementation of ViewsLocator involves reflection which may be trimmed away.",
        Url = "https://docs.avaloniaui.net/docs/concepts/Views-locator")]
    public class ViewsLocator : IDataTemplate
    {
        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            var name = param.GetType().FullName!.Replace("ViewModel", "Views", StringComparison.Ordinal);
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object? data)
        {
            return data is MainWindowViewModel;
        }
    }
}
