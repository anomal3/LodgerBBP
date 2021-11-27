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
using System.Windows.Media.Imaging;

#endregion

namespace LodgerBBP
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            string tabName = "ПСК ЛИК Plugins";
            string panName = "Дополнительно";
            a.CreateRibbonTab(tabName);

            var panel = a.CreateRibbonPanel(tabName, panName);

            //var addPToolTip = panel.AddItem(RibbonItemData.ToolTip);

            DateTime currentData = DateTime.Now; // Текущая дата
            DateTime endWork = new DateTime(2021, 11, 23); // Дата окончания работы плагина
            TimeSpan workTime = endWork - currentData; // Время работы плагина

            if (workTime.Days <= 0) //Если время работы плагина истекло...
            {
                panel.Enabled = false; //...то отключаем кнопку запуска плагина
            }

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
            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url, "http://psk-lik.ru/revit/");
            AddBtnTableRoom.SetContextualHelp(contextHelp);
            //Обявляем кнопку и помещаем её в группу
            var TableRoomnBnt = gr1.AddPushButton(AddBtnTableRoom) as PushButton;

            //Добавляем картинку на кнопку для визуала
            Image bImgTableRoom = Properties.Resources.s2_96;
            ImageSource imgSrcTableRoom = Helper.Convert(bImgTableRoom, Helper.FormatImageConverter.PNG);
            TableRoomnBnt.LargeImage = imgSrcTableRoom;
            TableRoomnBnt.Image = imgSrcTableRoom;
            //Выводим длинную подсказку [Когда указатель мыши задерживается на объекте больше 3 секунд]
            TableRoomnBnt.LongDescription = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.DISCRIPTION];
            //Создадим подсказку для F1

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

            AddSlideOut(panel);

            //panel.AddSeparator();

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

        #region Метод добавления выдвижного меню с кнопками ниже ленты
        //TODO : сделать классы и проработать логику для двух новых кнопок не относящихся напрямую к логике программы
        private static void AddSlideOut(Autodesk.Revit.UI.RibbonPanel panel)
        {
            string assembly = Assembly.GetExecutingAssembly().Location;

            panel.AddSlideOut();
            Image bugRep = Properties.Resources.bug;
            ImageSource _bugRep = Helper.Convert(bugRep, Helper.FormatImageConverter.PNG);

            // create some controls for the slide out
            PushButtonData b1 = new PushButtonData(HelperNaming.bBugReport[0], 
                                                   HelperNaming.bBugReport[1], 
                                                   assembly, 
                                                   "Hello.HelloButton");
            b1.LargeImage = _bugRep;
            b1.ToolTip = HelperNaming.bBugReport[2];
            b1.LongDescription = HelperNaming.bBugReport[3];

            Image procUpdate = Properties.Resources.update;
            ImageSource _procUpdate = Helper.Convert(procUpdate, Helper.FormatImageConverter.PNG);

            // create some controls for the slide out
            PushButtonData bUpdate = new PushButtonData(HelperNaming.bProcessUpdate[0],
                                                   HelperNaming.bProcessUpdate[1],
                                                   assembly,
                                                   "Hello.HelloButton");
            bUpdate.LargeImage = _procUpdate;
            bUpdate.ToolTip = HelperNaming.bProcessUpdate[2];
            bUpdate.LongDescription = HelperNaming.bProcessUpdate[3];


            panel.AddItem(b1);
            panel.AddSeparator();
            panel.AddItem(bUpdate);
        }
        #endregion

        #region Метод добавления своегообразного CheckBox'а
        public static void AddToggleCustom(Autodesk.Revit.UI.RibbonPanel panel)
        {
            SplitButtonData group2Data = new SplitButtonData("В разработке", new StackFrame(1).GetMethod().DeclaringType.Name);
            SplitButton gr2 = panel.AddItem(group2Data) as SplitButton;

            RadioButtonGroupData radioData = new RadioButtonGroupData("radioGroup");

            RadioButtonGroup radioButtonGroup = panel.AddItem(radioData) as RadioButtonGroup;

            ToggleButtonData tb1 = new ToggleButtonData("OverrideCommand1", "Override Cmd: Off", Assembly.GetExecutingAssembly().Location + "\\" + Assembly.GetExecutingAssembly().Location, "SnapshotRevitUI_CS.OverrideOff");

            ToggleButtonData tb2 = new ToggleButtonData("OverrideCommand2", "Override Cmd: On", Assembly.GetExecutingAssembly().Location + "\\" + Assembly.GetExecutingAssembly().Location, "SnapshotRevitUI_CS.OverrideOn");

            tb2.ToolTip = "Override the Wall Creation command";

            tb2.LargeImage = new BitmapImage(new Uri(@"D:\abacus32.png"));

            radioButtonGroup.AddItem(tb1);
            radioButtonGroup.AddItem(tb2);
        }
        #endregion

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

    }
}
