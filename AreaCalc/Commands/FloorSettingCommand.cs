using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AreaCalc.ModelObjects;
using AreaCalc.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Newtonsoft.Json;

namespace AreaCalc.Commands
{
    [Transaction(TransactionMode.Manual)]
    class FloorSettingCommand : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;
            string jsonPath = activeUIDocument.Document.PathName.Replace(".rvt", ".json");
            if (activeUIDocument == null)
            {
                MessageBox.Show("No active Document.");
                return Result.Failed;
            }
            if (string.IsNullOrEmpty(activeUIDocument.Document.PathName))
            {
                MessageBox.Show("Save Project First！", "Warning");
                activeUIDocument.SaveAs();
                return Result.Failed;
            }
            //判断是否进行项目设置
            if (File.Exists(jsonPath))
            {
                ProjectSettings projectSettings1 = JsonConvert.DeserializeObject<ProjectSettings>(File.ReadAllText(jsonPath));
                if (projectSettings1.BuildingType == null)
                {
                    MessageBox.Show("Please finish Project Setting First！", "提醒");
                    return Result.Failed;
                }
            }
            else
            {
                MessageBox.Show("Please finish Project Setting First！", "提醒");
                return Result.Failed;
            }
            
            //读取楼层信息数据文件
            FloorSettingWindow floorSettingWindow = new FloorSettingWindow(activeUIDocument);
            floorSettingWindow.Show();
            return Result.Succeeded;
        }

    }
}
