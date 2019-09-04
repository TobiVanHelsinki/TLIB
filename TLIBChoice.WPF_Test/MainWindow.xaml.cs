using System.Windows;
using TLIB;

namespace TLIBChoice.WPF_Test
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Log.DisplayChoiceRequested += TLIB.Choice.WPF.Builder.Log_DisplayQuestionRequested;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.DisplayChoice("ATTANTION! Importanté!", "This is a very important choice you have to made, young padawan. Choose wise and an not unimportant amount of money will be granted to you.",
                ("Opt1",()=>Result.Text = "Opt1"),
                ("Opt2",()=>Result.Text = "Opt2"),
                ("Opt3",()=>Result.Text = "Opt3"),
                ("Opt4",()=>Result.Text = "Opt4"),
                ("Opt5",()=>Result.Text = "Opt5"),
                ("Opt6",()=>Result.Text = "Opt6"),
                ("Opt7",()=>Result.Text = "Opt7"),
                ("Opt8",()=>Result.Text = "Opt8")
                );
        }
    }
}
