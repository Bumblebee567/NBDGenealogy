using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Models
{
    public enum EGender
    {
        Male,
        Female,
        brak
    }
    public class PersonModel
    {
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public EGender? Gender { get; set; }
        public string Father { get; set; }
        public string Mother { get; set; }
        public List<string> Children { get; set; }
        public PersonModel()
        {
        }
        public PersonModel(EGender gender)
        {
            Gender = gender;
        }
        public PersonModel(string name)
        {
            Name = name;
        }
        public static PersonModel WithFather(string father)
        {
            var person = new PersonModel();
            person.Father = father;
            return person;
        }
        public static PersonModel WithMother(string mother)
        {
            var person = new PersonModel();
            person.Mother = mother;
            return person;
        }
    }
}
