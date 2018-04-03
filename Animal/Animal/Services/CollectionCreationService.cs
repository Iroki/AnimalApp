using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal
{
    public class CollectionCreationService : AnimalAttributes
    {
        Random random = new Random();

        public CollectionCreationService()
        {

        }

        public List<Animal> CreateAnimalCollection()
        {
            var animalCollection = new List<Animal>();

            for (int i = 0; i < 15; i++)
            {
                animalCollection.Add(new Animal
                (
                    animalNamesList[random.Next(animalNamesList.Count)],
                    animalSpeciesList[random.Next(animalSpeciesList.Count)],
                    animalPetnameList[random.Next(animalPetnameList.Count)],
                    random.Next(1, 50),
                    random.Next(100, 100000),
                    endangered[random.Next(0, endangered.Length)]
                ));
            }
            return animalCollection;
        }
    }
}
