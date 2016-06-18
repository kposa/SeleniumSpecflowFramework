using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecflowTest.Library.Pages
{
    public class ResultsPage
    {
        Actions.Actions action = new Actions.Actions();
        private string FileInput = "xmlFileinput_Id";
        private string resultSummary = "appName_Id";


        public void ClickChooseFileButton()
        {
            action.ClickElement(FileInput);
        }

        public void VerifyImportedFile()
        {
            action.IsExists(resultSummary);
        }
    }
}
