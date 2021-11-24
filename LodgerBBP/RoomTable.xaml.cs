using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using ComboBox = System.Windows.Controls.ComboBox;

namespace LodgerBBP
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class RoomTable : Window
    {
        public readonly ObservableCollection<RoomValue> rooms = new ObservableCollection<RoomValue>();

        ICollection<Element> allRooms;                                              //Переменная колекция помещений 
        /// <summary>
        /// Инициирует форму WPF
        /// </summary>
        /// <param name="elements">Коллекция комнат</param>
        /// <param name="isFillRoom">Заполнять сразу в ListView</param>
        public RoomTable(ICollection<Element> elements, bool isFillRoom)
        {
            DataContext = this;
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


            #region При инициализации формы

            //TaskDialog.Show("Area Calculator", "Вот это поворот", TaskDialogCommonButtons.Close, TaskDialogResult.Close);
            int _ID = 0;
            foreach (var room in allRooms)                                          //Цикл перебора коллекций комнат
            {
                Parameter par = room.get_Parameter(BuiltInParameter.ROOM_AREA);     //Объявляем параметр и указываем что будем брать (какой параметр) из комнат

                string strArea = par.AsValueString(/*Round*/);                      //
                double varDouble = par.AsDouble();                                  //Вычисление прощади
                double ExactM2Area = varDouble / 10.7639111056;                     //
                
                if (isFillRoom)
                {
                    #region Новый метод добавления в коллекцию goto:c_LV.ItemsSource = rooms;
                    rooms.Add(new RoomValue 
                    {
                        Name = room.Name,
                        Area = ExactM2Area,
                        ExactArea = ExactM2Area,
                        TypeRoom = new string[] { "Без коэф.", "Балкон ^0.3", "Лоджия ^0.5" },
                        ID = _ID++
                    });
                    #endregion

                    #region Устаревший и не правильный метод добавления элементов в ListView
                    //c_LV.Items.Add(new RoomValue                                        //Заносим в ListView наши полученные данные из комнат
                    //{
                    //    Name = room.Name,
                    //    Area = ExactM2Area,
                    //    ExactArea = ExactM2Area,
                    //    TypeRoom = new RoomValue().TypeRoom,
                    //    Title = new RoomValue().Title
                    //});
                    #endregion
                }
                else
                {
                    //Добавляем коллекцию комнат в List

                }
               // c_LV.ItemsSource = rooms;
            }

            #endregion
        }

        /// <summary>
        /// Происходит при изменении ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbTypeRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var comboBox = e.OriginalSource as ComboBox; //Получаем тыкнутый мышкой ComboBox

                var FocusItem = (FrameworkElement)e.OriginalSource; //Получаем элемент строки в зависимости на какой ComboBox мы тыкнули

                var focusItem = (RoomValue)FocusItem.DataContext;  //Получаем коллекцию вышеполученного элемента

                var fobj = rooms.Where(x => x.ID == focusItem.ID).FirstOrDefault();
                //var findObj = rooms.FirstOrDefault(x => x.ID == focusItem.ID); //Ищем наш объект для изменений в коллекции по условию ID который мы фокусим и ID который нужно изменить
                var OldArea = fobj.ExactArea;

                //TODO : Сделать повышающий коэфициент. Чтобы при выборе обратно площадь вернулась в исходное значение
                if (fobj != null)
                {
                    switch (comboBox.SelectedIndex)
                    {
                        case 0:
                            fobj.AREA = OldArea;
                            break;
                        case 1:
                            //Балкон *.3
                            fobj.AREA = OldArea * 0.3;
                            break;
                        case 2:
                            //Лоджия *.5
                            fobj.AREA = OldArea * 0.5;
                            break;
                    }

                }
            }
            catch (Exception ex) { TaskDialog.Show("erf", ex.Message); }
        }


        #region Суммирование прощади
        private void BSum_Aera(object sender, RoutedEventArgs e)
        {
            SelectedListViewItemArea(c_LV);
#if DEBUG
            TestSum(c_LV);
#endif
        }

#if DEBUG
        /// <summary>
        /// Тестовый метод, только для теста. 
        /// </summary>
        /// <param name="lv"></param>
        void TestSum(ListView lv)
        {
            foreach (var item in lv.Items)
            {
               
            }
        }
#endif

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

        private void c_LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }

    /// <summary>
    /// Класс нэминг таблицы в которую будем заносить в ListView на форме
    /// </summary>
    public class RoomValue : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double Area { get; set; }
        public double ExactArea { get; set; }
        public string[] TypeRoom { get; set; } = new string[] { };
        public int ID { get; set; }

        public ComboBox cbRoomType { get; set; } = new ComboBox();

        public event PropertyChangedEventHandler PropertyChanged;

        int coeff;
        public int Coeff
        {
            get
            {
                return coeff;
            }
            set
            {
                coeff = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coeff)));
            }
        }

        public double AREA
        {
            get { return Area; }
            set { Area = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AREA))); }
        }


        //public System.Windows.Controls.ComboBox TypeRoom { get; set; } = new System.Windows.Controls.ComboBox();
        //public ComboBox TypeRoom { get; set; }

    }

   
}
