using System.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using AreaCalc.Tools;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AreaCalc.ModelObjects;

namespace AreaCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjectSettingWindow : Window
    {
        private readonly Document m_Doc;

        private List<string> _listBuildingTypes = new List<string>();

        private readonly string _configPath;
        private string _docPath;
        private string _docDir;
        private string _docNameWithoutExtension;
        private string jsonPath;
        public ProjectSettingWindow(UIDocument uidoc)
        {
            InitializeComponent();
            m_Doc = uidoc.Document;
            string assembleName = Assembly.GetExecutingAssembly().Location;
            _configPath = System.IO.Path.GetDirectoryName(assembleName) + "/AreaCalc_Config.ini";
            _docPath = m_Doc.PathName;
            _docDir = Path.GetDirectoryName(_docPath);
            _docNameWithoutExtension = Path.GetFileNameWithoutExtension(_docPath);
            jsonPath = _docDir + "/" + _docNameWithoutExtension + ".json";
        }

        private void ButtonOKClick(object sender, RoutedEventArgs e)
        {
            label_Name1.Content = m_Doc.PathName;
            SaveProjectSetting();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReadModelBuildingTypes();
            LoadProjectSetting();
        }
        /// <summary>
        /// 读取项目模型设置
        /// </summary>
        private void ReadModelBuildingTypes()
        {
            foreach (string item2 in IniTool.Read("ModelBuildingTypes", "BuildingType", "", this._configPath).Split(','))
            {
                _listBuildingTypes.Add(item2);
            }
            ComboBox1.ItemsSource = _listBuildingTypes;
        }
        
        private void SaveProjectSetting()
        {

            //读取window结果
            ProjectSettings projectSettings = new ProjectSettings
            {
                BuildingType = ComboBox1.Text
            };
           //写入数据
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(projectSettings));
            MessageBox.Show("项目设置保存成功!", "项目设置",MessageBoxButton.OK);
            Close();
        }
        private void LoadProjectSetting()
        {
            
            //读取json文件
            if (File.Exists(jsonPath))
            {
                ProjectSettings projectSettings1 = JsonConvert.DeserializeObject<ProjectSettings>(File.ReadAllText(jsonPath));
                    //判断是否为空
                if (projectSettings1 != null)
                {
                    ComboBox1.Text = projectSettings1.BuildingType;
                }
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
