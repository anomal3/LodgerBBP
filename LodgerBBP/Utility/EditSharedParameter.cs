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
            List<double> AllRooms = new List<double>();

            Element element = null;
            var RoomList = ExtensionHelperListView.RoomTable_.rooms;
            List<Element> ElLocalMinorRoom = new List<Element>();
            foreach (var _RoomValue in RoomList)
            {
                element = Data.ActiveUIDocument.Document.GetElement(_RoomValue.ElementID);

                //  0          1           2               3             4
                //"Жилая", "Не жилая", "Балкон(0.3)", "Лоджия(0.5)", "Терраса (0.3)"
                switch (_RoomValue.SelectedIndex)
                {
                    case 0:
                        dLeaveRoom.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        AllRooms.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        element.get_Parameter((BuiltInParameter)392421).Set(1);                  //Жилое помещения BOOL 1
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //Балкон или лождия BOOL
                        break;
                    case 1:
                        dNoLeaveRoom.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //Балкон или лождия BOOL
                        AllRooms.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        break;
                    case 2:
                        dBalconyRoom.Add(Math.Round(_RoomValue.AREA * 0.3d, Data.ExactAreaCount));
                        element.get_Parameter((BuiltInParameter)392421).Set(1);                  //Балкон или лождия BOOL
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        AllRooms.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        break;
                    case 3:
                        dLodgyRoom.Add(Math.Round(_RoomValue.AREA * 0.5d, Data.ExactAreaCount));
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        AllRooms.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        break;
                    case 4:
                        dTerRoom.Add(Math.Round(_RoomValue.AREA * 0.3d, Data.ExactAreaCount));
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        element.get_Parameter((BuiltInParameter)392421).Set(0);                  //НЕ жилое помещения BOOL 0
                        AllRooms.Add(Math.Round(_RoomValue.AREA, Data.ExactAreaCount));
                        break;
                }

                element.get_Parameter((BuiltInParameter)392431).Set(_IndexRoom);          //Индекс квартиры
                ElLocalMinorRoom.Add(element);
            }

           

            foreach (var _el in ElLocalMinorRoom)
            {
                if(Convert.ToInt32(_el.get_Parameter((BuiltInParameter)292423).AsString()) == Data.MinorNumberRoom.Min())
                {
                    _el.get_Parameter((BuiltInParameter)392423).Set(AllRooms.Sum());               //Все помещения AsInteger
                    AllRoom(AllRooms.Sum());
                    _el.get_Parameter((BuiltInParameter)392425).Set(dLeaveRoom.Union(dNoLeaveRoom).Sum());               //Все помещения без лоджий
                    AllRoomWhithOutBalcony(dLeaveRoom.Union(dNoLeaveRoom).Sum());
                    _el.get_Parameter((BuiltInParameter)392427).Set(dLeaveRoom.Union(dNoLeaveRoom).Union(dBalconyRoom).Union(dLodgyRoom).Union(dTerRoom).Sum());               //Все помещения с коэф
                    AllRoomCoef(dLeaveRoom.Union(dNoLeaveRoom).Union(dBalconyRoom).Union(dLodgyRoom).Union(dTerRoom).Sum());
                    _el.get_Parameter((BuiltInParameter)392429).Set(dLeaveRoom.Sum());               //Жилые помещения
                    LeaveRoom(dLeaveRoom.Sum());
                }
            }

        }

        public static double AllRoom(double dIn)
        {
            return dIn;
        }
        public static double AllRoomWhithOutBalcony(double dIn)
        {
            return dIn;
        }
        public static double AllRoomCoef(double dIn)
        {
            return dIn;
        }
        public static double LeaveRoom(double dIn)
        {
            return dIn;
        }
    }
}
