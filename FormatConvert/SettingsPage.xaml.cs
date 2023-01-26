// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace FormatConvert
{
    public sealed partial class SettingsPage : Page
    {
        // 启用本地设置数据
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public List<string> material { get; } = new List<string>()
        {
            "Mica",
            "Acrylic"
        };
        public SettingsPage()
        {
            this.InitializeComponent();

            // 读取本地设置数据，调整ComboBox状态
            if (localSettings.Values["materialStatus"] as string == "Mica")
            {
                backgroundMaterial.SelectedItem = material[0];
            }
            else if (localSettings.Values["materialStatus"] as string == "Acrylic")
            {
                backgroundMaterial.SelectedItem = material[1];
            }
            else
            {
                // 默认材料为Mica
                localSettings.Values["materialStatus"] = "Mica";
                backgroundMaterial.SelectedItem = material[0];
                // 非法输入，扔出警报
                //throw new Exception($"Wrong material type: {localSettings.Values["materialStatus"]}");
            }
        }
        private void backgroundMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string materialStatus = e.AddedItems[0].ToString();
            switch (materialStatus)
            {
                case "Mica":
                    if (localSettings.Values["materialStatus"] as string != "Mica")
                    {
                        localSettings.Values["materialStatus"] = "Mica";
                        Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
                    }
                    else
                    {
                        localSettings.Values["materialStatus"] = "Mica";
                    }
                    break;
                case "Acrylic":
                    if (localSettings.Values["materialStatus"] as string != "Acrylic")
                    {
                        localSettings.Values["materialStatus"] = "Acrylic";
                        Microsoft.Windows.AppLifecycle.AppInstance.Restart("");
                    }
                    else
                    {
                        localSettings.Values["materialStatus"] = "Acrylic";
                    }
                    break;
                default:
                    throw new Exception($"Invalid argument: {materialStatus}");
            }
        }
    }
}
