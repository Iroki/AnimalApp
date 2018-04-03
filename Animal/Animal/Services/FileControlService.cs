using Animal.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;



namespace Animal
{
    class FileControlService
    {
        private string _fullPath;
        public string FullPath
        {
            get
            {
                return _fullPath;
            }
            set
            {
                _fullPath = value;
                if (!_fullPath.Contains(".txt"))
                    _fullPath = _fullPath + ".txt";

                if (_fullPath.Contains(".txt.txt"))
                    _fullPath.Substring(0, _fullPath.LastIndexOf('.'));
            }
        }

        public string GetFilePath(string relativePath)
        {
            FullPath = DependencyService.Get<IGetFilePath>().GetFullPath(relativePath); //добавлено, чтобы была возможность использовать путь Андроида return Path.Combine(Xamarin.Forms.Forms.Context.ExternalCacheDir.Path, relativeFilePath); Он реализован в классе GetFilePathDroid.
            return FullPath;                                                    //детали реализации DependencyService по ссылке https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/dependency-service/introduction
        }

        public async Task CreateJsonFile(List<Animal> animalCollection)
        {
            if (File.Exists(FullPath))  // иначе в файле могли остаться строки из предыдущего файла, из-за чего StreamReader выдавал ошибку
                File.Delete(FullPath);
            using (FileStream stream = new FileStream(FullPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(stream, System.Text.Encoding.Default))
                {
                    await Task.Run(() => sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(animalCollection,
                       Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })));
                }
            }
        }

        public async Task<List<Animal>> DeserializeJsonFile(string FullPath)
        {
            //if (!File.Exists(FullPath + ".txt")) //=FullPath.Substring(0,FullPath.LastIndexOf('.')))) //иначе программа не видит файл, поэтому часть пути ".txt" отбрасываем здесь (возможно, файл создаётся изначально неправильно - без ".txt"
            //{

            //    // var newAnimalViewModel = new AnimalViewModel(); //неправильно с точки зрения ИЕРАРХИИ MVVM
            //    //await newAnimalViewModel.DownloadFile().ConfigureAwait(false);  //ПОКА ОШИБКА НА СОЗДАНИИ ФАЙЛА, ТАК КАК В ЭТОЙ ВЕРСИИ ИНДЕКС "-1" ПРИ ПЕРЕХОДЕ ВО VIEWMODEL.

            //}
            using (FileStream stream = new FileStream(FullPath, FileMode.Open))  //FullPath + ".txt"
            {
                using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.Default))
                {
                    var newAnimalCollection = await Task.Run(() => Newtonsoft.Json.JsonConvert.DeserializeObject<List<Animal>>(sr.ReadToEnd())); //конструкция "await Task.Run(() => " добавлена, чтобы работал async
                    return newAnimalCollection;
                }
            }
        }

        public void CheckIfDirectoryExists(string absolutePath)
        {
            if (!Directory.GetParent(absolutePath).Exists)  //проверяет, существует ли путь (папка) к файлу, полный путь к которому указан в absolutePath
            {
                DependencyService.Get<IUserInteraction>().ShowMessageAsync("Directory doesn't exist, creating directory");
                Directory.CreateDirectory(Directory.GetParent(absolutePath).ToString());
            }
        }
    }
}
