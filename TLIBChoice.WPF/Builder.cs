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
        public static void Log_DisplayQuestionRequested(string title, string text, ResultCallback choice, object[] choices, Options options)
        {
            var ctrl = new TLIBChoice_Wpf();
            var win = new Window
            {
                Title = title,
                Content = ctrl,
                SizeToContent = SizeToContent.Height
            };
            ctrl.Text.Text = text;
            win.Width = ctrl.Text.Text.Length * 3.4;
            int optionscounter = 0;
            int buttonColumns = options.ButtonColumns;
            if (buttonColumns < 1)
            {
                buttonColumns = 3;
            }
            for (int i = 0; i < buttonColumns; i++)
            {
                if (options.ButtonOrientation == Orientation.Horizontal)
                {
                    ctrl.OptionsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
                else
                {
                    ctrl.OptionsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
            }
            for (int i = 0; i < Math.Ceiling(choices.Length / (double)buttonColumns); i++)
            {
                if (options.ButtonOrientation == Orientation.Horizontal)
                {
                    ctrl.OptionsGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
                else
                {
                    ctrl.OptionsGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
            }
            
            ctrl.OptionsStack.Orientation = options.ButtonOrientation == Orientation.Horizontal ? System.Windows.Controls.Orientation.Horizontal : System.Windows.Controls.Orientation.Vertical;
            foreach (var item in choices)
            {
                var b = new Button
                {
                    Tag = optionscounter,
                    Content = item,
                    Style = ctrl.Resources["ButtonStyle"] as Style,
                };
                b.Click += (s, e) => { choice.SendResultNo((int)(s as Button).Tag); win.Close(); };
                if (options.ButtonPresentation == Presentation.Stackpanel)
                {
                    ctrl.OptionsStack.Children.Add(b);
                }
                else
                {
                    if (options.ButtonOrientation == Orientation.Horizontal)
                    {
                        Grid.SetColumn(b, optionscounter % buttonColumns);
                        Grid.SetRow(b, optionscounter / buttonColumns);
                    }
                    else
                    {
                        Grid.SetColumn(b, optionscounter / buttonColumns);
                        Grid.SetRow(b, optionscounter % buttonColumns);
                    }
                    ctrl.OptionsGrid.Children.Add(b);
                }
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
