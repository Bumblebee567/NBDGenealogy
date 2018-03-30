using Caliburn.Micro;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Gender MyProperty
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
    }
}
