// Project: MapleSeed-Lite
// File: ThemeManagerHelper.cs
// Updated By: Jared
// 

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using MahApps.Metro;

namespace MapleSeedU.Models
{
    public static class ThemeManagerHelper
    {
        public static void CreateAppStyleBy(Color color, bool changeImmediately = true)
        {
            // create a runtime accent resource dictionary

            var resourceDictionary = new ResourceDictionary
            {
                {"HighlightColor", color},
                {"AccentBaseColor", color},
                {"AccentColor", Color.FromArgb(204, color.R, color.G, color.B)},
                {"AccentColor2", Color.FromArgb(153, color.R, color.G, color.B)},
                {"AccentColor3", Color.FromArgb(102, color.R, color.G, color.B)},
                {"AccentColor4", Color.FromArgb(51, color.R, color.G, color.B)}
            };


            resourceDictionary.Add("HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["HighlightColor"]));
            resourceDictionary.Add("AccentBaseColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentBaseColor"]));
            resourceDictionary.Add("AccentColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("AccentColorBrush2", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("AccentColorBrush3", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("AccentColorBrush4", GetSolidColorBrush((Color)resourceDictionary["AccentColor4"]));
            resourceDictionary.Add("WindowTitleColorBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("ProgressBrush", new LinearGradientBrush(
                new GradientStopCollection(new[]
                {
                    new GradientStop((Color)resourceDictionary["HighlightColor"], 0),
                    new GradientStop((Color)resourceDictionary["AccentColor3"], 1)
                }),
                new Point(0.001, 0.5), new Point(1.002, 0.5)));

            resourceDictionary.Add("CheckmarkFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("RightArrowFill", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));

            resourceDictionary.Add("IdealForegroundColor", IdealTextColor(color));
            resourceDictionary.Add("IdealForegroundColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("IdealForegroundDisabledBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"], 0.4));
            resourceDictionary.Add("AccentSelectedColorBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            resourceDictionary.Add("MetroDataGrid.HighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.HighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));
            resourceDictionary.Add("MetroDataGrid.MouseOverHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor3"]));
            resourceDictionary.Add("MetroDataGrid.FocusBorderBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightBrush", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("MetroDataGrid.InactiveSelectionHighlightTextBrush", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor"]));
            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.OnSwitchMouseOverBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["AccentColor2"]));
            resourceDictionary.Add("MahApps.Metro.Brushes.ToggleSwitchButton.ThumbIndicatorCheckedBrush.Win10", GetSolidColorBrush((Color)resourceDictionary["IdealForegroundColor"]));

            // applying theme to MahApps

            var resDictName = $"ApplicationAccent_{color.ToString().Replace("#", string.Empty)}.xaml";
            var fileName = Path.Combine(Path.GetTempPath(), resDictName);
            using (var writer = System.Xml.XmlWriter.Create(fileName, new System.Xml.XmlWriterSettings { Indent = true }))
            {
                System.Windows.Markup.XamlWriter.Save(resourceDictionary, writer);
                writer.Close();
            }

            resourceDictionary = new ResourceDictionary() { Source = new Uri(fileName, UriKind.Absolute) };

            var newAccent = new Accent { Name = resDictName, Resources = resourceDictionary };
            ThemeManager.AddAccent(newAccent.Name, newAccent.Resources.Source);

            if (changeImmediately)
            {
                var application = Application.Current;
                //var applicationTheme = ThemeManager.AppThemes.First(x => string.Equals(x.Name, "BaseLight"));
                // detect current application theme
                Tuple<AppTheme, Accent> applicationTheme = ThemeManager.DetectAppStyle(application);
                ThemeManager.ChangeAppStyle(application, newAccent, applicationTheme.Item1);
            }
        }

        /// <summary>
        /// Determining Ideal Text Color Based on Specified Background Color
        /// http://www.codeproject.com/KB/GDI-plus/IdealTextColor.aspx
        /// </summary>
        /// <param name = "color">The bg.</param>
        /// <returns></returns>
        private static Color IdealTextColor(Color color)
        {
            const int nThreshold = 105;
            var bgDelta = Convert.ToInt32((color.R * 0.299) + (color.G * 0.587) + (color.B * 0.114));
            var foreColor = (255 - bgDelta < nThreshold) ? Colors.Black : Colors.White;
            return foreColor;
        }

        private static SolidColorBrush GetSolidColorBrush(Color color, double opacity = 1d)
        {
            var brush = new SolidColorBrush(color) { Opacity = opacity };
            brush.Freeze();
            return brush;
        }
    }
}