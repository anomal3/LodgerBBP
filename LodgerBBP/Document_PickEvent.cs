using Autodesk.Revit.ApplicationServices;
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
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Document_PickEvent : IExternalEventHandler
    {
        public Document doc;
        public Application RevitApp;

        #region События для передачи [не используется]
        public delegate void ActionDocumentPick(object sender, Document_PickEventArgs e);
        public event ActionDocumentPick ActDP;
        #endregion



        public void Execute(UIApplication uiapp)
        {
            ExtensionHelperListView.ChangeTitle("Выбираем квартиры");
            ExtensionHelperListView EHLV = new ExtensionHelperListView();

            UIDocument uidoc = uiapp.ActiveUIDocument;
            if (null == uidoc)
            {
                return; // no document, nothing to do
            }
            Document doc = uidoc.Document;
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("DocumentPickEvent");
                //===============================================//
                // Action within valid Revit API context thread



                List<ElementId> elemIdList = new List<ElementId>();

                List<Element> elemList = new List<Element>();
                try
                {

                    while (true)
                    {
                        elemIdList.Add(uidoc.Selection.PickObject(ObjectType.Element, "Выберите помещения для добавления в коллекцию").ElementId);
                        if (elemIdList != null & elemIdList.Count != 0)
                        {
                            EHLV.ClearItems();
                            foreach (ElementId id in elemIdList)
                            {
                                Element elements = uidoc.Document.GetElement(id);
                                elemList.Add(elements);

                                Parameter par = elements.get_Parameter(BuiltInParameter.ROOM_AREA);
                                string strArea = par.AsValueString(/*Round*/);
                                double varDouble = par.AsDouble();
                                double ExactM2Area = varDouble / 10.7639111056;
                                EHLV.AddToList(elements.Name, strArea, ExactM2Area);

                                ActDP?.Invoke(this, new Document_PickEventArgs(elements.Name));
                            }
                        }
                    }
                }
                catch
                {
                    uidoc.Selection.SetElementIds(elemIdList);

                    //TODO : В собитии передаём комнату

                    var td = new TaskDialog("Info");                                    //Всплывающее окно
                    //td.MainInstruction = uiapp.ActiveUIDocument.Selection.GetElementIds().Aggregate("", (ss, el) => ss + "," + el).TrimStart(','); // Выводит ID выбранных пом-ий
                    td.MainInstruction = $" Добавлено {uiapp.ActiveUIDocument.Selection.GetElementIds().Count.ToString()} помещения!";
                    td.TitleAutoPrefix = false;
                    td.Show();


                }



                uidoc.RefreshActiveView();

                //===============================================//
                tx.Commit();
            }
        }
        public string GetName()
        {

            return "DocumentPickEvent";
        }
    }

    public class Document_PickEventArgs
    {
        public string room { get; }

        public Document_PickEventArgs(string _room)
        {
            room = _room;
        }
    }


}
