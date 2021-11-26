using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public ICollection<Element> ICE;

        public ICollection<Element> AR(ExternalCommandData commandData) //Возвращаемая колекция помещений
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Data.UIDOC = doc;
            FilteredElementCollector roomFilter = new FilteredElementCollector(doc);
            ICollection<Element> allRooms = roomFilter.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            ICE = allRooms;
            return allRooms;
        }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            RoomTable uwr = new RoomTable(AR(commandData), false);
            uwr.Show();
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

                        //Открываем таблицу и заносим все данные
                        //TODO :
                    }
                    
                    TaskDialog.Show("RevitInf", $"Было добавлено {ExtensionHelperListView.RoomTable_.rooms.Count} помещений\r\t{info}");
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
            ExtensionHelperListView EHLV = new ExtensionHelperListView();

            // Получить выбранные элементы из текущего документа.
            ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();

            //// Отображаем текущее количество выбранных элементов
            //TaskDialog.Show("Lodger", $"Вы выбрали - {selectedIds.Count.ToString()} элементов");
            
            // Пройдемся по выбранным элементам и отфильтруем только комнаты[помещения].
            ICollection<ElementId> selectedRoomIds = new List<ElementId>();

            foreach (ElementId id in selectedIds)
            {
                Element elements = uidoc.Document.GetElement(id);
                if (elements is Room)
                {

                    selectedRoomIds.Add(id);
                    Parameter par = elements.get_Parameter(BuiltInParameter.ROOM_AREA);
                    string strArea = par.AsValueString(/*Round*/);
                    double varDouble = par.AsDouble();
                    double ExactM2Area = varDouble / 10.7639111056;
                    double dArea = ExactM2Area;
                    
                    if(!ExtensionHelperListView.RoomTable_.rooms.Any(x => x.Name == elements.Name)) //Перебирая элементы проверим добавили ли мы их уже в коллекцию. Если нет
                    EHLV.AddToObserverCollection(elements.Name, dArea, ExactM2Area); //Добавляем в коллекцию и помещаем в ListView
                }
            }

            // Установить созданный набор элементов как текущий набор элементов выбора.
            uidoc.Selection.SetElementIds(selectedRoomIds);


            //// Даем пользователю некоторую информацию.
            //if (0 != selectedRoomIds.Count)
            //{
            //    TaskDialog.Show("Lodger", selectedRoomIds.Count.ToString() + " kомнат выбрано!");
            //}
            //else
            //{
            //    TaskDialog.Show("Lodger", "В выбранных элементах нет комнат!", TaskDialogCommonButtons.Close);
            //}

        }

        /// <summary>
        /// Возвращает имя класса для вызова функции
        /// </summary>
        public string ClassName
        {
            get
            {
                return $"LodgerBBP.{GetType().Name}";
            }
        }
    }
}
