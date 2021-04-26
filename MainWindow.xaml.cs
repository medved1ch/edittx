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
using System.Windows.Controls.Primitives;
using System.IO;
using Microsoft.Win32;

namespace edit
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 2; i < 74; i++)
                cbFS.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            cbFF.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
        }
        void ApplyPropertyValueToSelectedText(DependencyProperty formattingProperty, object value)
        {
            if (value == null)
                return;
            Workspace.Selection.ApplyPropertyValue(formattingProperty, value);
        }
        private void FontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FontFamily editValue = (FontFamily)e.AddedItems[0];
                ApplyPropertyValueToSelectedText(TextElement.FontFamilyProperty, editValue);
            }
            catch (Exception) { }
        }
        private void FontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplyPropertyValueToSelectedText(TextElement.FontSizeProperty, e.AddedItems[0]);
            }
            catch (Exception) { }
        }
        void UpdateItemCheckedState(ToggleButton button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = Workspace.Selection.GetPropertyValue(formattingProperty);
            button.IsChecked = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }
       
            private void UpdateSelectedFontFamily()
            {
                object value = Workspace.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
                FontFamily currentFontFamily = (FontFamily)((value == DependencyProperty.UnsetValue) ? null : value);
                if (currentFontFamily != null)
                {
                    cbFF.SelectedItem = currentFontFamily;
                }
            }
            private void UpdateSelectedFontSize()
            {
                object value = Workspace.Selection.GetPropertyValue(TextElement.FontSizeProperty);
                cbFS.SelectedValue = (value == DependencyProperty.UnsetValue) ? null : value;
            }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog(); 
            savefile.FileName = "unknown.doc"; 
            savefile.Filter = "Document files (*.doc)|*.doc";
            if (savefile.ShowDialog() == true)
            {
                TextRange t = new TextRange(Workspace.Document.ContentStart, Workspace.Document.ContentEnd);
                FileStream file = new FileStream(savefile.FileName, FileMode.Create);
                t.Save(file, System.Windows.DataFormats.Rtf);
                file.Close();
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Document files (*.doc)|*.doc";
            var result = dlg.ShowDialog();
            if (result.Value)
            {
                TextRange t = new TextRange(Workspace.Document.ContentStart, Workspace.Document.ContentEnd);
                FileStream file = new FileStream(dlg.FileName, FileMode.Open);
                t.Load(file, System.Windows.DataFormats.Rtf);
            }
        }

        private void Workspace_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                object temp = Workspace.Selection.GetPropertyValue(Inline.FontWeightProperty);
                btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
                temp = Workspace.Selection.GetPropertyValue(Inline.FontStyleProperty);
                btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
                temp = Workspace.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
                btnUndr.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

                temp = Workspace.Selection.GetPropertyValue(Inline.FontFamilyProperty);
                cbFF.SelectedItem = temp;
                temp = Workspace.Selection.GetPropertyValue(Inline.FontSizeProperty);
                cbFS.Text = temp.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    }
    

