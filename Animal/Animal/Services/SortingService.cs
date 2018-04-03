using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal

{
    class Sorting
    {
        public enum SortingTypeEnum { ByName, BySpecies, ByPetname, ByAge, ByPopulation, ByEndangered } // сказали перевести в enum на случай, если придётся подключить другой язык;

        public List<SortingTypeEnum> SortingTypeEnumList { get; } = new List<SortingTypeEnum> { SortingTypeEnum.ByName, SortingTypeEnum.BySpecies, SortingTypeEnum.ByPetname, SortingTypeEnum.ByAge, SortingTypeEnum.ByPopulation, SortingTypeEnum.ByEndangered }; //OR THE SAME: Enum.GetValues(typeof(SortingType)).Cast<SortingType>().ToList();

        public string ConvertEnumToString(SortingTypeEnum sortingType)
        {
            switch (sortingType)
            {
                case SortingTypeEnum.ByName:
                    return "Sort by name";
                case SortingTypeEnum.BySpecies:
                    return "Sort by species";
                case SortingTypeEnum.ByPetname:
                    return "Sort by petname";
                case SortingTypeEnum.ByAge:
                    return "Sort by age";
                case SortingTypeEnum.ByPopulation:
                    return "Sort by population";
                case SortingTypeEnum.ByEndangered:
                    return "Sort by endangered";
                default:
                    return String.Empty;
            }
        }

        public List<Animal> SortingSelection(List<Animal> createdAnimalList, SortingTypeEnum type)
        {
            switch (type)
            {
                case SortingTypeEnum.ByName:
                    return SortByName(createdAnimalList);
                case SortingTypeEnum.BySpecies:
                    return SortBySpecies(createdAnimalList);
                case SortingTypeEnum.ByPetname:
                    return SortByPetname(createdAnimalList);
                case SortingTypeEnum.ByAge:
                    return SortByAge(createdAnimalList);
                case SortingTypeEnum.ByPopulation:
                    return SortByPopulation(createdAnimalList);
                case SortingTypeEnum.ByEndangered:
                    return SortByEndangered(createdAnimalList);
                default:
                    return createdAnimalList;
            }
        }

        public List<Animal> SortByName(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.Name).ToList();
            //var sortedListByName = from animal in createdAnimalList    //Альтернативная запись.
            //                       orderby animal.Name 
            //                       select animal;  
            //return sortedListByName.ToList();
            //Также, если не указать ToList(), коллекция создаётся на этапе компиляции!!! При указании ToList() коллекция создаётся немедленно;
        }

        public List<Animal> SortBySpecies(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.Species).ToList();
        }

        public List<Animal> SortByPetname(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.PetName).ToList();
        }

        public List<Animal> SortByAge(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.Age).ToList();
        }

        public List<Animal> SortByPopulation(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.Population).ToList();
        }

        public List<Animal> SortByEndangered(List<Animal> createdAnimalList)
        {
            return createdAnimalList.OrderBy(animal => animal.Endangered).ToList();
        }

        // метод, переводящий ЕНАМ в стринг (сразу лист сказали не делать?)
    }
}
