using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodgerBBP.Utility
{
    public class EditSharedParameter
    {
        public static void Edit_PSKPlugin_Parameter(string _IndexRoom)
        {
            bool isLeaveRoom;
            bool isCoef;

            List<double> dLeaveRoom = new List<double>();
            List<double> dNoLeaveRoom = new List<double>();
            List<double> dBalconyRoom = new List<double>();
            List<double> dLodgyRoom = new List<double>();
            List<double> dTerRoom = new List<double>();


            Element element = null;
            var RoomList = ExtensionHelperListView.RoomTable_.rooms;

            foreach (var _RoomValue in RoomList)
            {
                element = Data.ActiveUIDocument.Document.GetElement(_RoomValue.ElementID);

                //  0          1           2               3             4
                //"Жилая", "Не жилая", "Балкон(0.3)", "Лоджия(0.5)", "Терраса (0.3)"
                switch (_RoomValue.SelectedIndex)
                {
                    case 0:
                        dLeaveRoom.Add(_RoomValue.AREA);
                        break;
                    case 1:
                        dNoLeaveRoom.Add(_RoomValue.AREA);
                        break;
                    case 2:
                        dBalconyRoom.Add(_RoomValue.AREA * 0.3d);
                        break;
                    case 3:
                        dLodgyRoom.Add(_RoomValue.AREA * 0.5d);
                        break;
                    case 4:
                        dTerRoom.Add(_RoomValue.AREA * 0.3d);
                        break;
                }
                TaskDialog.Show("ersa", _RoomValue.SelectedIndex.ToString());

                element.get_Parameter((BuiltInParameter)392431).Set(_IndexRoom);          //Индекс квартиры
                element.get_Parameter((BuiltInParameter)392421).Set(1);                  //Балкон или лождия BOOL
                element.get_Parameter((BuiltInParameter)392421).Set(1);                  //Жилое помещения BOOL
                element.get_Parameter((BuiltInParameter)392423).Set(10.1);               //Все помещения AsInteger
                element.get_Parameter((BuiltInParameter)392425).Set(10.1);               //Все помещения без лоджий
                element.get_Parameter((BuiltInParameter)392427).Set(10.1);               //Все помещения с коэф
                element.get_Parameter((BuiltInParameter)392429).Set(10.1);               //Жилые помещения

            }
           
        }
    }
}
