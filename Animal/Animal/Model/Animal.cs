using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal
{
    public class Animal
    {
        public Animal(string name, string species, string petname, int age, int population, string endangered)
        {
            Name = name;
            Species = species;
            PetName = petname;
            Age = age;
            Population = population;
            Endangered = endangered;
        }

        public string Name { get; } // только get, так как мы сетим в конструкторе

        public string Species { get; }

        public string PetName { get; }

        public int Age { get; }

        public int Population { get; }

        public string Endangered { get; }

        public string Description { get => ToString(); } //добавлено, чтобы в ListView можно было обработать текст, так как иначе выводился элемент списка, который является классом

        public override string ToString()
        {
            return $"The animal {Name} is a part of {Species} species, has a petname {PetName}, is of age {Age}. It also has an estimated population of {Population} and is {Endangered}.";
        }
    }

    public class AnimalAttributes
    {
        public List<string> animalNamesList = new List<string> { "Lesser white-fronted goose", "Fruit Bat", "Bengal Tiger", "Black Panther", "Great White Shark", "Blue Whale", "Jaguar", "Gorilla", "Amazon river dolphin", "Bold eagle", "Zebra", "Pony", "African elephant" };
        public List<string> animalSpeciesList = new List<string> { "Wild Cat", "Panther", "Lion", "Bat", "Tiger", "Dolphin", "Whale", "Shark", "Ape", "Goose", "Horse" };
        public List<string> animalPetnameList = new List<string> { "Peanut butter", "Teddy bear biscuit", "Albert Pamplemousse", "Apricot Strudel", "Baron Von Thumper", "Basil Fawlty", "Kransky", "Roxy RottenBottom", "Sir Benedict Cumberbatch", "Toffee Towers", "Tonks the Beautiful Lady", "Uggboot", "Zac Efron" };
        public string[] endangered = { "endangered", "not endangered" };
    }

}

