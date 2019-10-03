using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TLIB;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TLIB.Choice.Xamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TLIBChoice_Xamarin : Rg.Plugins.Popup.Pages.PopupPage
    {
        public TLIBChoice_Xamarin()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Property to determine how many columns the ui have for the choices
        /// </summary>
        public static void Log_DisplayQuestionRequested(string title, string text, ResultCallback choice, object[] choices, Options options)
        {
            var ctrl = new TLIBChoice_Xamarin();

            ctrl.MainText.Text = text;
            ctrl.TitleText.Text = title;
            ctrl.MinimumWidthRequest = text.Length * 3.4;
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

            ctrl.OptionsStack.Orientation = options.ButtonOrientation == Orientation.Horizontal ? StackOrientation.Horizontal : StackOrientation.Vertical;
            foreach (var item in choices)
            {
                var b = new Button
                {
                    Text = item.ToString(),
                };
                b.Resources.Add(nameof(optionscounter), optionscounter);
                b.Clicked += (s, e) => { choice.SendResultNo((int)(s as Button).Resources[nameof(optionscounter)]); PopupNavigation.Instance.PopAsync(true); };
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
                PopupNavigation.Instance.PushAsync(ctrl, true);
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}