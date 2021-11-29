#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace LodgerBBP
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {

        public ICollection<Element> ICE;

        public ICollection<Element> AR(ExternalCommandData commandData) //Возвращаемая колекция помещений
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Data.ActiveDocument = doc;
            FilteredElementCollector roomFilter = new FilteredElementCollector(doc);
            ICollection<Element> allRooms = roomFilter.OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
            ICE = allRooms;
            return allRooms;
        }

        public ICollection<Element> ART(ExternalCommandData commandData) //Возвращаемая колекция помещений
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Data.ActiveDocument = doc;
            FilteredElementCollector roomFilter = new FilteredElementCollector(doc);
            ICollection<Element> allRooms = roomFilter.OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsNotElementType().ToElements();
            ICE = allRooms;
            return allRooms;
        }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //UIApplication uiapp = commandData.Application;
            //UIDocument uidoc = uiapp.ActiveUIDocument;
            //Application app = uiapp.Application;
            //Document doc = uidoc.Document;

            //// Access current selection

            //Selection sel = uidoc.Selection;

            //// Retrieve elements from database

            //FilteredElementCollector col
            //  = new FilteredElementCollector(doc)
            //    .WhereElementIsNotElementType()
            //    .OfCategory(BuiltInCategory.INVALID)
            //    .OfClass(typeof(Wall));

            //// Filtered element collector is iterable

            //foreach (Element e in col)
            //{
            //    Debug.Print(e.Name);
            //}

            //// Modify document within a transaction

            //using (Transaction tx = new Transaction(doc))
            //{
            //    tx.Start("Transaction Name");
            //    tx.Commit();
            //}

            RoomTable uwr = new RoomTable(AR(commandData), false);
            uwr.Show();
            return Result.Succeeded;
        }
    }
}
