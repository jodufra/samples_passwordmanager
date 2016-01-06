using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Entities
{
    [DataContract]
    public class Category
    {
        int idCategory = 0;
        string title = "";
        string categoryPath = "0";
        int? parentCategoryId = null;
        string icon = null;

        [DataMember]
        public int IdCategory
        {
            get { return idCategory; }
            set { idCategory = value; categoryPath = idCategory.ToString(); }
        }
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [DataMember]
        public string CategoryPath
        {
            get { return categoryPath; }
            set { categoryPath = value; }
        }
        [DataMember]
        public int? ParentCategoryId
        {
            get { return parentCategoryId; }
            set { parentCategoryId = value; }
        }
        [DataMember]
        public string Icon
        {
            get { return icon; }
            set { icon = value; }
        }

    }
}