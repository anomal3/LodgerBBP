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
            string tabName = "��� ��� Plugins";
            string panName = "�������������";
            a.CreateRibbonTab(tabName);
            var panel = a.CreateRibbonPanel(tabName, panName);

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
            //�������� ������ � �������� � � ������
            var TableRoomnBnt = gr1.AddPushButton(AddBtnTableRoom) as PushButton;

            //��������� �������� �� ������ ��� �������
            Image bImgTableRoom = Properties.Resources.s2_96;
            ImageSource imgSrcTableRoom = Helper.Convert(bImgTableRoom, Helper.FormatImageConverter.PNG);
            TableRoomnBnt.LargeImage = imgSrcTableRoom;
            TableRoomnBnt.Image = imgSrcTableRoom;
            //������� ������� ��������� [����� ��������� ���� ������������� �� ������� ������ 3 ������]
            TableRoomnBnt.LongDescription = HelperNaming.TableRoomAdd[(int)Helper.UINamingArray.DISCRIPTION];
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

            panel.AddSeparator();

            SplitButtonData group2Data = new SplitButtonData("� ����������", new StackFrame(1).GetMethod().DeclaringType.Name);
            SplitButton gr2 = panel.AddItem(group2Data) as SplitButton;
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

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

    }
}
