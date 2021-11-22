using System;
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
        public static BitmapImage Convert (Image img)
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
        public static BitmapImage Convert(Image img, FormatImageConverter fic)
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

    }
    #endregion

    #region Класс ExtensionHelperListView для обращения к форме RoomTable.xaml
    class ExtensionHelperListView
    {
        public static RoomTable RoomTable_ { get; set; }

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

        public void ClearItems()
        {
            RoomTable_.c_LV.Items.Clear();
        }

        public static void ChangeTitle(string message)
        {
            RoomTable_.Title = message;
        }
    }
    #endregion
}
