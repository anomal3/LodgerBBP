using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodgerBBP
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class SchelVent : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                UIDocument uiDocument = commandData.Application.ActiveUIDocument;

                ScheduleCreationUtility utility = new ScheduleCreationUtility();
                utility.CreateAndAddSchedule(uiDocument);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
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

    class ScheduleCreationUtility
    {
        bool drag = false;
        System.Drawing.Point startDragPoint;
        System.Windows.Forms.Form NF;

        #region Перетаскивание формы ввода имени экспликации

        private void NFMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            drag = false;
        }

        private void NFMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Y > 32) return;
            drag = true;
            startDragPoint = new System.Drawing.Point(e.X, e.Y);
        }

        void Drag(System.Windows.Forms.MouseEventArgs e)
        {
            if (drag)
            {
                System.Drawing.Point p1 = new System.Drawing.Point(e.X, e.Y);
                System.Drawing.Point p2 = NF.PointToScreen(p1);
                System.Drawing.Point p3 = new System.Drawing.Point(p2.X - this.startDragPoint.X,
                                     p2.Y - this.startDragPoint.Y);
                NF.Location = p3;
                
            }
        }

        private void NFMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Drag(e);
        }
        #endregion

        private static BuiltInParameter[] addedParameters = new BuiltInParameter[7]; //{ BuiltInParameter.ALL_MODEL_MARK };

        public void CreateAndAddSchedule(UIDocument uiDocument)
        {
            TransactionGroup tGroup = new TransactionGroup(uiDocument.Document, "Create schedule");
            tGroup.Start();

            ViewSchedule schedules = CreateSchedule(uiDocument);
            tGroup.Assimilate();
        }

        private ViewSchedule CreateSchedule(UIDocument uiDocument)
        {
            Document document = uiDocument.Document;

            Transaction t = new Transaction(document, "Create Schedules");
            t.Start();

            //IList<ElementId> categories = new List<ElementId>;
            //categories.Add(new ElementId(BuiltInCategory.OST_DuctCurves));
            //categories.Add(new ElementId(BuiltInCategory.OST_DuctFitting));
            //categories.Add(new ElementId(BuiltInCategory.OST_DuctAccessory));
            //categories.Add(new ElementId(BuiltInCategory.OST_DuctTerminal));
            //categories.Add(new ElementId(BuiltInCategory.OST_MechanicalEquipment));
            //categories.Add(new ElementId(BuiltInCategory.OST_FlexDuctCurves));
            #region Форма для ввода пользователем Названия листа спецификации

            NF = new System.Windows.Forms.Form(); //Инициируем нашу форму ввода
            System.Windows.Forms.TextBox tbName = new System.Windows.Forms.TextBox(); //Инициируем  текстовое поле
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button() {     //Инициируем кнопку приминения
                Text = "ОК",
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Dock = System.Windows.Forms.DockStyle.Bottom,
                BackColor = System.Drawing.Color.FromArgb(153, 180, 209)
        };
            System.Windows.Forms.LinkLabel lbl = new System.Windows.Forms.LinkLabel() { //Инициируем подсказку
                Text = "Введите название листа спецификации",
                Location = new System.Drawing.Point(49, 9),
                AutoSize = false,
                Size = new System.Drawing.Size(209, 13),
            };
            string SpecificName = $"Не названная спецификация ({DateTime.Now.Day}_{ DateTime.Now.Month}_{DateTime.Now.Year}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second})";
            btn.Click += (s, a) => { Data.NameSpecificSchedule = !string.IsNullOrEmpty(tbName.Text) ? tbName.Text : SpecificName; NF.Close(); }; //Объявляем имя спецификации, и защитимся если имя не присвоено

            NF.Load += (s, a) => { NF.KeyPreview = true; }; //При старте формы активируем горячие клавиши
            NF.KeyDown += (s,a) => { if (a.KeyCode == System.Windows.Forms.Keys.Enter) btn.PerformClick(); }; //Если нажмём кнопу Enter & Escape то имитируем нажатие кнопки ОК
            NF.Size = new System.Drawing.Size(310, 80); //Зададим размер формы
            NF.BackColor = System.Drawing.Color.Silver; //Задний фон немного подтемним
            NF.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; //Стиль формы сделаем без кнопок навигации
            NF.MouseUp += NFMouseUp;                    //
            NF.MouseDown += NFMouseDown;                //Сделаем умное перетаскивание формы
            NF.MouseMove += NFMouseMove;                //

            tbName.Width = 286;                                                     //Правим размер и положение нашего текстового поля
            tbName.Location = new System.Drawing.Point(12, 25);
            

            NF.Controls.Add(tbName);  //Добавляем кнотролы к инициированной форме
            NF.Controls.Add(btn);
            NF.Controls.Add(lbl);
            NF.ShowDialog(); //Открываем форму в виде диалога чтобы не спряталось где нибудь
            #endregion

            ViewSchedule schedule = ViewSchedule.CreateSchedule(document, new ElementId(BuiltInCategory.OST_Rooms), ElementId.InvalidElementId);
            schedule.Name = Data.NameSpecificSchedule;
            foreach (SchedulableField schedulableField in schedule.Definition.GetSchedulableFields()) //Проходимся по полям и добавляем что нужно в спецификацию
            {
                //TaskDialog.Show("tester", schedulableField.FieldType.ToString());
                ScheduleField field = schedule.Definition.AddField(schedulableField);
                
            }

            t.Commit();

            uiDocument.ActiveView = schedule;


            return schedule;

        }

        private bool ShouldBeAdded(ElementId parameterId)
        {
            foreach (BuiltInParameter bip in addedParameters)
            {
                if (new ElementId(bip) == parameterId)
                    return true;
            }
            return false;
        }

      
    }
}