using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AreaCalc.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class ProjectSettingCommand : IExternalCommand
    {
        
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument activeUIDocument = commandData.Application.ActiveUIDocument;

            //检查是否存在活动窗口。
            if (activeUIDocument == null)
            {
                MessageBox.Show("请在一个活动项目视图下进行操作该命令");
                return Result.Failed;
            }

            Document document = activeUIDocument.Document;

            //检查文件是否已保存。
            if (string.IsNullOrEmpty(activeUIDocument.Document.PathName))
            {
                MessageBox.Show("Please save your document first.","Warning");
                activeUIDocument.SaveAs();
                return Result.Failed;
            }
            var v = document.ActiveView;
            var vName = v.Name;

            ProjectSettingWindow window = new ProjectSettingWindow(activeUIDocument);

            window.label_Name1.Content =$"Name:{vName}";
            
            window.Show();
            return Result.Succeeded;
		}
    }
}
