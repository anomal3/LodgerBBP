using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodgerBBP
{
    //Класс реализующий принцип сначала выбрали комнаты потом нажали кнопку => получили информацию

    // [Transaction(TransactionMode.ReadOnly)]
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Document_Selection : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //Выберите некоторые элементы в Revit перед вызовом этой команды
                //Получить дескриптор текущего документа.
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                //Получить выбор элемента текущего документа.
                Selection selection = uidoc.Selection;
                ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

                if (0 == selectedIds.Count)
                {
                    //Если элементы не выбраны
                    TaskDialog.Show("Quartography", "Вы должны сначала выбрать одну из комнат!", TaskDialogCommonButtons.Ok);
                }
                else
                {
                    String info = "ID выбранных элементов в документе: ";
                    foreach (ElementId id in selectedIds)
                    {

                        info += "\n\t" + id.IntegerValue;
                        ChangeSelection(uidoc);
                    }

                    TaskDialog.Show("Revit", info);
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        private void ChangeSelection(UIDocument uidoc)
        {


            // Получить выбранные элементы из текущего документа.
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            // Отображаем текущее количество выбранных элементов
            TaskDialog.Show("Revit", "Количество выбранных элементов: " + selectedIds.Count.ToString());

            // Пройдемся по выбранным элементам и отфильтруем только стены.
            ICollection<ElementId> selectedWallIds = new List<ElementId>();

            foreach (ElementId id in selectedIds)
            {
                Element elements = uidoc.Document.GetElement(id);
                if (elements is Wall)
                {
                    selectedWallIds.Add(id);
                }
            }

            // Установить созданный набор элементов как текущий набор элементов выбора.
            uidoc.Selection.SetElementIds(selectedWallIds);


            // Даем пользователю некоторую информацию.
            if (0 != selectedWallIds.Count)
            {
                TaskDialog.Show("Revit", selectedWallIds.Count.ToString() + " стен выбрано!");
            }
            else
            {
                TaskDialog.Show("Revit", "Нет стен в выбранных элементах!");
            }

        }

        public string ClassName
        {
            get
            {
                return $"LodgerBBP.{GetType().Name}";
            }
        }
    }
}
