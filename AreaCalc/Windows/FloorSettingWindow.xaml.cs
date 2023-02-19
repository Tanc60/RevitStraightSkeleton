using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AreaCalc.Windows
{
    /// <summary>
    /// Interaction logic for FloorSettingWindow.xaml
    /// </summary>
    public partial class FloorSettingWindow : Window
    {
        public FloorSettingWindow(UIDocument uidoc)
        {
            InitializeComponent();
           
            DataGrid1.ItemsSource = CollectLevel(uidoc.Document);
        }
        private static List<Level> CollectLevel(Document doc)
        {
            List<Level> list = (from u in new FilteredElementCollector(doc).WhereElementIsNotElementType().OfType<Level>().ToList<Level>()
                                where u.get_Parameter(BuiltInParameter.LEVEL_IS_BUILDING_STORY).AsInteger() == 1
                                select u).ToList<Level>();
            list.Sort((Level a, Level b) => a.Elevation.CompareTo(b.Elevation));
            return list;
        }
    }
}
