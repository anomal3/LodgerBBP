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
            string tabName = "��� ��� Plugins";
            string panName = "�������������";
            a.CreateRibbonTab(tabName);

            var panel = a.CreateRibbonPanel(tabName, panName);

            //var addPToolTip = panel.AddItem(RibbonItemData.ToolTip);

            DateTime currentData = DateTime.Now; // ������� ����
            DateTime endWork = new DateTime(2021, 11, 23); // ���� ��������� ������ �������
            TimeSpan workTime = endWork - currentData; // ����� ������ �������

            if (workTime.Days <= 0) //���� ����� ������ ������� �������...
            {
                panel.Enabled = false; //...�� ��������� ������ ������� �������
            }

            #region ��������� ������
            SplitButtonData grpoup1Data = new SplitButtonData("����������", "����������");
            SplitButton gr1 = panel.AddItem(grpoup1Data) as SplitButton;
            #endregion

            #region ������ "������� ������ ���������"
            //�������������� ������ � ����� ������� ������� ����� �����������
            var AddBtnTableRoom = new PushButtonData(
                HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.NAME],
                 HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.TEXT],
                Assembly.GetExecutingAssembly().Location,
                "LodgerBBP.Command");
            //������� �������� ��������� � ������
            AddBtnTableRoom.ToolTip = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.TOOLTIP];
            ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.Url, "http://psk-lik.ru/revit/");
            AddBtnTableRoom.SetContextualHelp(contextHelp);
            //�������� ������ � �������� � � ������
            var TableRoomnBnt = gr1.AddPushButton(AddBtnTableRoom) as PushButton;

            //��������� �������� �� ������ ��� �������
            Image bImgTableRoom = Properties.Resources.s2_96;
            ImageSource imgSrcTableRoom = Helper.Convert(bImgTableRoom, Helper.FormatImageConverter.PNG);
            TableRoomnBnt.LargeImage = imgSrcTableRoom;
            TableRoomnBnt.Image = imgSrcTableRoom;
            //������� ������� ��������� [����� ��������� ���� ������������� �� ������� ������ 3 ������]
            TableRoomnBnt.LongDescription = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.DISCRIPTION];
            //�������� ��������� ��� F1

            #endregion

            #region ������ "������� ��������� ���������"
            //�������������� ������ � ����� ������� ������� ����� �����������
            var AddBtnSelectedRoom = new PushButtonData(
                HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.NAME],
                 HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.TEXT],
                Assembly.GetExecutingAssembly().Location,
                new Document_Selection().ClassName
                );

            //������� �������� ��������� � ������
            AddBtnSelectedRoom.ToolTip = HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.TOOLTIP];
            //�������� ������ � �������� � � ������
            var SelectedRoomnBnt = gr1.AddPushButton(AddBtnSelectedRoom) as PushButton;

            //��������� �������� �� ������ ��� �������
            Image bImgSelectedRoom = Properties.Resources.s2_add_96;
            ImageSource imgSrcSelectedRoom = Helper.Convert(bImgSelectedRoom, Helper.FormatImageConverter.PNG);
            SelectedRoomnBnt.LargeImage = imgSrcSelectedRoom;
            SelectedRoomnBnt.Image = imgSrcSelectedRoom;
            //������� ������� ��������� [����� ��������� ���� ������������� �� ������� ������ 3 ������]
            SelectedRoomnBnt.LongDescription = HelperNaming.SelectedTableRoom[(int)Helper.UINamingArray.DISCRIPTION];

            #endregion

            AddSlideOut(panel);

            //panel.AddSeparator();

            #region ������� � ������ �� ������� ������ ������������ [��� �������!]

            //var AddBtn = new PushButtonData("-----", "������� ����� ���������", Assembly.GetExecutingAssembly().Location, "LodgerBBP.Command");
            //var nBnt = panel.AddItem(AddBtn) as PushButton;

            //Image bImg = Properties.Resources.s2_96;
            //ImageSource imgSrc = Helper.Convert(bImg, Helper.FormatImageConverter.PNG);

            //nBnt.LongDescription = "����� ������� ������ ���������";
            //nBnt.LargeImage = imgSrc;
            //nBnt.Image = imgSrc;
            #endregion

            #region ������ ������

            #endregion



            return Result.Succeeded;
        }

        #region ����� ���������� ���������� ���� � �������� ���� �����
        //TODO : ������� ������ � ����������� ������ ��� ���� ����� ������ �� ����������� �������� � ������ ���������
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

        #region ����� ���������� ��������������� CheckBox'�
        public static void AddToggleCustom(Autodesk.Revit.UI.RibbonPanel panel)
        {
            SplitButtonData group2Data = new SplitButtonData("� ����������", new StackFrame(1).GetMethod().DeclaringType.Name);
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
