using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace AreaCalc
{
    class TestApp : IExternalApplication
    {
        
        public Result OnStartup(UIControlledApplication application)
        {
            string assembleName = Assembly.GetExecutingAssembly().Location;
            string tabName = "Plugins";
            string path = System.IO.Path.GetDirectoryName(assembleName);
            //create a tab
            application.CreateRibbonTab(tabName);
            
            //create a panel
            RibbonPanel panel1 = application.CreateRibbonPanel(tabName,"Panel");

            //create button
            PushButtonData buttonData1 = new PushButtonData("ProjectSetting", "ProjectSetting", assembleName, "AreaCalc.Commands.ProjectSettingCommand");
            buttonData1.LargeImage = new BitmapImage(new Uri(path + @"\projectsetting.png"));
            PushButtonData buttonData2 = new PushButtonData("FloorSetting", "FloorSetting", assembleName, "AreaCalc.Commands.FloorSettingCommand");
            buttonData2.LargeImage = new BitmapImage(new Uri(path + @"\floorsetting-32.png"));
            PushButtonData buttonData3 = new PushButtonData("ModelLineToRoomSeparationLine", "ModelLineToRoomSeparationLine", assembleName, "AreaCalc.Commands.ModelLineToRoomSeparationLine");
            buttonData3.LargeImage = new BitmapImage(new Uri(path + @"\roomseparationline.png"));
            PushButtonData buttonData4 = new PushButtonData("ModelLineToAreaBoundary", "ModelLineToAreaBoundary", assembleName, "AreaCalc.Commands.ModelLineToAreaBoundary");
            buttonData4.LargeImage = new BitmapImage(new Uri(path + @"\areaboundaryline.png"));
            //straight skeleton tool
            PushButtonData buttonData5 = new PushButtonData("StraightSkeleton", "StraightSkeleton", assembleName, "AreaCalc.Commands.StraightSkeleton");
            buttonData5.LargeImage = new BitmapImage(new Uri(path + @"\straightskeleton.png"));

            //add tooltips
            buttonData1.ToolTip = "tooltips";

            panel1.AddItem(buttonData1);
            panel1.AddItem(buttonData2);
            panel1.AddItem(buttonData3);
            panel1.AddItem(buttonData4);
            panel1.AddItem(buttonData5);

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
