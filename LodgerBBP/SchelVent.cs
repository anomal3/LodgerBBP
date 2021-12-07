using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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

            List<string> polya = new List<string>();
            List<ViewSchedule> lvs = new List<ViewSchedule>();
           


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
            //NF.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;//
            NF.ShowDialog(); //Открываем форму в виде диалога чтобы не спряталось где нибудь
            #endregion

            ViewSchedule schedule = ViewSchedule.CreateSchedule(document, new ElementId(BuiltInCategory.OST_Rooms), ElementId.InvalidElementId);
            
            schedule.Name = Data.NameSpecificSchedule; // schedule сама спецификация
            lvs.Add(schedule);
            foreach (SchedulableField schedulableField in schedule.Definition.GetSchedulableFields()) //Проходимся по полям и добавляем что нужно в спецификацию
            {
                //TaskDialog.Show("tester", schedulableField.FieldType.ToString());
                //ScheduleField field = schedule.Definition.AddField(schedulableField);

                if (schedulableField.GetName(document) == "Индекс квартиры")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Индекс квартиры";
                    field.SheetColumnWidth = .1d;

                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                    schedule.Definition.AddFilter(new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue));

                    /*ield.Definition.SetFieldOrder(ListschedulableFields);*/
                    //schedule.Definition.SetFilter(0, new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue));
                }

                if (schedulableField.GetName(document) == "ADSK_Площадь квартиры")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Площадь квартиры";
                    field.SheetColumnWidth = .14d;

                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                    var Filter = new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue);
                    schedule.Definition.AddFilter(Filter);
                }
                //else { schedule.Definition.RemoveField(); }


                if (schedulableField.GetName(document) == "Все помещения")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Площадь всех помещений";
                    field.SheetColumnWidth = .14d;
                    var Filter = new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue);
                    schedule.Definition.AddFilter(Filter);
                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                }

                if (schedulableField.GetName(document) == "Все помещения без лоджий и балконов")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Все помещения без лоджий и балконов";

                    field.SheetColumnWidth = .14d;
                    var Filter = new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue);
                    schedule.Definition.AddFilter(Filter);
                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                }
                
                if (schedulableField.GetName(document) == "Все помещения с коэф")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Все помещения с коэф";
                    field.SheetColumnWidth = .1d;
                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                    schedule.Definition.AddFilter(new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue));
                }
                
                if (schedulableField.GetName(document) == "Жилая площадь")
                {
                    ScheduleField field = schedule.Definition.AddField(schedulableField);
                    field.ColumnHeading = "Жилая площадь";
                    field.SheetColumnWidth = .1d;
                    field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                    schedule.Definition.AddFilter(new ScheduleFilter(field.FieldId, ScheduleFilterType.HasValue));
                   
                }
                //else
                //{

                //}

                polya.Add(schedulableField.GetName(document));
                
            }
            //AddFieldToSchedule(lvs);
            
            MessageBox.Show("Все найденные параметры\r\n" + string.Join("_", polya.ToArray()), "");
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

        /// <summary>
        /// Add fields to view schedule.
        /// </summary>
        /// <param name="schedules">List of view schedule.</param>
        public void AddFieldToSchedule(List<ViewSchedule> schedules)
        {
            IList<SchedulableField> schedulableFields = null;

            foreach (ViewSchedule vs in schedules)
            {
                //Get all schedulable fields from view schedule definition.
                schedulableFields = vs.Definition.GetSchedulableFields();

                foreach (SchedulableField sf in schedulableFields)
                {
                    bool fieldAlreadyAdded = false;
                    //Get all schedule field ids
                    IList<ScheduleFieldId> ids = vs.Definition.GetFieldOrder();
                    foreach (ScheduleFieldId id in ids)
                    {
                        //If the GetSchedulableField() method of gotten schedule field returns same schedulable field,
                        // it means the field is already added to the view schedule.
                        if (vs.Definition.GetField(id).GetSchedulableField() == sf)
                        {
                            fieldAlreadyAdded = true;
                            break;
                        }
                    }

                    //If schedulable field doesn't exist in view schedule, add it.
                    if (fieldAlreadyAdded == false)
                    {
                        vs.Definition.AddField(sf);
                    }
                }
            }
        }

        // format length units to display in feet and inches format
        public void FormatLengthFields(ViewSchedule schedule)
        {
            int nFields = schedule.Definition.GetFieldCount();
            for (int n = 0; n < nFields; n++)
            {
                ScheduleField field = schedule.Definition.GetField(n);
                if (field.GetSpecTypeId() == SpecTypeId.Length)
                {
                    FormatOptions formatOpts = new FormatOptions();
                    formatOpts.UseDefault = false;
                    formatOpts.DisplayUnits = DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES;
                    field.SetFormatOptions(formatOpts);
                }
            }
        }

    }
}