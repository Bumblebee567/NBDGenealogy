using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NBDGenealogy.ViewModels
{
    public class AddPersonViewModel : Screen
    {
        private string _name;
        private DateTime _birthDate;
        private DateTime _deathDate;
        private Gender _gender;
        private PersonModel _father;
        private PersonModel _mother;
        private List<PersonModel> _children;
        private ObservableCollection<PersonModel> _possibleFathers;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; }
        }
        public DateTime DeathDate
        {
            get { return _deathDate; }
            set { _deathDate = value; }
        }
        public Gender Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public PersonModel Father
        {
            get { return _father; }
            set { _father = value; }
        }
        public PersonModel Mother
        {
            get { return _mother; }
            set { _mother = value; }
        }
        public List<PersonModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        public ObservableCollection<PersonModel> PossibleFathers
        {
            get { return AllPossibleFathers(); }
            set { _possibleFathers = value; }
        }

        public ObservableCollection<PersonModel> AllPossibleFathers()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            PersonModel exPerson = new PersonModel
            {
                Name = "Janusz Kowalski",
                Gender = Gender.Male
            };
            db.Store(exPerson);
            ObservableCollection<PersonModel> PossibleFathers = new ObservableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(Gender.Male));
            foreach (var man in allMenInDatabase)
            {
                PossibleFathers.Add((PersonModel)man);
            }
            db.Close();
            return PossibleFathers;
        }

    }
}
