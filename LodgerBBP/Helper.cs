﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using Autodesk.Revit.DB.Architecture;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;

namespace LodgerBBP
{
    public class Helper
    {

        #region Простой конвертер изображений с перегрузкой метода в BtimapImage для отображения картинок на форме или для использования RevitAPIUI
        /// <summary>
        /// Конвертер из (по-умолчанию PNG) в BitMap
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static BitmapImage Convert (System.Drawing.Image img)
        {
            using (var memory = new MemoryStream())
            {
                img.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bImage = new BitmapImage();
                bImage.BeginInit();
                bImage.StreamSource = memory;
                bImage.CacheOption = BitmapCacheOption.OnLoad;
                bImage.EndInit();
                return bImage;
            }
        }

        /// <summary>
        /// Конвертер из выбранного формата в BitMap. Метод перегрузки
        /// </summary>
        /// <param name="img">Исходное изображение</param>
        /// <param name="fic">Выбор формата</param>
        /// <returns></returns>
        public static BitmapImage Convert(System.Drawing.Image img, FormatImageConverter fic)
        {
            ImageFormat IF = ImageFormat.Png;

            switch (fic)
            {
                case FormatImageConverter.PNG:
                    IF = ImageFormat.Png;
                    break;
                case FormatImageConverter.JPEG:
                    IF = ImageFormat.Jpeg;
                    break;
                case FormatImageConverter.BMP:
                    IF = ImageFormat.Bmp;
                    break;
                case FormatImageConverter.GIF:
                    IF = ImageFormat.Gif;
                    break;
                case FormatImageConverter.TIFF:
                    IF = ImageFormat.Tiff;
                    break;
            }
            using (var memory = new MemoryStream())
            {
                img.Save(memory, IF);
                memory.Position = 0;

                var bImage = new BitmapImage();
                bImage.BeginInit();
                bImage.StreamSource = memory;

                bImage.CacheOption = BitmapCacheOption.OnLoad;
                bImage.DecodePixelHeight = 32;
                bImage.DecodePixelWidth = 32;
                bImage.EndInit();
                return bImage;
            }
        }

        #endregion

        #region [enum] Нумерация формата картинок из ресурсов
        public enum FormatImageConverter
        {
            PNG = 0,
            JPEG = 1,
            BMP = 2,
            GIF =3,
            TIFF = 4
        }
        #endregion

        #region [enum] Нумерация для определения Нейминга [Имя, текст, подсказка, описание]
        public enum UINamingArray
        {
            NAME = 0,
            TEXT = 1,
            TOOLTIP = 2,
            DISCRIPTION = 3
        }
        #endregion

        #region Метод определения и автоматического заполнения ComboBox типа помещения
        /// <summary>
        /// Метод определения и автоматического заполнения ComboBox типа помещения
        /// </summary>
        /// <param name="line">Входящий параметр имя комнаты</param>
        public void RoomTypeDefinition(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                switch (line.ToLower())
                {
                    case "лоджия":
                        Data.iTypeRoomSlectionIndex = 3;
                        break;
                    case "балкон":
                        Data.iTypeRoomSlectionIndex = 2;
                        break;
                    default:
                        Data.iTypeRoomSlectionIndex = 0;
                        break;
                }
            }
        }
        #endregion


        #region Методы переопределения цветов выбора
        /// <summary>
        /// переопределение цвета для выделения
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="SelectionSemitransparent">Полупрозрачность эелемента</param>
        public static void SelectionColor(byte red, byte green, byte blue, bool SelectionSemitransparent)
        {
            ColorOptions ColOp = ColorOptions.GetColorOptions();
                ColOp.SelectionColor = new Autodesk.Revit.DB.Color(red, green, blue);
                ColOp.SelectionSemitransparent = SelectionSemitransparent;
        }
        /// <summary>
        /// Определяет цвет при наведении мыши на эелемент PreSelect
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public static void PreselectionColor(byte red, byte green, byte blue)
        {
            ColorOptions ColOp = ColorOptions.GetColorOptions();
            ColOp.PreselectionColor = new Autodesk.Revit.DB.Color(red, green, blue);
        }

        public static void ColorDefault()
        {
            ColorOptions ColOp = ColorOptions.GetColorOptions();
            ColOp.SelectionColor = new Autodesk.Revit.DB.Color(0, 59, 189);
            ColOp.PreselectionColor = new Autodesk.Revit.DB.Color(0, 59, 189);
        }

        #endregion

    }

    #region Класс HelperNaming Наименование элементов [Имя, текст, описание и т.п.]
    /// <summary>
    /// Публичный класс для Нейминга переменных для RevitAPIUI
    /// </summary>
    public class HelperNaming
    {
       public static string[] TableRoomAdd = { 
            "bTableRoomAdd", 
            "Таблица выбра помещений", 
            "Задаёт помещения в таблицу кликом мыши", 
            "Откроет дополнительно окно где можно будет мышкой указать только те помощения которые нужно поменять. Удобно для обсчёта отдельных помещений"
        };

        public static string[] SelectedTableRoom = {
            "bSelectedTableRoomAdd",
            "Таблица выбранных помещений",
            "Откроет таблицу уже выбранных помещений",
            "Перед выбором данной фунции необходимо заранее выбрать помещения которые нужно занести в таблицу"
        };

        public static string[] bBugReport = {
            "bBugRep",
            "БАГОРЕПОРТ",
            "Сообщить о найденом глюке",
            "Откроет форму для заполнения, и отправки глюка или ошибки"
        };

        public static string[] bProcessUpdate = {
            "bPUpdate",
            "Обновление",
            "Проверить наличие новых обновлений",
            "Критические обновлениия могут перезапустить ревит, что может повлечь последствия"
        };

        public static string[] bScheduleSheets = {
            "bSchedule",
            "Создать экспликацию",
            "Создаёт экспликацию текущего вида",
            "Перед созданием нужно будет задать имя, инчаче имя присвоится как \"Не названная экспликация (DateTime.Now)\""
        };
    }
    #endregion

    #region Класс Data для передачи параметров любого типа
    public static class Data
    {
        public static object obj { get; set; }
        public static Document UIDOC { get; set; }
        public static UIDocument ActiveUIDocument { get; set; }

        #region Версия
        public static Version AssemblyVers()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        public static String Version()
        {
            return $" v.0.{AssemblyVers().Major} | Сборка {AssemblyVers().Build} | Ревизия {AssemblyVers().Revision}";
        }
        #endregion

        public static ICollection<Element> CollectionElements { get; set; }

        public static string NameSpecificSchedule { get; set; }

        public static int iTypeRoomSlectionIndex { get; set; } //Передаваемый параметр типа помещения при заполнеии ComboBox

        public static readonly ObservableCollection<RoomCollectionToAppartament> RoomCol2App = new ObservableCollection<RoomCollectionToAppartament>();

    }
    #endregion

    #region Класс ExtensionHelperListView для обращения к форме RoomTable.xaml
    class ExtensionHelperListView
    {
        public static RoomTable RoomTable_ { get; set; }

        #region Устаревший метод добавления напрямую в ListView
        public void AddToList(string NameRoom, string strArea, double _ExactArea)
        {
            RoomTable_.c_LV.Items.Add(new RoomValue
            {
                Name = NameRoom,
                Area = _ExactArea,
                ExactArea = _ExactArea,
                TypeRoom = new RoomValue().TypeRoom
            });
        }
        #endregion

        int _ID = 0;
        public void AddToObserverCollection(string NameRoom, double dArea, double _ExactArea, List<ElementId> lei ,ElementId elemId)
        {
            RoomTable_.rooms.Add(new RoomValue
            {
                Name = NameRoom,
                Area = dArea,
                ExactArea = _ExactArea,
                TypeRoom = new string[] { "Жилая", "Не жилая", "Балкон(0.3)", "Лоджия(0.5)", "Терраса (0.3)" },
                ID = _ID++,
                ElementID = elemId
            });
            RoomTable_.c_LV.ItemsSource = RoomTable_.rooms;


            Data.RoomCol2App.Add(new RoomCollectionToAppartament
            {
                Name = NameRoom,
                ElementIdList = lei
            });
            
        }


        public void ClearItems()
        {
            try
            {
                if (RoomTable_.rooms.Count > 0)
                    RoomTable_.rooms.Clear();
            }
            catch { }
        }

        public static void ChangeTitle(string message)
        {
            RoomTable_.Title = message;
        }
    }
    #endregion

    public class RoomCollectionToAppartament
    {
        public string Name { get; set; }
        public List<ElementId> ElementIdList { get; set; } = new List<ElementId>();
    }
}
