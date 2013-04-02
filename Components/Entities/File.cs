using System;

namespace IgorKarpov.DocumentsExchangeModule.Components.Entities
{
    /// <summary>
    /// Represents File entity from IgorKarpov_DocumentsExchangeModule_Files.
    /// </summary>
    public class File
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

        public String OriginalName
        {
            get;
            set;
        }

        public String ContentType
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

        public DateTime LastVersionDate
        {
            get;
            set;
        }
    }
}