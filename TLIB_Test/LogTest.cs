using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLIB;

namespace TLIB_Test
{
    [TestClass]
    public class LogTest
    {

        [TestMethod]
        public void MyMethod()
        {
            Log.IsFileLogEnabled = false;
            Log.IsInMemoryLogEnabled = true;
            Log.IsConsoleLogEnabled = true;

            Log.DisplayQuestionRequested += Log_DisplayQuestionRequested;

            int SelectedOption = -1;
            Log.DisplayChoice("The text is the answere", "1"
                , ("option1", () => { SelectedOption = 1; }
            )
                , ("possibility2", () => { SelectedOption = 2; }
            )
                , ("choice3", () => { SelectedOption = 3; }
            ));
            Assert.AreEqual(SelectedOption, 2);


            SelectedOption = -1;
            Log.DisplayChoice("The text is the answere", "2"
                , ("option1", () => { SelectedOption = 1; }
            )
                , ("possibility2", () => { SelectedOption = 2; }
            )
                , ("choice3", () => { SelectedOption = 3; }
            ));
            Assert.AreEqual(SelectedOption, 3);

            SelectedOption = -1;
            Log.DisplayChoice("The text is the answere", "4"
                , ("option1", () => { SelectedOption = 1; }
            )
                , ("possibility2", () => { SelectedOption = 2; }
            )
                , ("choice3", () => { SelectedOption = 3; }
            ));
            Assert.AreEqual(SelectedOption, -1);

            SelectedOption = -1;
            Log.DisplayChoice("The text is the answere", "0"
                , ("option1", () => { SelectedOption = 1; }
            )
                , ("possibility2", () => { SelectedOption = 2; }
            )
                , ("choice3", () => { SelectedOption = 3; }
            ));
            Assert.AreEqual(SelectedOption, 1);
        }

        private void Log_DisplayQuestionRequested(string title, string text, ResultCallback choice, params string[] choices)
        {
            //Imagine show a messagebox or so
            // Selecting answere 2
            choice.SendResultNo(int.Parse(text));
        }
    }
}
