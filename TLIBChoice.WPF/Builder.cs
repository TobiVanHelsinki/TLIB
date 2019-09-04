using System;
using System.Windows;
using System.Windows.Controls;

namespace TLIB.Choice.WPF
{
    /// <summary>
    /// This calss contains a Method to display a wpf choice window. you can use it as event handler
    /// </summary>
    public static class Builder
    {
        /// <summary>
        /// Property to determine how many columns the ui have for the choices
        /// </summary>
        public static int Columns { get; set; } = 3;
        public static void Log_DisplayQuestionRequested(string title, string text, ResultCallback choice, params string[] choices)
        {
            var ctrl = new TLIBChoice_Wpf();
            var win = new Window
            {
                Title = title,
                Content = ctrl,
                Height = 60 * choices.Length / Columns + 50,
                Width = 180 * Columns,
            };
            ctrl.Text.Text = text;
            int optionscounter = 0;
            for (int i = 0; i < Columns; i++)
            {
                ctrl.OptionsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < Math.Ceiling(choices.Length / (double)Columns); i++)
            {
                ctrl.OptionsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            }
            foreach (var item in choices)
            {
                var b = new Button
                {
                    Tag = optionscounter,
                    Content = item,
                    Style = ctrl.Resources["ButtonStyle"] as Style,
                };
                b.Click += (s, e) => { choice.SendResultNo((int)(s as Button).Tag); win.Close(); };
                Grid.SetColumn(b, optionscounter % Columns);
                Grid.SetRow(b, optionscounter / Columns);
                ctrl.OptionsGrid.Children.Add(b);
                optionscounter++;
            }
            try
            {
                win.ShowDialog();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
