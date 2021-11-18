using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace LodgerBBP
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class RoomTable : Window
    {
        ICollection<Element> allRooms;                                              //Переменная колекция помещений 
        /// <summary>
        /// Инициирует форму WPF
        /// </summary>
        /// <param name="elements">Коллекция комнат</param>
        /// <param name="isFillRoom">Заполнять сразу в ListView</param>
        public RoomTable(ICollection<Element> elements, bool isFillRoom)
        {
            InitializeComponent();

            allRooms = elements;

            FormatOptions Round = new FormatOptions();                              //
            Round.UseDefault = false;                                               //Метод округления чисел по математическому принципу. Используется библиотека Revit.DB
            Round.RoundingMethod = RoundingMethod.Up;                               //

            IExternalEventHandler DocumentPickEvent = new Document_PickEvent();     //Обявляем новое событие выделение по кнопке
            ExternalEvent exEvent = ExternalEvent.Create(DocumentPickEvent);        //Создадим событие класса и подпишемся на него

            this.Title += Data.Version();

            chkMath.Click += (s, a) =>
            {                                            //Чекбокс который делает округление. Проверяем если нажат то активируем элементы формы
                lbMathRound.IsEnabled = (bool)chkMath.IsChecked & c_LV.SelectedItems.Count >= 1 ? true : false;
                SlMathRound.IsEnabled = (bool)chkMath.IsChecked & c_LV.SelectedItems.Count >= 1 ? true : false;
            };

            bSelectPick.Click += (s, a) => { exEvent.Raise(); };                    /*Событие когда мы выбираем помещения мышкой*/

            #region Выполняем когда форма загружена
            this.Loaded += (s, e) => {
                System.Drawing.Image bSelRoom = Properties.Resources.add;
                ImageSource imgSrcTableRoom = Helper.Convert(bSelRoom, Helper.FormatImageConverter.PNG);
                iBtnPickSel.Source = imgSrcTableRoom;

                ExtensionHelperListView.RoomTable_ = this;
                
            };
            #endregion

            ////TODO : Сделать закрепляющую область если не будет сделано закреплённого окна
            chkTopMost.Click += (_sender, _event) =>
            {
                this.Topmost = (bool)chkTopMost.IsChecked ? true : false;
            };

           

            bSum.Click += BSum_Aera;                                                //Метод который суммирует выбранные помещения
            c_LV.SelectionChanged += C_LV_SelectionChanged;                         //Метод который выполняется если выбираем элемент

            #region При инициализации формы

            //TaskDialog.Show("Area Calculator", "Вот это поворот", TaskDialogCommonButtons.Close, TaskDialogResult.Close);

            foreach (var room in allRooms)                                          //Цикл перебора коллекций комнат
            {
                Parameter par = room.get_Parameter(BuiltInParameter.ROOM_AREA);     //Объявляем параметр и указываем что будем брать (какой параметр) из комнат

                string strArea = par.AsValueString(/*Round*/);                      //
                double varDouble = par.AsDouble();                                  //Вычисление прощади
                double ExactM2Area = varDouble / 10.7639111056;                     //

                if (isFillRoom)
                {
                    c_LV.Items.Add(new RoomValue                                        //Заносим в ListView наши полученные данные из комнат
                    {
                        Name = room.Name,
                        Area = strArea,
                        ExactArea = ExactM2Area
                    });
                }
                else
                {
                    //Добавляем коллекцию комнат в List

                }
            }

            #endregion
        }

        private void C_LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = c_LV.SelectedItem;
            var NameRoom = selectedItem.Name;
            foreach (var room in allRooms)
            {
                if (NameRoom == room.Name)                                               //Проверяем если то что мы выбрали есть в коллекции комнат
                {
                    //TODO : По логике нужно просто подсветить на плане ревита, то что мы выбрали в ListView
                    //new Quartography().uu1(); //TODO : link метода
                    //TaskDialog.Show("Совпадение!", $"{"s"}\rNameRoom = {NameRoom}\rroom.Name = {room.Name}", TaskDialogCommonButtons.Close, TaskDialogResult.Close);
                }
            }
        }

        #region Суммирование прощади
        private void BSum_Aera(object sender, RoutedEventArgs e)
        {
            SelectedListViewItemArea(c_LV);
        }


        private double SelectedListViewItemArea(ListView lv) //TODO : Переделать метод логики чтобы не заносились данные в TextBox 
        {
            if (lv.SelectedItems.Count != 0)
            {
                if (lv.SelectedItems.Count <= 1)
                {
                    dynamic selectedItem = lv.SelectedItem;
                    var Exact = selectedItem.ExactArea;
                    tbSelectArea.Text = string.Format("{0}", Exact);
                    return (double)Exact;
                }
                else
                {
                    double SumExact = 0d;
                    foreach (dynamic item in lv.SelectedItems)
                    {
                        var area = item.ExactArea;
                        SumExact += area;
                    }
                    tbSelectArea.Text = string.Format("{0}", SumExact);
                    return (double)SumExact;
                }
            }
            else return 0;
        }
        #endregion


        //Дублирование кода будет происходить. Изменить метод надо правильно SelectedListViewItemArea
        /// <summary>
        /// Округление чисел при помощи слайдера
        /// </summary>
        /// <param name="sender">передаваемый объект <oject as Slider></oject></param>
        /// <param name="e">Параметр слайдера</param>
        private void SlMathRound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double OldValue = SelectedListViewItemArea(c_LV);
            lbMathRound.Content = e.NewValue.ToString();
            double ExactSumMath = Math.Round(Convert.ToDouble(OldValue), (int)e.NewValue); //Округление проходит до двух знаков. сделать до 3
            tbSelectArea.Text = ExactSumMath.ToString();
        }
    }

    /// <summary>
    /// Класс нэминг таблицы в которую будем заносить в ListView на форме
    /// </summary>
    public class RoomValue
    {
        public string Name { get; set; }
        public string Area { get; set; }
        public double ExactArea { get; set; }
    }
}
