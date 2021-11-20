#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Media;

#endregion

namespace LodgerBBP
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            string tabName = "ПСК ЛИК Plugins";
            string panName = "Архитектурный";
            a.CreateRibbonTab(tabName);
            var panel = a.CreateRibbonPanel(tabName, panName);

            #region Сдвоенная кнопка
            SplitButtonData grpoup1Data = new SplitButtonData("Квартирник", "Квартирник");
            SplitButton gr1 = panel.AddItem(grpoup1Data) as SplitButton;
            #endregion

            #region Кнопка "Таблица выбора помещений"
            //Инициализируем кнопку и задаём команду которая будет выполняться
            var AddBtnTableRoom = new PushButtonData(
                HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.NAME],
                 HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.TEXT], 
                Assembly.GetExecutingAssembly().Location, 
                "LodgerBBP.Command");
            //Выводим короткую подсказку о кнопке
            AddBtnTableRoom.ToolTip = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.TOOLTIP];
            //Обявляем кнопку и помещаем её в группу
            var TableRoomnBnt = gr1.AddPushButton(AddBtnTableRoom) as PushButton;

            //Добавляем картинку на кнопку для визуала
            Image bImgTableRoom = Properties.Resources.s2_96;
            ImageSource imgSrcTableRoom = Helper.Convert(bImgTableRoom, Helper.FormatImageConverter.PNG);
            TableRoomnBnt.LargeImage = imgSrcTableRoom;
            TableRoomnBnt.Image = imgSrcTableRoom;
            //Выводим длинную подсказку [Когда указатель мыши задерживается на объекте больше 3 секунд]
            TableRoomnBnt.LongDescription = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.DISCRIPTION];
            #endregion

            #region Кнопка "Таблица выбранных помещений"
            //Инициализируем кнопку и задаём команду которая будет выполняться
            var AddBtnSelectedRoom = new PushButtonData(
                HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.NAME],
                 HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.TEXT],
                Assembly.GetExecutingAssembly().Location,
                new Document_Selection().ClassName
                );

            //Выводим короткую подсказку о кнопке
            AddBtnSelectedRoom.ToolTip = HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.TOOLTIP];
            //Обявляем кнопку и помещаем её в группу
            var SelectedRoomnBnt = gr1.AddPushButton(AddBtnSelectedRoom) as PushButton;
            
            //Добавляем картинку на кнопку для визуала
            Image bImgSelectedRoom = Properties.Resources.s2_add_96;
            ImageSource imgSrcSelectedRoom = Helper.Convert(bImgSelectedRoom, Helper.FormatImageConverter.PNG);
            SelectedRoomnBnt.LargeImage = imgSrcSelectedRoom;
            SelectedRoomnBnt.Image = imgSrcSelectedRoom;
            //Выводим длинную подсказку [Когда указатель мыши задерживается на объекте больше 3 секунд]
            SelectedRoomnBnt.LongDescription = HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.DISCRIPTION];
            
            #endregion

            panel.AddSeparator();

            SplitButtonData group2Data = new SplitButtonData("В разработке", new StackFrame(1).GetMethod().DeclaringType.Name);
            SplitButton gr2 = panel.AddItem(group2Data) as SplitButton;
            #region Вкладка и кнопка на врехней панеле инструментов [Как образец!]

            //var AddBtn = new PushButtonData("-----", "Таблица выбра помещений", Assembly.GetExecutingAssembly().Location, "LodgerBBP.Command");
            //var nBnt = panel.AddItem(AddBtn) as PushButton;

            //Image bImg = Properties.Resources.s2_96;
            //ImageSource imgSrc = Helper.Convert(bImg, Helper.FormatImageConverter.PNG);

            //nBnt.LongDescription = "Вызов таблицы выбора помещений";
            //nBnt.LargeImage = imgSrc;
            //nBnt.Image = imgSrc;
            #endregion


            #region Кнопка выбора
           
            #endregion

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

    }
}
