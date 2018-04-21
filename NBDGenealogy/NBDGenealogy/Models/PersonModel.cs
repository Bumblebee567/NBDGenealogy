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
        Female
    }
    public class PersonModel
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
        public EGender Gender { get; set; }
        public PersonModel Father { get; set; }
        public PersonModel Mother { get; set; }
        public List<PersonModel> Children { get; set; }
        public PersonModel()
        {
        }
        public PersonModel(EGender gender)
        {
            Gender = gender;
        }
    }
}
