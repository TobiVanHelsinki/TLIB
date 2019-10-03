using System.ComponentModel;
using System.Runtime.CompilerServices;
using TLIB;
using Xamarin.Forms;

namespace TLIBChoice.Xamarin_Test
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        #region NotifyPropertyChanged
        public new event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        string _Title = "ATTANTION! Importanté!";
        public string Caption
        {
            get { return _Title; }
            set { if (_Title != value) { _Title = value; NotifyPropertyChanged(); } }
        }

        string _Text = "This is a very important choice you have to made, young padawan. Choose wise and an not unimportant amount of money will be granted to you.";
        public string Text
        {
            get { return _Text; }
            set { if (_Text != value) { _Text = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(TextLength)); } }
        }
        public int TextLength => _Text.Length;


        int _Columns;
        public int Columns
        {
            get { return _Columns; }
            set { if (_Columns != value) { _Columns = value; NotifyPropertyChanged(); } }
        }

        int _Presentation;
        public int Presentation
        {
            get { return _Presentation; }
            set { if (_Presentation != value) { _Presentation = value; NotifyPropertyChanged(); } }
        }
        int _Orientation;
        public int Orientation
        {
            get { return _Orientation; }
            set { if (_Orientation != value) { _Orientation = value; NotifyPropertyChanged(); } }
        }


        public MainPage()
        {
            InitializeComponent();
            Log.DisplayChoiceRequested += TLIB.Choice.Xamarin.TLIBChoice_Xamarin.Log_DisplayQuestionRequested;
            BindingContext = this;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            Log.DisplayChoice(Caption, Text
                , new Options() { ButtonColumns = Columns, ButtonOrientation = (Orientation)Orientation, ButtonPresentation = (Presentation)Presentation },
                ("Opt1", () => Result.Text = "Opt1"),
                ("Opt2", () => Result.Text = "Opt2"),
                ("Opt3", () => Result.Text = "Opt3"),
                ("Opt4", () => Result.Text = "Opt4"),
                ("Opt5", () => Result.Text = "Opt5"),
                ("Opt6", () => Result.Text = "Opt6"),
                ("Opt7", () => Result.Text = "Opt7"),
                ("Opt8", () => Result.Text = "Opt8")
                );
        }
    }
}
