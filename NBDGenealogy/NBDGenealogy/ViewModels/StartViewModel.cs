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
    public class StartViewModel : Screen
    {
        private string _name;
        private DateTime _birthDate = DateTime.MinValue;
        private DateTime _deathDate = DateTime.MinValue;
        private EGender _gender;
        private PersonModel _father;
        private PersonModel _mother;
        private List<PersonModel> _children;
        private ObservableCollection<PersonModel> _possibleFathers;
        private ObservableCollection<PersonModel> _possibleMothers;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set
            {
                _birthDate = value;
                NotifyOfPropertyChange(() => BirthDate);
            }
        }
        public DateTime DeathDate
        {
            get { return _deathDate; }
            set
            {
                _deathDate = value;
                NotifyOfPropertyChange(() => DeathDate);
            }
        }
        public EGender Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                NotifyOfPropertyChange(() => Gender);
            }
        }
        public PersonModel Father
        {
            get { return _father; }
            set
            {
                _father = value;
                NotifyOfPropertyChange(() => Father);
            }
        }
        public PersonModel Mother
        {
            get { return _mother; }
            set
            {
                _mother = value;
                NotifyOfPropertyChange(() => Mother);
            }
        }
        public List<PersonModel> Children
        {
            get { return _children; }
            set { _children = value; }
        }
        public ObservableCollection<PersonModel> PossibleFathers
        {
            get { return AllPossibleFathers(); }
            set
            {
                _possibleFathers = value;
                NotifyOfPropertyChange(() => PossibleFathers);
            }
        }
        public ObservableCollection<PersonModel> PossibleMothers
        {
            get { return AllPossibleMothers(); }
            set
            {
                _possibleMothers = value;
                NotifyOfPropertyChange(() => PossibleMothers);
            }
        }
        public IList<EGender> GenderTypes
        {
            get
            {
                return Enum.GetValues(typeof(EGender)).Cast<EGender>().ToList<EGender>();
            }
        }

        public ObservableCollection<PersonModel> AllPossibleFathers()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> PossibleFathers = new ObservableCollection<PersonModel>();
            var allMenInDatabase = db.QueryByExample(new PersonModel(EGender.Male));
            foreach (var man in allMenInDatabase)
            {
                PossibleFathers.Add((PersonModel)man);
            }
            db.Close();
            var wrongImported = PossibleFathers.Select(x => x.Gender == EGender.Female).ToArray();
            for (int i = 0; i < wrongImported.Count(); i++)
            {
                if(wrongImported[i] == true)
                {
                    PossibleFathers.RemoveAt(i);
                }
            }
            return PossibleFathers;
        }
        public ObservableCollection<PersonModel> AllPossibleMothers()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            ObservableCollection<PersonModel> PossibleMothers = new ObservableCollection<PersonModel>();
            var allWomenInDatabase = db.QueryByExample(new PersonModel(EGender.Female));
            foreach (var woman in allWomenInDatabase)
            {
                PossibleMothers.Add((PersonModel)woman);
            }
            db.Close();
            return PossibleMothers;
        }
        public void AddPersonToDatabase()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            PersonModel newPerson = new PersonModel
            {
                Name = Name,
                BirthDate = BirthDate,
                DeathDate = DeathDate,
                Gender = Gender,
                Father = Father,
                Mother = Mother
            };
            db.Store(newPerson);
            db.Close();
            Name = null;
            Father = null;
            Mother = null;
            BirthDate = DateTime.MinValue;
            DeathDate = DateTime.MinValue;
            NotifyOfPropertyChange("PossibleFathers");
            NotifyOfPropertyChange("PossibleMothers");
        }
    }
}
