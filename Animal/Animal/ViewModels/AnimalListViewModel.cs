using Animal.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Xamarin.Forms;

namespace Animal
{
	public class AnimalListViewModel : INotifyPropertyChanged
	{
		public AnimalListViewModel ()
		{
			
		}

        public AnimalListViewModel(ObservableCollection<Animal> sortedAnimalCollection)
        {
            SortedAnimalCollection = new ObservableCollection<Animal>(); //??
            SortedAnimalCollection.Clear();                              //??
            SortedAnimalCollection = sortedAnimalCollection;
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Animal SelectedListItem { get; set; }

        public Command RemoveSelectedItemCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    if (SelectedListItem == null)
                        return;
                    SortedAnimalCollection.Remove(SelectedListItem);
                    DependencyService.Get<IUserInteraction>().ShowMessageAsync("Entry removed!");
                });
            }
        }

        public Command ClearListAndReturn 
        {
            get
            {
                return new Command((obj) =>
                {
                    SelectedListItem = null;
                    SortedAnimalCollection.Clear();
                    DependencyService.Get<IUserInteraction>().ShowMessageAsync("List cleared!");
                    App.Navigation.PopAsync(true) ; //true = animated
                });
            }
        }

    }
}