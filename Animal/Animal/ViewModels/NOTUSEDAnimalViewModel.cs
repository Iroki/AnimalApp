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
    public class AnimalViewModel : INotifyPropertyChanged

    {
        public AnimalViewModel()
        {
            newCollectionCreationService = new CollectionCreationService();
            _newAnimalCollection = newCollectionCreationService.CreateAnimalCollection();
            SortingTypes = _sorting.SortingTypeEnumList.Select((Sorting.SortingTypeEnum sorttype) => // делаем из листа, созданного из перечилсения ENUM - лист Animal;
            {
                return _sorting.ConvertEnumToString(sorttype);
            }).ToList();

            //The same:
            //SortingTypes = sorting.SortingTypeList.Select(AnonymousMethod).ToList(); альтернативная запись к записи выше. Должна быть использована с методом AnonymousMethod
            SortingTypeIndex = -1; 
            DownloadFileCommand = new Command(async () => await DownloadFile()); //лямбда добавлена, чтобы метод DownloadFile мог быть указан Task, а не void (Command принимает только VOID метод)
            DeserializedResultList = new ObservableCollection<Animal>();
            DeserializedResultList.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //   public string FullPath { get; set; }

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
                fileControl.FullPath = fileControl.GetFilePath(DesiredFileName);
                RaisePropertyChanged();
                // OR PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_desiredFileName));
            }
        }

        private ObservableCollection<Animal> _deserializedResultList;
        public ObservableCollection<Animal> DeserializedResultList
        {
            get
            {
                return _deserializedResultList;
            }
            set
            {
                _deserializedResultList = value;
                RaisePropertyChanged();
            }
        }

        public List<string> SortingTypes { get; set; }

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
                RaisePropertyChanged();
                // OR PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_sortingTypesIndex.ToString()));
            }
        }

        private bool IsBusy { get; set; } = false; //свойство создано, чтобы использовать в командах, чтобы они не запукались одновременно несколько раз и не запускались одновременно друг с другом

        private Sorting _sorting = new Sorting();
        CollectionCreationService newCollectionCreationService;
        List<Animal> _newAnimalCollection;

        FileControlService fileControl = new FileControlService();

        //private string AnonymousMethod(Sorting.SortingType sorttype) //альтернативная запись метода ниже в комбинации с "SortingTypes = ..."
        //{
        //    return sorting.SwitchOnEnum(sorttype);
        //}

        public Command DownloadFileCommand { get; }

        public async Task DownloadFile()
        {
            if (IsBusy) //проверка, чтобы процесс не запускался несколько раз до завершения при нажатии на кнопку несколько раз
                return;

            while (DesiredFileName == String.Empty || SortingTypeIndex == -1) //проверка, чтобы приложение не закрывалось, если какое-то из полей пустое
                return;

            IsBusy = true;
            var sortedList = _sorting.SortingSelection(_newAnimalCollection, _sorting.SortingTypeEnumList[SortingTypeIndex]);
            fileControl.CheckIfDirectoryExists(fileControl.FullPath);
            await fileControl.CreateJsonFile(sortedList);   //await fileControl.CreateJsonFile(createdAnimalCollection.CreateAnimalCollection()); было изначально до перенесения в конструктор;
            IsBusy = false;
        }

        public Command ReturnDeserializedListCommand
        {
            get
            {
                return new Command(async () => // async добавлен, так как иначе команда не будет работать, так как метод DeserializeJsonFile возвращает Task<Animal> 
                {
                    if (IsBusy) //проверка, чтобы процесс не запускался несколько раз до завершения при нажатии на кнопку несколько раз
                        return;

                    if (!File.Exists(fileControl.FullPath))
                        await DownloadFile();  //.ConfigureAwait.False вызывал КРЭШ с ошибкой "Only the original thread that created a view hierarchy can touch its views"

                    while (SortingTypeIndex == -1 || DesiredFileName == String.Empty) //проверка, чтобы приложение не закрывалось, если какое-то из полей пустое
                        return;

                    IsBusy = true;

                    _sorting.SortingSelection(_newAnimalCollection, _sorting.SortingTypeEnumList[SortingTypeIndex]);


                    var deserializedAnimalCollection = new ObservableCollection<Animal>(await fileControl.DeserializeJsonFile(fileControl.FullPath));
                    DeserializedResultList = deserializedAnimalCollection;
                    IsBusy = false;
                    await App.Navigation.PushAsync(new GeneratedListPage(DeserializedResultList));

                });
            }
        }





    }
}
