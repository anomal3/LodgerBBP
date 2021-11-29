using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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

            
            IExternalEventHandler DocumentPickEvent = new Document_PickEvent();     //Обявляем новое событие выделение по кнопке
            ExternalEvent exEvent = ExternalEvent.Create(DocumentPickEvent);        //Создадим событие класса и подпишемся на него

            allRooms = elements;

            this.Loaded += RoomTable_Loaded;

            tbNewNameAdd.Loaded += (s, a) => {
                tbNewNameAdd.Foreground = new SolidColorBrush(Colors.Gray);
                tbNewNameAdd.FontStyle = FontStyles.Italic;
                tbNewNameAdd.Text = "0";
            };                                   //

            tbNewNameAdd.GotFocus += RemoveText;                                   //Метод с текстовым полем 
            tbNewNameAdd.LostFocus += AddText;                                     //
            lbAppartament.LostFocus += (s, a) => { lbAppartament.SelectedIndex = -1; };

            FormatOptions Round = new FormatOptions();                              //
            Round.UseDefault = false;                                               //Метод округления чисел по математическому принципу. Используется библиотека Revit.DB
            Round.RoundingMethod = RoundingMethod.Up;                               //


            this.Title += Data.Version();

            chkMath.Click += (s, a) =>
            {                                            //Чекбокс который делает округление. Проверяем если нажат то активируем элементы формы
                lbMathRound.IsEnabled = (bool)chkMath.IsChecked & c_LV.SelectedItems.Count >= 1 ? true : false;
                SlMathRound.IsEnabled = (bool)chkMath.IsChecked & c_LV.SelectedItems.Count >= 1 ? true : false;
            };

            bSelectPick.Click += (s, a) => {
                new ExtensionHelperListView().ClearItems();
                exEvent.Raise(); 
            };                    /*Событие когда мы выбираем помещения мышкой*/


            bSelectPick.MouseEnter += MouseEnterButtonSound;
            bUpdateList.MouseEnter += MouseEnterButtonSound;
            bSum.MouseEnter += MouseEnterButtonSound;
            

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
                    new Helper().RoomTypeDefinition(room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString());

                    #region Новый метод добавления в коллекцию goto:c_LV.ItemsSource = rooms;
                    rooms.Add(new RoomValue
                    {
                        Name = room.get_Parameter(BuiltInParameter.ROOM_NAME).AsString(),
                        Area = ExactM2Area,
                        ExactArea = ExactM2Area,
                        TypeRoom = new RoomValue().TypeRoom,
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
               c_LV.ItemsSource = rooms;

            }

            #endregion

            bAddAppart.Click += (s, a) => {
                if (!string.IsNullOrEmpty(tbSection.Text) & !string.IsNullOrEmpty(tbRoof.Text) & Convert.ToInt32(tbNewNameAdd.Text) != 0)
                {
                    string NameAppart = $"{tbSection.Text}-{tbRoof.Text}-{tbNewNameAdd.Text}";
                    ExtensionHelperListView.AddAppartament(c_LV, NameAppart);
                    lbAppartament.ItemsSource = Data.RoomCol2App;
                    tbNewNameAdd.Text = "0";
                }
                else
                {
                    TaskDialog.Show("Error added", "Номер аппартаментов должен быть заполнен!");
                    return;
                }
            };


            lbAppartament.MouseDoubleClick += (s, a) => {
                var FocusItem = (FrameworkElement)a.OriginalSource;
                if (FocusItem == null) return;
                else {

                    try
                    {
                        var focusItem = (RoomCollectionToAppartament)FocusItem.DataContext;

                        if (a.ChangedButton == MouseButton.Left)
                        {
                            //Добавляем контекстное меню
                            MenuItem ShowRoom = new MenuItem();
                            ShowRoom.Header = $"Отобразить";
                            ShowRoom.Click += (@sender, @event) => { ExtensionHelperListView.AppartamentSelectedShowDocument(focusItem.NameAppartament, true); };

                            MenuItem DelRoom = new MenuItem();
                            DelRoom.Header = "Удалить";
                            DelRoom.Click += (@sender, @event) => { Data.RoomCol2App.Remove(Data.RoomCol2App.FirstOrDefault(x => x.NameAppartament == focusItem.NameAppartament)); };

                            ContextMenu cm = new ContextMenu();
                            cm.Items.Add(ShowRoom);
                            cm.Items.Add(DelRoom);
                            cm.IsOpen = true;
                        }
                        else
                        {
                            ExtensionHelperListView.AppartamentSelectedShowDocument(focusItem.NameAppartament, false);
                        }
                    }
                    catch { }
                }
            };
        }


        #region Placeholder текстового поля названия новой квартиры
        public void RemoveText(object sender, EventArgs e)
        {
            tbNewNameAdd.Foreground = new SolidColorBrush(Colors.Black);
            tbNewNameAdd.FontStyle = FontStyles.Normal;

            if (tbNewNameAdd.Text == "0")
            {
                tbNewNameAdd.Text = "";
            }
        }
       

        public void AddText(object sender, EventArgs e)
        {
            //Поправить в  обновлении PlaceHoldedr
            tbNewNameAdd.Foreground = new SolidColorBrush(Colors.Gray);
            tbNewNameAdd.FontStyle = FontStyles.Italic;
            if (string.IsNullOrWhiteSpace(tbNewNameAdd.Text)/* || Convert.ToInt32(tbNewNameAdd.Text) == 0*/)
            {
                //tbNewNameAdd.Foreground = new SolidColorBrush(Colors.Gray);
                //tbNewNameAdd.FontStyle = FontStyles.Italic;
                tbNewNameAdd.Text = "0";
            }
        }
        #endregion

        private void RoomTable_Loaded(object sender, RoutedEventArgs e)
        {
            if(Data.RoomCol2App.Count > 0) lbAppartament.ItemsSource = Data.RoomCol2App;
        }

       

        #region Метод вопсроизведения звука при наведении мыша на кнопку
        private void MouseEnterButtonSound(object s, MouseEventArgs e)
        {
            using (MemoryStream fileOut = new MemoryStream(Properties.Resources.MouseEnterElement))
            using (GZipStream gz = new GZipStream(fileOut, CompressionMode.Decompress))
                new SoundPlayer(gz).Play();
        }
        #endregion
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

                    
                    if (fobj != null)
                    {
                        switch (comboBox.SelectedIndex)
                        {
                            case 2:
                                //Балкон *.3
                                fobj.AREA = OldArea * 0.3;
                                break;
                            case 4:
                                //Терраса *.3
                                fobj.AREA = OldArea * 0.3;
                                break;
                            case 3:
                                //Лоджия *.5
                                fobj.AREA = OldArea * 0.5;
                                break;
                            default:
                                fobj.AREA = OldArea;
                                break;
                        }

                    }
                }
                catch (Exception ex) {  }
            
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
            for(int i = 0; i < lv.SelectedItems.Count; i++)
            {
                var to2 = lv.Items.IndexOf(lv.SelectedItems[i]);
                MessageBox.Show(rooms[to2].Name + rooms[to2].ExactArea.ToString());
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
        public RoomValue() //Конструктор для того чтобы установить по-умолчанию значение коэф.
        {
            if (Data.iTypeRoomSlectionIndex >= -1)
                SelectedIndex = Data.iTypeRoomSlectionIndex;
        }

        public RoomValue(RoomCollectionToAppartament _roomCollectionToAppartament)
        {
            this.RoomCollectionToAppartament = _roomCollectionToAppartament;
        }

        public RoomCollectionToAppartament RoomCollectionToAppartament { get;set;}

        public string Name { get; set; } //Имя помещения
        public double Area { get; set; }
        public double ExactArea { get; set; }
        public string[] TypeRoom { get; set; } = new string[] { "Жилая", "Не жилая", "Балкон(0.3)", "Лоджия(0.5)", "Терраса (0.3)" };
        public int ID { get; set; } //ID порядкового номера
      
        

        public ElementId ElementID { get; set; } //Реальный ID елемента

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

       
        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("SelectedIndex"));
            }
        }

      
        //public System.Windows.Controls.ComboBox TypeRoom { get; set; } = new System.Windows.Controls.ComboBox();
        //public ComboBox TypeRoom { get; set; }

    }

   
}
