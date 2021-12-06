#define PRAGMA_NAME_ROMAN   //Директивы для понимания имени откуда брать файл общих параметров
//#define PRAGMA_NAME_ALEX  //
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using LodgerBBP.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;



namespace LodgerBBP
{
    internal class CreateSharedParameter
    {
        public void CreateShared_Parameter(Document doc, Application app)
        {
            Category category = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rooms); //К какой категории привяжем параметр
            CategorySet categorySet = app.Create.NewCategorySet();
            categorySet.Insert(category);

            string originalFile = app.SharedParametersFilename;
            //string tempFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\StrParameters.txt";

#if PRAGMA_NAME_ROMAN
            string tempFile = @"X:\BIM-Хранилище\PSKPlugins\StrParameters.txt";
#elif PRAGMA_NAME_ALEX
            string tempFile = @"PATH_TO\StrParameters.txt";
#endif

            try
            {
                app.SharedParametersFilename = tempFile;
                DefinitionFile sharedParameterFile = null;
                if (File.Exists(tempFile))
                {
                    sharedParameterFile = app.OpenSharedParameterFile();
                }
                else {
#if DEBUG
                    new MsgBox("Ошибка имени в классе CreateSharedParameter.\r\nВидимо стоит не то имя. Раскоментируйте #define своего имени а второе заблокируйте", 
                        "Ошибка общих параметров", MsgBox.MsgBoxIcon.Warning, MsgBox.MsgBoxButton.OK).Show();
#elif TRACE
                    new MsgBox("У Вас не подключен диск Х. Подключите его и попробуйте снова.", "Ошибка общих параметров", MsgBox.MsgBoxIcon.Warning, MsgBox.MsgBoxButton.OK).Show();
#endif

                    return; }

                foreach(var dg in sharedParameterFile.Groups)
                {
                    if(dg.Name == "PSKPlugins_ЛиК_АР")
                    {
                        ExternalDefinition balcony = dg.Definitions.get_Item("Балкон или лоджия") as ExternalDefinition;
                        ExternalDefinition boolLeaveRoom = dg.Definitions.get_Item("Жилое помещение") as ExternalDefinition;
                        ExternalDefinition AllRooms = dg.Definitions.get_Item("Все помещения") as ExternalDefinition;
                        ExternalDefinition AllRoomsWhithOut = dg.Definitions.get_Item("Все помещения без лоджий и балконов") as ExternalDefinition;
                        ExternalDefinition AllRoomsCoef = dg.Definitions.get_Item("Все помещения с коэф") as ExternalDefinition;
                        ExternalDefinition AllRoomsLeave = dg.Definitions.get_Item("Жилая площадь") as ExternalDefinition;
                        ExternalDefinition RoomsIndex = dg.Definitions.get_Item("Индекс квартиры") as ExternalDefinition;


                        using (Transaction t= new Transaction(doc))
                        {
                            t.Start("Add shared parameter");
                            InstanceBinding newIB = app.Create.NewInstanceBinding(categorySet);

                            doc.ParameterBindings.Insert(balcony, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(boolLeaveRoom, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(AllRooms, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(AllRoomsWhithOut, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(AllRoomsCoef, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(AllRoomsLeave, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            doc.ParameterBindings.Insert(RoomsIndex, newIB, BuiltInParameterGroup.PG_TEXT); // к какому параметру свойств втавим
                            //doc.ParameterBindings.Remove() //Удаляет параметр
                            t.Commit();
                        }
                    }
                }
            }
            catch { }
            finally
            {
                //сбросим оригинальный файл
                app.SharedParametersFilename = originalFile;
            }
        }

        //NOTRUN : Не протестированно
        public void RemoveShared_Parameter(Document doc, Application app)
        {
            Category category = doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rooms); //К какой категории привяжем параметр
            CategorySet categorySet = app.Create.NewCategorySet();
            categorySet.Insert(category);

            string originalFile = app.SharedParametersFilename;
            string tempFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\StrParameters.txt";
            MessageBox.Show(tempFile);
            try
            {
                app.SharedParametersFilename = tempFile;
                DefinitionFile sharedParameterFile = app.OpenSharedParameterFile();

                foreach (var dg in sharedParameterFile.Groups)
                {
                    if (dg.Name == "PSKPlugins_ЛиК_АР")
                    {
                        ExternalDefinition balcony = dg.Definitions.get_Item("Балкон или лоджия") as ExternalDefinition;
                        ExternalDefinition AllRooms = dg.Definitions.get_Item("Все помещения") as ExternalDefinition;
                        ExternalDefinition AllRoomsWhithOut = dg.Definitions.get_Item("Все помещения без лоджий и балконов") as ExternalDefinition;
                        ExternalDefinition AllRoomsCoef = dg.Definitions.get_Item("Все помещения с коэф") as ExternalDefinition;
                        ExternalDefinition AllRoomsLeave = dg.Definitions.get_Item("Жилая площадь") as ExternalDefinition;
                        ExternalDefinition RoomsIndex = dg.Definitions.get_Item("Индекс квартиры") as ExternalDefinition;

                        using (Transaction t = new Transaction(doc))
                        {
                            t.Start("Add shared parameter");
                            InstanceBinding newIB = app.Create.NewInstanceBinding(categorySet);

                            doc.ParameterBindings.Remove(balcony); 
                            doc.ParameterBindings.Remove(AllRooms); 
                            doc.ParameterBindings.Remove(AllRoomsWhithOut); 
                            doc.ParameterBindings.Remove(AllRoomsCoef); 
                            doc.ParameterBindings.Remove(AllRoomsLeave);
                            doc.ParameterBindings.Remove(RoomsIndex); 
                            t.Commit();
                        }
                    }
                }
            }
            catch { }
            finally
            {
                //сбросим оригинальный файл
                app.SharedParametersFilename = originalFile;
            }
        }
    }
}
