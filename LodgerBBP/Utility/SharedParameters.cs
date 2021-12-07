using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LodgerBBP.Utility
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SharedParameters : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            bool isSucceeded = false;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;

            DialogResult result = MessageBox.Show("Сейчас будет добавлен файл общих параметров в проект." +
                "\n\nПосле чего данные будут имплементированы в эти параметры\r\nВыполнить?", "[Рекомендуется]", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            switch (result)
            {
                case DialogResult.Yes:
                    new CreateSharedParameter().CreateShared_Parameter(doc, app);
                    isSucceeded = true;
                    break;
                case DialogResult.No:
                    //Nothing 
                    break;
            }

            

            return isSucceeded ? Result.Succeeded : Result.Failed;
        }

        public string ClassName
        {
            get
            {
                return $"LodgerBBP.Utility.{GetType().Name}";
            }
        }
    }
}
