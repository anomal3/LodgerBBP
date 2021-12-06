using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LodgerBBP
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class MarkerStamp : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Autodesk.Revit.DB.Document doc = uidoc.Document;

            var OriginalValuesById = new Dictionary<int, string>();
            ICollection<ElementId> SelectedElements = uidoc.Selection.GetElementIds();

            using (Transaction trans = new Transaction(doc, "Change selected Elements Mark"))
            {
                trans.Start();

                foreach (ElementId id in SelectedElements)
                {
                    Element elem = doc.GetElement(id); //полчуаем элемент по ID
                   
                    Room roomroom = elem as Room;

                    RoomTag requiredRoomTag = null;
                    if (roomroom != null)
                    {
                        FilteredElementCollector coll = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RoomTags).WhereElementIsNotElementType();
                        foreach (Element ee in coll)
                        {
                            RoomTag RT = ee as RoomTag;
                            Room r = RT.Room;
                            if (r.Name == roomroom.Name)
                            {
                                //Required roomtag
                                requiredRoomTag = RT;
                                BoundingBoxXYZ bBox = requiredRoomTag.get_BoundingBox(doc.ActiveView);
                                if (bBox != null)
                                {
                                var FAMILY_PARAM = RT.get_Parameter(BuiltInParameter.ELEM_FAMILY_PARAM);

                                    //Family fam = doc.OwnerFamily;

                                    //doc.EditFamily(fam);

                                    //TaskDialog.Show("found!", $"family name: {GetFamilyName(elem)}");

                                }
                            }    
                        }
                    }

                }

                trans.Commit();
            }

            

            return Result.Succeeded;

        }
        public string ClassName
        {
            get
            {
                return $"LodgerBBP.{GetType().Name}";
            }
        }


        public static string GetFamilyName(Element e)
        {
            var eId = e?.GetTypeId();
            if (eId == null)
                return "";
            var elementType = e.Document.GetElement(eId) as ElementType;
            return elementType?.FamilyName ?? "";
        }


    }
}
