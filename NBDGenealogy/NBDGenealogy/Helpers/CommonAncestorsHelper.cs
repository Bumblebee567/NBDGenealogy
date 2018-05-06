using Db4objects.Db4o;
using NBDGenealogy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Helpers
{
    class CommonAncestorsHelper
    {
        public static string ListOfCommonAncestors(string firstPersonName, string secondPersonName)
        {
            List<string> firstPersonAncestors = ListOfAncestors(firstPersonName);
            List<string> secondPersonAncestors = ListOfAncestors(secondPersonName);

            List<string> commonAncestors = new List<string>();
            foreach (var ancestor in firstPersonAncestors)
            {
                if(secondPersonAncestors.Contains(ancestor))
                {
                    commonAncestors.Add(ancestor);
                }
            }

            var sb = new StringBuilder();
            foreach (var ancestor in commonAncestors)
            {
                sb.AppendLine(ancestor);
            }
            return sb.ToString();

        }
        public static List<string> ListOfAncestors(string personName)
        {
            IObjectContainer db = Db4oFactory.OpenFile("person.data");
            var person = db.QueryByExample(new PersonModel(personName)).Next() as PersonModel;
            db.Close();
            List<string> personAncestors = new List<string>();
            if (person.Father != null)
            {
                personAncestors.Add(person.Father);
                foreach (var ancestor in ListOfAncestors(person.Father))
                {
                    personAncestors.Add(ancestor);
                }
            }
            if (person.Mother != null)
            {
                personAncestors.Add(person.Mother);
                db.Close();
                foreach (var ancestor in ListOfAncestors(person.Mother))
                {
                    personAncestors.Add(ancestor);
                }
            }
            return personAncestors;
        }
    }
}
