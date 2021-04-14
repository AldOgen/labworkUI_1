using ClassLibrary;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace labworkUI_1 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private V2MainCollection MainCollection;

        public MainWindow() {
            MainCollection = new V2MainCollection();
            InitializeComponent();
            Resources["MainCollection"] = MainCollection;
        }

        #region Clicks
        private void NewClick(object sender, RoutedEventArgs e) {
            if (MainCollection.IsSave || SaveDialog()) {
                MainCollection = new V2MainCollection();
                Resources["MainCollection"] = MainCollection;
            }
        }

        private void OpenClick(object sender, RoutedEventArgs e) {
            if (MainCollection.IsSave || SaveDialog()) {
                OpenFileDialog OpenDialog = new OpenFileDialog {
                    Filter = "Binary data|*.dat|All|*.*",
                    FilterIndex = 1
                };

                if (OpenDialog.ShowDialog() == true) {
                    MainCollection = new V2MainCollection();
                    MainCollection.Load(OpenDialog.FileName);
                    Resources["MainCollection"] = MainCollection;
                }
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e) {
            SaveFileDialog SaveDialog = new SaveFileDialog {
                Filter = "Binary data|*.dat|All|*.*",
                FilterIndex = 1
            };

            if (SaveDialog.ShowDialog() == true) {
                MainCollection.Save(SaveDialog.FileName);
                MainCollection.IsSave = true;
            }
        }

        private void AddDefaultsClick(object sender, RoutedEventArgs e) {
            MainCollection.AddDefaults();
            MainCollection.IsSave = false;
        }

        private void AddDefaultV2DataCollectionClick(object sender, RoutedEventArgs e) {
            V2DataCollection data_collection = new V2DataCollection(0.0, "Default info");
            data_collection.InitRandom(3, 10.0f, 10.0f, -10.0f, 10.0f);
            MainCollection.Add(data_collection);
            MainCollection.IsSave = false;
        }

        private void AddDefaultV2DataOnGridClick(object sender, RoutedEventArgs e) {
            V2DataOnGrid data_on_grid = new V2DataOnGrid(0.0, "Default info", new double[] { 0.01, 0.01 }, new int[] { 3, 3 });
            data_on_grid.InitRandom(-10.0f, 10.0f);
            MainCollection.Add(data_on_grid);
            MainCollection.IsSave = false;
        }

        private void AddElementFromFileClick(object sender, RoutedEventArgs e) {
            OpenFileDialog OpenDialog = new OpenFileDialog {
                Filter = "All|*.*",
                FilterIndex = 0
            };

            if (OpenDialog.ShowDialog() == true) {
                V2DataCollection data_collection = new V2DataCollection(OpenDialog.FileName);
                MainCollection.Add(data_collection);
                MainCollection.IsSave = false;
            }
        }

        private void RemoveClick(object sender, RoutedEventArgs e) {
            if ((sender as MenuItem).DataContext != null) {
                var dc = (V2Data)(sender as MenuItem).DataContext;
                var id = dc.Description;
                var w = dc.Freq_field;
                MainCollection.Remove(id, w);
                MainCollection.IsSave = false;
            }
        }
        #endregion

        #region Filters
        private void FilterDataCollection(object sender, FilterEventArgs args) {
            args.Accepted = args.Item is V2DataCollection;
        }

        private void FilterDataOnGrid(object sender, FilterEventArgs args) {
            args.Accepted = args.Item is V2DataOnGrid;
        }
        #endregion

        #region Utility
        private bool SaveDialog() {
            var MessageSave = MessageBox.Show("Сохранить текущий экземпляр?", "AO", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            switch (MessageSave) {
                case MessageBoxResult.Yes:
                    SaveFileDialog SaveDialog = new SaveFileDialog {
                        Filter = "Binary data|*.dat|All|*.*",
                        FilterIndex = 1
                    };

                    if (SaveDialog.ShowDialog() == true) {
                        MainCollection.Save(SaveDialog.FileName);
                        MainCollection.IsSave = true;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    return false;
            }
            return true;
        }
        #endregion
    }
}
