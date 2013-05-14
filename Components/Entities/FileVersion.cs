using System;

namespace IgorKarpov.DocumentsExchangeModule.Components.Entities
{
    /// <summary>
    /// Represents File entity from IgorKarpov_DocumentsExchangeModule_FileVersions.
    /// </summary>
    public class FileVersion
    {
        public int Id
        {
            get;
            set;
        }

        public int FileId
        {
            get;
            set;
        }

        public String Comment
        {
            get;
            set;
        }

        public String LocalName
        {
            get;
            set;
        }

        public int CreatorUserId
        {
            get;
            set;
        }

        public String CreatorDisplayName
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