using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

namespace LodgerBBP
{
    public class Helper
    {
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

        public enum FormatImageConverter
        {
            PNG = 0,
            JPEG = 1,
            BMP = 2,
            GIF =3,
            TIFF = 4
            
        }

        public enum UINamingArray
        {
            NAME = 0,
            TEXT = 1,
            TOOLTIP = 2,
            DISCRIPTION = 3
        }
    }

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
}
