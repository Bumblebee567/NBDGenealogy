using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Helpers
{
    class SuccessorsTreeHelper
    {
        public static SuccessorsTreeItem GetPersonsChildren(string personName)
        {
            SuccessorsTreeItem root = new SuccessorsTreeItem { Title = personName };
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            var person = db.QueryByExample(new PersonModel(personName)).Next() as PersonModel;
            db.Close();
            if (person.Children != null)
            {
                foreach (var child in person.Children)
                {
                    db = Db4oFactory.OpenFile("person.data");
                    var personChild = db.QueryByExample(new PersonModel(child)).Next() as PersonModel;
                    db.Close();
                    root.Items.Add(GetPersonsChildren(personChild.Name));
                }
            }
            return root;
        }
    }
}
