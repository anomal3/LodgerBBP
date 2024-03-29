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
        protected PushButton SelectedRoomnBnt;
        public Result OnStartup(UIControlledApplication a)
        {
            string tabName = "��� ��� Plugins";
            string panName = "�������������";
            a.CreateRibbonTab(tabName);

            var panel = a.CreateRibbonPanel(tabName, panName);

            //var addPToolTip = panel.AddItem(RibbonItemData.ToolTip);

            DateTime currentData = DateTime.Now; // ������� ����
            DateTime endWork = new DateTime(2022, 11, 23); // ���� ��������� ������ �������
            TimeSpan workTime = endWork - currentData; // ����� ������ �������

            if (workTime.Days <= 0) //���� ����� ������ ������� �������...
            {
                panel.Enabled = false; //...�� ��������� ������ ������� �������
            }

            #region ��������� ������
            SplitButtonData grpoup1Data = new SplitButtonData("����������", "����������");
            SplitButton gr1 = panel.AddItem(grpoup1Data) as SplitButton;
            gr1.AddSeparator();
            #endregion

            panel.AddSeparator();
            #region ������ "����������� �������� ����"
            //�������������� ������ � ����� ������� ������� ����� �����������
            var AddBtnCreateSchedule = new PushButtonData(
                HelperNaming.bScheduleSheets[(int)Helper.UINamingArray.NAME],
                 HelperNaming.bScheduleSheets[(int)Helper.UINamingArray.TEXT],
                Assembly.GetExecutingAssembly().Location,
                new SchelVent().ClassName
                );

            //������� �������� ��������� � ������
            AddBtnCreateSchedule.ToolTip = HelperNaming.bScheduleSheets[(int)Helper.UINamingArray.TOOLTIP];
            //�������� ������ � �������� � � ������
            var SheduleBtn = panel.AddItem(AddBtnCreateSchedule) as PushButton;

            //��������� �������� �� ������ ��� �������
            Image bImgSchedule = Properties.Resources.document_valid;
            ImageSource imgSrcchedule = Helper.Convert(bImgSchedule, Helper.FormatImageConverter.PNG);
            SheduleBtn.LargeImage = imgSrcchedule;
            SheduleBtn.Image = imgSrcchedule;
            //������� ������� ��������� [����� ��������� ���� ������������� �� ������� ������ 3 ������]
            SheduleBtn.LongDescription = HelperNaming.bScheduleSheets[(int)Helper.UINamingArray.DISCRIPTION];

            #endregion
            panel.AddSeparator();

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
            //var SelectedRoomnBnt = gr1.AddPushButton(AddBtnSelectedRoom) as PushButton;
            SelectedRoomnBnt = gr1.AddPushButton(AddBtnSelectedRoom) as PushButton;
            SelectedRoomnBnt.Enabled = false;

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

            #region ������ ����������
            panel.AddSlideOut();
            Image bugRep = Properties.Resources.bug;
            ImageSource _bugRep = Helper.Convert(bugRep, Helper.FormatImageConverter.PNG);

            // �������� ������ ��� ������
            PushButtonData b1 = new PushButtonData(HelperNaming.bBugReport[(int)Helper.UINamingArray.NAME], 
                                                   HelperNaming.bBugReport[(int)Helper.UINamingArray.TEXT], 
                                                   assembly,
                                                   new MarkerStamp().ClassName); //����� ����� ����� ���������� �� �������
            b1.LargeImage = _bugRep;
            b1.ToolTip = HelperNaming.bBugReport[(int)Helper.UINamingArray.TOOLTIP];
            b1.LongDescription = HelperNaming.bBugReport[(int)Helper.UINamingArray.DISCRIPTION];
            #endregion

            #region ������ ��������
            Image procUpdate = Properties.Resources.update;
            ImageSource _procUpdate = Helper.Convert(procUpdate, Helper.FormatImageConverter.PNG);

            // create some controls for the slide out
            PushButtonData bUpdate = new PushButtonData(HelperNaming.bProcessUpdate[(int)Helper.UINamingArray.NAME],
                                                   HelperNaming.bProcessUpdate[(int)Helper.UINamingArray.TEXT],
                                                   assembly,
                                                   "LodgerBBP.SchelVent");
            bUpdate.LargeImage = _procUpdate;
            bUpdate.ToolTip = HelperNaming.bProcessUpdate[(int)Helper.UINamingArray.TOOLTIP];
            bUpdate.LongDescription = HelperNaming.bProcessUpdate[(int)Helper.UINamingArray.DISCRIPTION];
            #endregion

            #region ������ ���������� ����� ����������
            Image imgSharedParameter = Properties.Resources.application_form;
            ImageSource _imgSharedParameter = Helper.Convert(imgSharedParameter, Helper.FormatImageConverter.PNG);

            // create some controls for the slide out
            PushButtonData bSharParameter = new PushButtonData(HelperNaming.bShredParam[(int)Helper.UINamingArray.NAME],
                                                   HelperNaming.bShredParam[(int)Helper.UINamingArray.TEXT],
                                                   assembly,
                                                   new Utility.SharedParameters().ClassName);
            bSharParameter.LargeImage = _imgSharedParameter;
            bSharParameter.ToolTip = HelperNaming.bShredParam[(int)Helper.UINamingArray.TOOLTIP];
            bSharParameter.LongDescription = HelperNaming.bShredParam[(int)Helper.UINamingArray.DISCRIPTION];
            #endregion

            panel.AddItem(b1);
            panel.AddSeparator();
            panel.AddItem(bUpdate);
            panel.AddSeparator();
            panel.AddItem(bSharParameter);
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

        #region ����� ��������� ���������� ������

       
        #endregion

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

    }

    class PushButtonEnable : IExternalCommandAvailability
    {
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            UIDocument uidoc = applicationData.ActiveUIDocument;
            Document famDoc = uidoc.Document;
            // Only enabled this control in the self-adaptive family
            if (famDoc != null && !AdaptiveComponentFamilyUtils.IsAdaptiveComponentFamily(famDoc.OwnerFamily))
            {
                return false;
            }
            return true;
        }
    }
}
