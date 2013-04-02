using System;

namespace IgorKarpov.DocumentsExchangeModule.Components.Entities
{
    /// <summary>
    /// Represents Folder entity from IgorKarpov_DocumentsExchangeModule_Folders.
    /// </summary>
    public class Folder
    {
        public int Id
        {
            get;
            set;
        }

        public int? ParentFolderId
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public int CreatorUserId
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }
    }
}