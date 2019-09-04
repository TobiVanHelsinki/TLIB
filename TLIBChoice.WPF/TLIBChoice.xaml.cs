using System.Windows;
using System.Windows.Controls;
using TLIB;

namespace TLIBChoice.WPF
{
    public partial class TLIBChoice : UserControl
    {
        public static int Columns { get; set; } = 3;
        public static void Log_DisplayQuestionRequested(string title, string text, ResultCallback choice, params string[] choices)
        {
            var ctrl = new TLIBChoice();
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
            for (int i = 0; i < choices.Length / Columns; i++)
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
            catch (System.InvalidOperationException)
            {
            }
        }

        private static void B_Click(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public TLIBChoice()
        {
            InitializeComponent();
        }
    }
}
