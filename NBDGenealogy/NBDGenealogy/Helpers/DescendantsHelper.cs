using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Helpers
{
    class DescendantsHelper
    {
        public static List<PersonModel> GetPersonDescendants(PersonModel person)
        {
            List<PersonModel> personDescendants = new List<PersonModel>();

            foreach (var child in person.Children)
            {
                IObjectContainer db = Db4oFactory.OpenFile("person.data");
                var childAsPersonModel = db.QueryByExample(new PersonModel(child)).Next() as PersonModel;
                db.Close();
                personDescendants.Add(childAsPersonModel);
                if (childAsPersonModel.Children != null)
                {
                    foreach (var childsChild in childAsPersonModel.Children)
                    {
                        db = Db4oFactory.OpenFile("person.data");
                        var childsChildAsPersonModel = db.QueryByExample(new PersonModel(childsChild)).Next() as PersonModel;
                        db.Close();
                        personDescendants.AddRange(GetPersonDescendants(childsChildAsPersonModel));
                    }
                }
            }
            return personDescendants;
        }
    }
}
