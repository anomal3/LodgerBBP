using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using LodgerBBP.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LodgerBBP
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class Document_PickEvent : IExternalEventHandler
    {
        public Document doc;
        public Autodesk.Revit.ApplicationServices.Application RevitApp;

        #region События для передачи [не используется]
        public delegate void ActionDocumentPick(object sender, Document_PickEventArgs e);
        public event ActionDocumentPick ActDP;
        #endregion



        public void Execute(UIApplication uiapp)
        {
            //new MsgBox("Простое кастомное окно для вызова", "Это будет заголовок", MsgBox.MsgBoxIcon.Info, MsgBox.MsgBoxButton.OK).Show();
            ExtensionHelperListView.ChangeTitle("Выбираем квартиры");
            ExtensionHelperListView EHLV = new ExtensionHelperListView();
            EHLV.ClearItems();

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Data.UIApplication = uiapp;
            Data.ActiveUIDocument = Data.UIApplication.ActiveUIDocument;
            if (null == uidoc)
            {
                return; // no document, nothing to do
            }
            Document doc = uidoc.Document;
            Data.ActiveDocument = doc;
            Data.ActiveUIDocument = uiapp.ActiveUIDocument;

            Autodesk.Revit.ApplicationServices.Application app = uiapp.ActiveUIDocument.Application.Application;
            
            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("DocumentPickEvent");
                //===============================================//
                // Action within valid Revit API context thread
                List<ElementId> elemIdList = new List<ElementId>();
                List<Element> elemList = new List<Element>();

                #region Убираем бесконечный цикл и Try Catch
                //try
                //{

                //while (true)
                //{
                #endregion
                Selection sel = uidoc.Selection;

                #region Выбор цвета https://thebuildingcoder.typepad.com/blog/2019/09/whats-new-in-the-revit-20201-api.html#2.5.1
                //Helper.SelectionColor(128,255,64,true);
                //Helper.PreselectionColor(255,0,0);
                #endregion
                Helper.SelectionColor(255,10,0, true);
                Helper.PreselectionColor(200, 0, 0);

                IList<Reference> objRefsToCopy = sel.PickObjects(ObjectType.Element, "Выберите помещения для добавления в коллекцию");

                //XYZ basePoint = sel.PickPoint("Pick base point");

                //TaskDialog.Show("dsa", "dsada");
                foreach (Reference r in objRefsToCopy)
                {
                    
                    Element element2Ref = uidoc.Document.GetElement(r.ElementId);
                    if (element2Ref is Room)
                    {
                        elemIdList.Add(r.ElementId);
                        elemList.Add(element2Ref);

                        if (objRefsToCopy.Count != 0)
                        {
                            Parameter par = element2Ref.get_Parameter(BuiltInParameter.ROOM_AREA);
                            //string strArea = par.AsValueString(/*Round*/);
                            double varDouble = par.AsDouble();
                            double ExactM2Area = varDouble / 10.7639111056;
                            double dArea = ExactM2Area;
                            //EHLV.AddToList(element2Ref.Name, strArea, ExactM2Area);
                            new Helper().RoomTypeDefinition(element2Ref.get_Parameter(BuiltInParameter.ROOM_NAME).AsString());
                            EHLV.AddToObserverCollection(element2Ref.get_Parameter(BuiltInParameter.ROOM_NAME).AsString(), dArea, ExactM2Area, r.ElementId, uidoc);
                            // Data.MinorNumberRoom.Add(Convert.ToInt32(element2Ref.get_Parameter((BuiltInParameter)292423).AsString()));
                            //Parameter IndexRoom = element2Ref.get_Parameter((BuiltInParameter)392429);
                            //IndexRoom.Set("ПРОСТО НАФИГ ИНДЕКС");

                        }
                    }
                    
                    //ActDP?.Invoke(this, new Document_PickEventArgs(element2Ref.Name));
                }

                

                uidoc.Selection.SetElementIds(elemIdList);
                uidoc.ShowElements(elemIdList);
                Helper.ColorDefault();

                var td = new TaskDialog("Info");                                    //Всплывающее окно
                //td.MainInstruction = uiapp.ActiveUIDocument.Selection.GetElementIds().Aggregate("", (ss, el) => ss + "," + el).TrimStart(','); // Выводит ID выбранных пом-ий
                td.MainInstruction = $" Добавлено {objRefsToCopy.Count} помещения!";
                td.TitleAutoPrefix = false;
                td.Show();

                //uidoc.Selection.SetElementIds(elemIdList); //Подсветить элементы 
                #region Устаревший метод выбра
                //elemIdList.Add(uidoc.Selection.PickObject(ObjectType.Element, "Выберите помещения для добавления в коллекцию").ElementId);
                //if (elemIdList != null & elemIdList.Count != 0)
                //{
                //    EHLV.ClearItems();
                //    foreach (ElementId id in elemIdList)
                //    {
                //        Element elements = uidoc.Document.GetElement(id);
                //        elemList.Add(elements);

                //        Parameter par = elements.get_Parameter(BuiltInParameter.ROOM_AREA);
                //        string strArea = par.AsValueString(/*Round*/);
                //        double varDouble = par.AsDouble();
                //        double ExactM2Area = varDouble / 10.7639111056;
                //        EHLV.AddToList(elements.Name, strArea, ExactM2Area);

                //        ActDP?.Invoke(this, new Document_PickEventArgs(elements.Name));

                //        uidoc.Selection.SetElementIds(elemIdList);
                //        uidoc.RefreshActiveView();
                //        doc.Regenerate();
                //    }
                //}

                //}
                //}
                //catch
                //{
                //    uidoc.Selection.SetElementIds(elemIdList);

                //    

                //    var td = new TaskDialog("Info");                                    //Всплывающее окно
                //    //td.MainInstruction = uiapp.ActiveUIDocument.Selection.GetElementIds().Aggregate("", (ss, el) => ss + "," + el).TrimStart(','); // Выводит ID выбранных пом-ий
                //    td.MainInstruction = $" Добавлено {uiapp.ActiveUIDocument.Selection.GetElementIds().Count.ToString()} помещения!";
                //    td.TitleAutoPrefix = false;
                //    td.Show();
                //}
                #endregion
                uidoc.RefreshActiveView();
                //===============================================//
                tx.Commit();

                ExtensionHelperListView.ChangeTitle(Data.Version()); //Возвращаем версию приложения после выбора
            }
        }

      

        #region Смена цвета выбранного элемента [Не поддерживается]
        public void ChangeElementColor(UIApplication uiapp)
        {
            Autodesk.Revit.ApplicationServices.Application app = uiapp.ActiveUIDocument.Application.Application;

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Color color = app.Create.NewColor();
            color.Blue = (byte)150;
            color.Red = (byte)200;
            color.Green = (byte)200;

            Selection sel = uidoc.Selection;

            Reference ref1 = sel.PickObject(ObjectType.Element, "Pick element to change its colour");

            Element elem = uidoc.Document.GetElement(ref1.ElementId);

            List<ElementId> ids = new List<ElementId>(1);
            ids.Add(elem.Id);

            Transaction trans = new Transaction(doc);
            trans.Start("ChangeColor");

            doc.ActiveView.SetElementOverrides(ids[0], new OverrideGraphicSettings());

            trans.Commit();
        }
        #endregion

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
