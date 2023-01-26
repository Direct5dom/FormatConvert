// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Threading;
using Microsoft.UI;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace FormatConvert
{
    public sealed partial class FormatConvertPage : Page
    {
        public List<string> status { get; } = new List<string>()
        {
            "Keep",
            "Discard"
        };
        public List<string> vcodec { get; } = new List<string>()
        {
            "Copy",
            "AV1"
        };
        public List<string> acodec { get; } = new List<string>()
        {
            "Copy",
            "AC3"
        };

        string VideoStatus = "";
        string AudioStatus = "";
        string VcodecSelect = "";
        string AcodecSelect = "";

        private void refreshCMD()
        {
            cmd.Text = "ffmpeg -i '" + inputPath.Text + "' " + VideoStatus + " " + VcodecSelect + " " + AudioStatus + " " + AcodecSelect + " '" + outputPath.Text + "\\" + outputFileName.Text + "'";
            if ((inputPath.Text != "") && (outputPath.Text != "") && (outputFileName.Text != ""))
            {
                convertButton.IsEnabled = true;
            }
            else
            {
                convertButton.IsEnabled = false;
            }
        }
        public FormatConvertPage()
        {
            this.InitializeComponent();
            videoStatus.SelectedItem = status[0];
            audioStatus.SelectedItem = status[0];
            vcodecChangedX.SelectedItem = vcodec[0];
            acodecChangedX.SelectedItem = acodec[0];
            convertStatus.Text = "";
            refreshCMD();
        }
        public void inputPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            refreshCMD();
        }
        public void outputPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            refreshCMD();
        }
        public void outputFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            refreshCMD();
        }
        public async void inputPathPickerButton_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个FilePicker
            var filePicker = new FileOpenPicker();
            // 通过传入App.m_window对象获取当前窗口的HWND
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            // 将 HWND 与文件选择器相关联
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);
            // 使用文件选择器
            filePicker.FileTypeFilter.Add("*");
            var file = await filePicker.PickSingleFileAsync();
            // 如果file不为空，则将Path填入FileFolderPath.Text
            if (file != null)
            {
                inputPath.Text = file.Path;
                outputFileName.Text = file.Name;
            }
            refreshCMD();
        }
        public async void outputPathPickerButton_Click(object sender, RoutedEventArgs e)
        {
            // 创建一个FolderPicker
            var folderPicker = new FolderPicker();
            // 通过传入App.m_window对象获取当前窗口的HWND
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);
            // 将 HWND 与文件夹选择器相关联
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);
            // 使用文件夹选择器
            var folder = await folderPicker.PickSingleFolderAsync();
            // 如果folder不为空，将Path填入ToWindowsPath.Text
            if (folder != null)
            {
                outputPath.Text = folder.Path;
            }
            refreshCMD();
        }
        public async void cmdCopyButton_Click(object sender, RoutedEventArgs e)
        {
        }
        private void convertButton_Click(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "PowerShell.exe";
            //process.StartInfo.Arguments = "ffmpeg -i '" + inputPath.Text + "' " + VideoStatus + " " + VcodecSelect + " " + AudioStatus + " " + AcodecSelect + " '" + outputPath.Text + "'";
            process.StartInfo.UseShellExecute = false; //是否使用操作系统shell启动
            process.StartInfo.CreateNoWindow = true; //是否在新窗口中启动该进程的值 (不显示程序窗口)
            process.Start();
            process.WaitForExit(); //等待程序执行完退出进程
            process.Close();
        }

        private void videoChanged(object sender, SelectionChangedEventArgs e)
        {
            string videoStatus = e.AddedItems[0].ToString();
            switch (videoStatus)
            {
                case "Keep":
                    VideoStatus = "";
                    vcodecChangedX.IsEnabled = true;
                    break;
                case "Discard":
                    VideoStatus = "-vn";
                    vcodecChangedX.IsEnabled = false;
                    break;
                default:
                    throw new Exception($"Invalid argument: {VideoStatus}");
            }
            refreshCMD();
        }

        private void audioChanged(object sender, SelectionChangedEventArgs e)
        {
            string audioStatus = e.AddedItems[0].ToString();
            switch (audioStatus)
            {
                case "Keep":
                    AudioStatus = "";
                    acodecChangedX.IsEnabled = true;
                    break;
                case "Discard":
                    AudioStatus = "-an";
                    acodecChangedX.IsEnabled = false;
                    break;
                default:
                    throw new Exception($"Invalid argument: {AudioStatus}");
            }
            refreshCMD();
        }

        private void vcodecChanged(object sender, SelectionChangedEventArgs e)
        {
            string vcodecSelect = e.AddedItems[0].ToString();
            switch (vcodecSelect)
            {
                case "Copy":
                    VcodecSelect = "-vcodec copy";
                    break;
                case "AV1":
                    VcodecSelect = "-vcodec av1";
                    break;
                default:
                    throw new Exception($"Invalid argument: {VcodecSelect}");
            }
            refreshCMD();
        }

        private void acodecChanged(object sender, SelectionChangedEventArgs e)
        {
            string acodecSelect = e.AddedItems[0].ToString();
            switch (acodecSelect)
            {
                case "Copy":
                    AcodecSelect = "-acodec copy";
                    break;
                case "AC3":
                    AcodecSelect = "-acodec ac3";
                    break;
                default:
                    throw new Exception($"Invalid argument: {AcodecSelect}");
            }
            refreshCMD();
        }
    }
}
