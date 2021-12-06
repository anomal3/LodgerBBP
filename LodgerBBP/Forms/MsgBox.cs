using LodgerBBP.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LodgerBBP.Forms
{
    public partial class MsgBox : Form
    {
        public MsgBox(string InMessage, string InTitle, MsgBoxIcon InmsgBoxIcon, MsgBoxButton InmsgBoxButton)
        {
            InitializeComponent();
            Load += (s, a) => { Show(InMessage, InTitle, InmsgBoxIcon, InmsgBoxButton); };
            bOkYes.Click += (s, a) => { this.Close(); /*Просто пока закрываем*/ };
            lTitle.MouseUp   += NFMouseUp;
            lTitle.MouseDown += NFMouseDown;
            lTitle.MouseMove += NFMouseMove;
            MouseUp += NFMouseUp;
            MouseDown += NFMouseDown;
            MouseMove += NFMouseMove;
            panel1.MouseUp += NFMouseUp;
            panel1.MouseDown += NFMouseDown;
            panel1.MouseMove += NFMouseMove;
        }

        bool drag = false;
        System.Drawing.Point startDragPoint;


        #region Перетаскивание формы ввода имени экспликации

        private void NFMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            drag = false;
        }

        private void NFMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Y > 32) return;
            drag = true;
            startDragPoint = new System.Drawing.Point(e.X, e.Y);
        }

        void Drag(System.Windows.Forms.MouseEventArgs e)
        {
            if (drag)
            {
                System.Drawing.Point p1 = new System.Drawing.Point(e.X, e.Y);
                System.Drawing.Point p2 = this.PointToScreen(p1);
                System.Drawing.Point p3 = new System.Drawing.Point(p2.X - this.startDragPoint.X,
                                     p2.Y - this.startDragPoint.Y);
                this.Location = p3;

            }
        }

        private void NFMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Drag(e);
        }
        #endregion

        private void Show(string Message, string Title, MsgBoxIcon msgBoxIcon, MsgBoxButton msgBoxButton)
        {
            lTitle.Text = Title;
            tbMsg.Text = Message;
            cImage.BackgroundImageLayout = ImageLayout.Stretch;
            switch(msgBoxIcon)
            {
                case MsgBoxIcon.Error:
                    cImage.BackgroundImage =  Resources.msgbox_error;
                    break;
                case MsgBoxIcon.Info:
                    cImage.BackgroundImage = Resources.msgbox_info;
                    break;
                case MsgBoxIcon.Warning:
                    cImage.BackgroundImage = Resources.msgbox_warn;
                    break;
                case MsgBoxIcon.Questrion:
                    cImage.BackgroundImage = Resources.msgbox_question;
                    break;
            }

            switch(msgBoxButton)
            {
                case MsgBoxButton.OK:
                    bOkYes.Visible = true;
                    bOkYes.Text = "OK";
                    bOkYes.Location = new Point(418, 189);
                break;
                case MsgBoxButton.YesNo:
                    bOkYes.Visible = true;
                    bNoCancel.Visible = true;
                    bOkYes.Text = "Да";
                    bNoCancel.Text = "Нет";
                    break;
                case MsgBoxButton.YesNoCancel:
                    bOkYes.Visible = true;
                    bNoCancel.Visible = true;
                    bRetry.Visible = true;
                    bOkYes.Text = "Да";
                    bNoCancel.Text = "Нет";
                    bRetry.Text = "Отмена";
                    break;
            }
        }

        public enum MsgBoxIcon {
            Error = 1,
            Info = 2,
            Warning = 3,
            Questrion = 4
        }

        public enum MsgBoxButton
        {
            OK = 1,
            YesNo = 2,
            YesNoCancel =3
                //TODO : Кнопка повтор
        }
    }
}
