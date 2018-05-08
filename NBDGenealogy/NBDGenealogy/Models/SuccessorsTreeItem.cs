using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBDGenealogy.Models
{
    public class SuccessorsTreeItem
    {
        public SuccessorsTreeItem()
        {
            this.Items = new ObservableCollection<SuccessorsTreeItem>();
        }

        public string Title { get; set; }

        public ObservableCollection<SuccessorsTreeItem> Items { get; set; }
    }
}
