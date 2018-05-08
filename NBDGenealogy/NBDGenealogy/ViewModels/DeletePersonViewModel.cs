using Caliburn.Micro;
using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NBDGenealogy.ViewModels
{
    class DeletePersonViewModel : Screen
    {
        private BindableCollection<PersonModel> _allPeopleInDatabase;
        private PersonModel _selectedPerson;

        public PersonModel SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);
            }
        }

        public BindableCollection<PersonModel> AllPeopleInDatabase
        {
            get
            {
                return GetAllPeopleFormDatabase();
            }
            set
            {
                _allPeopleInDatabase = value;
                NotifyOfPropertyChange(() => AllPeopleInDatabase);
            }
        }

        #region methods
        public BindableCollection<PersonModel> GetAllPeopleFormDatabase()
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            BindableCollection<PersonModel> allPeopleInDatabse = new BindableCollection<PersonModel>();
            var allPeople = db.QueryByExample(new PersonModel());
            foreach (var person in allPeople)
            {
                allPeopleInDatabse.Add((PersonModel)person);
            }
            db.Close();
            foreach (var person in allPeopleInDatabse)
            {
                if (person.BirthDate != null)
                    person.BirthDate.Value.ToShortDateString();
                if (person.DeathDate != null)
                    person.DeathDate.Value.ToShortDateString();
            }
            return allPeopleInDatabse;
        }
        public void DeletePerson()
        {
            if (SelectedPerson == null)
            {
                MessageBox.Show("Nie wybrano osoby", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                var result = MessageBox.Show("Czy chcesz usunąć wybraną osobę?", "Pytanie", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    IObjectContainer db = Db4oFactory.OpenFile("person.data");
                    var personToRemove = db.QueryByExample(new PersonModel(SelectedPerson.Name)).Next();
                    var personToRemoveAsPersonModel = personToRemove as PersonModel;
                    if(personToRemoveAsPersonModel.Children != null)
                    {
                        if(personToRemoveAsPersonModel.Gender == EGender.Male)
                        {
                            var personsChildren = db.QueryByExample(PersonModel.WithFather(personToRemoveAsPersonModel.Name));
                            var childrenAsPersonModel = new List<PersonModel>();
                            foreach (var child in personsChildren)
                            {
                                childrenAsPersonModel.Add(child as PersonModel);
                            }
                            foreach (var child in childrenAsPersonModel)
                            {
                                child.Father = null;
                                db.Store(child);
                            }
                            personToRemoveAsPersonModel.Children = null;
                        }
                        else
                        {
                            var personsChildren = db.QueryByExample(PersonModel.WithMother(personToRemoveAsPersonModel.Name));
                            var childrenAsPersonModel = new List<PersonModel>();
                            foreach (var child in personsChildren)
                            {
                                childrenAsPersonModel.Add(child as PersonModel);
                            }
                            foreach (var child in childrenAsPersonModel)
                            {
                                child.Mother = null;
                                db.Store(child);
                            }
                            personToRemoveAsPersonModel.Children = null;
                        }
                    }
                    if(personToRemoveAsPersonModel.Father != null && personToRemoveAsPersonModel.Father != String.Empty)
                    {
                        var personsFather = db.QueryByExample(new PersonModel(personToRemoveAsPersonModel.Father)).Next();
                        var personsFatherAsPersonModel = personsFather as PersonModel;
                        personsFatherAsPersonModel.Children.Remove(personToRemoveAsPersonModel.Name);
                        if(personsFatherAsPersonModel.Children.Count == 0)
                        {
                            personsFatherAsPersonModel.Children = null;
                        }
                        db.Store(personsFatherAsPersonModel);
                    }
                    if(personToRemoveAsPersonModel.Mother != null && personToRemoveAsPersonModel.Mother != String.Empty)
                    {
                        var personsMother = db.QueryByExample(new PersonModel(personToRemoveAsPersonModel.Mother)).Next();
                        var personsMotherAsPersonModel = personsMother as PersonModel;
                        personsMotherAsPersonModel.Children.Remove(personToRemoveAsPersonModel.Name);
                        if(personsMotherAsPersonModel.Children.Count == 0)
                        {
                            personsMotherAsPersonModel.Children = null;
                        }
                        db.Store(personsMotherAsPersonModel);
                    }
                    db.Delete(personToRemove);
                    db.Commit();
                    db.Close();
                    NotifyOfPropertyChange(() => AllPeopleInDatabase);
                }
                else if(result == MessageBoxResult.No)
                {
                    return;
                }
            }
        }
        #endregion
    }
}
