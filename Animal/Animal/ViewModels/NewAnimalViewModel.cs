using Animal.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Animal

{
    public class NewAnimalViewModel : INotifyPropertyChanged

    {
        public NewAnimalViewModel()
        {
            newCollectionCreationService = new CollectionCreationService();

            SortingTypeStringList = _sorting.SortingTypeEnumList.Select((Sorting.SortingTypeEnum sorttype) => // делаем из листа, созданного из перечилсения ENUM - лист Animal;
            {
                return _sorting.ConvertEnumToString(sorttype);
            }).ToList();

            SortingTypeIndex = -1;
        }

        CollectionCreationService newCollectionCreationService;
        FileControlService fileControl = new FileControlService();
        Sorting _sorting = new Sorting();

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _desiredFileName;
        public string DesiredFileName
        {
            get
            {
                return _desiredFileName;
            }
            set
            {
                _desiredFileName = value;
                fileControl.FullPath = fileControl.GetFilePath(_desiredFileName);
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(GetDataEnabled)); //или "GetDataEnabled" без nameof

            }
        }

        //   private string _lastFileName; // добавлено, чтобы сортировка всё равно работала, даже, если имя файла поменялось

        private string _currentFileName;
        public string CurrentFileName
        {
            get
            {
                return _currentFileName;
            }
            set
            {
                _currentFileName = value;
                RaisePropertyChanged();
            }
        }

        private string _currentSortingLabel;
        public string CurrentSortingLabel
        {
            get
            {
                return _currentSortingLabel;
            }
            set
            {
                _currentSortingLabel = value;
                RaisePropertyChanged();
            }
        }

        public List<string> SortingTypeStringList { get; set; } // подвязан под Picker сортировки

        private int _sortingTypeIndex;
        public int SortingTypeIndex
        {
            get
            {
                return _sortingTypeIndex;
            }

            set
            {
                _sortingTypeIndex = value;
                // SortEnabled = true;
                RaisePropertyChanged();
            }
        }

        private bool IsBusy { get; set; } = false; //свойство создано, чтобы использовать в командах, чтобы они не запукались одновременно несколько раз и не запускались одновременно друг с другом

        public bool GetDataEnabled //свойство создано для отображения активности кнопок через XAML
        {
            get
            {
                return !string.IsNullOrEmpty(DesiredFileName); //не совсем понимаю
            }
        }

        private bool _sortEnabled = false;
        public bool SortEnabled  //свойство создано для отображения активности кнопок через XAML
        {
            get
            {
                return _sortEnabled;
            }
            set
            {
                _sortEnabled = value;
                RaisePropertyChanged();
            }
        }

        private bool _viewListEnabled = false;
        public bool ViewListEnabled //свойство создано для отображения активности кнопок через XAML
        {
            get
            {
                return _viewListEnabled;
            }
            set
            {
                _viewListEnabled = value;
                RaisePropertyChanged();
            }
        }

        List<Animal> NewAnimalCollection { get; set; }

        private ObservableCollection<Animal> _sortedAnimalCollection;
        public ObservableCollection<Animal> SortedAnimalCollection
        {
            get
            {
                return _sortedAnimalCollection;
            }
            set
            {
                _sortedAnimalCollection = value;
                RaisePropertyChanged();
            }
        }

        public Command GetAllDataCommand
        {
            get
            {
                return new Command(async (obj) =>
               {
                   if (IsBusy) //проверка, чтобы процесс не запускался несколько раз до завершения при нажатии на кнопку несколько раз
                   {
                       await DependencyService.Get<IUserInteraction>().ShowMessageAsync("The program is busy with another process");
                       return;
                   }

                   if (string.IsNullOrEmpty(DesiredFileName)) //проверка, чтобы приложение не закрывалось, если какое-то из полей пустое
                   {
                       await DependencyService.Get<IUserInteraction>().ShowMessageAsync("Please fill the name of the file");
                       return;
                   }

                   IsBusy = true;

                   await DependencyService.Get<IUserInteraction>().ShowMessageAsync(String.Format("The full path to the file is:{0}", fileControl.FullPath));
                   fileControl.CheckIfDirectoryExists(fileControl.FullPath);
                   NewAnimalCollection = newCollectionCreationService.CreateAnimalCollection();
                   await fileControl.CreateJsonFile(NewAnimalCollection);
                   CurrentFileName = DesiredFileName;
                   SortEnabled = true;
                   IsBusy = false;

               });
            }
        }

        public Command SortListCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    if (IsBusy) //проверка, чтобы процесс не запускался несколько раз до завершения при нажатии на кнопку несколько раз
                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync("The program is busy with another task");
                        return;
                    }
                    if (SortingTypeIndex == -1 || NewAnimalCollection == null) //проверка, чтобы приложение не закрывалось, если какое-то из полей пустое
                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync("Please fill the name and select the sorting type");
                        return;
                    }

                    if (!File.Exists(fileControl.FullPath))

                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync(String.Format("The file doesn't exist, creating new file"));
                        fileControl.FullPath = fileControl.GetFilePath(CurrentFileName); // добавлено, чтобы сортировка всё равно работала, даже, если имя файла поменялось
                    }


                    if (!File.Exists(fileControl.FullPath))
                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync(String.Format("The filepath \"{0}\" is incorrect, please repeat all steps", fileControl.FullPath));
                        return;
                    }

                    IsBusy = true;

                    CurrentSortingLabel = SortingTypeStringList[_sortingTypeIndex];
                    var deserializedList = await fileControl.DeserializeJsonFile(fileControl.FullPath);
                    SortedAnimalCollection = new ObservableCollection<Animal>(_sorting.SortingSelection(deserializedList, _sorting.SortingTypeEnumList[SortingTypeIndex]));
                    ViewListEnabled = true;
                    IsBusy = false;
                });
            }
        }

        public Command ViewSortedListCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    if (IsBusy) //проверка, чтобы процесс не запускался несколько раз до завершения при нажатии на кнопку несколько раз
                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync("The program is busy with another task");
                        return;
                    }

                    if (SortedAnimalCollection == null)
                    {
                        await DependencyService.Get<IUserInteraction>().ShowMessageAsync("Please choose the sorting and press \"Sort\"");
                        return;
                    }


                    await App.Navigation.PushAsync(new GeneratedListPage(SortedAnimalCollection));
                });
            }
        }

        public Command ClearDataCommand //сбрасываем все поля. Сказали, что создавать новый экземпляр нельзя - необходимо обнулить все значения
        {
            get
            {
                return new Command(() =>
                {
                    DesiredFileName = String.Empty;
                    SortingTypeIndex = -1;
                    CurrentFileName = String.Empty;
                    CurrentSortingLabel = String.Empty;
                    SortedAnimalCollection = null;
                    fileControl.FullPath = String.Empty;
                    IsBusy = false;
                    SortEnabled = false;
                    ViewListEnabled = false;

                    DependencyService.Get<IUserInteraction>().ShowMessageAsync("Everything's clear!");
                });
            }
        }
    }
}



