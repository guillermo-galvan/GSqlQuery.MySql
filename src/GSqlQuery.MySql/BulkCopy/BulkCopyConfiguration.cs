using System;

namespace GSqlQuery.MySql.BulkCopy
{
    public class BulkCopyConfiguration
    {
        internal const string ALLOWLOADLOCALINFILE = "ALLOWLOADLOCALINFILE=TRUE";
        internal const string ALLOWUSERVARIABLES = "ALLOWUSERVARIABLES=TRUE";
        internal const string FIELDTERMINATOR = "^";

        public string ConnectionString { get; }

        public IFormats Formats { get; }

        public BulkCopyEvents Events { get; }

        public BulkCopyConfiguration(string connectionString) : this(connectionString, new MySqlFormats(), new BulkCopyEvents())
        { }

        public BulkCopyConfiguration(string connectionString, IFormats formats) : this(connectionString, formats, new BulkCopyEvents())
        { }

        public BulkCopyConfiguration(string connectionString, IFormats formats, BulkCopyEvents events)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (!connectionString.ToUpper().Contains(ALLOWLOADLOCALINFILE))
            {
                throw new InvalidOperationException("Connection string does not contain AllowLoadLocalInfile=true");
            }

            if (!connectionString.ToUpper().Contains(ALLOWUSERVARIABLES))
            {
                throw new InvalidOperationException("Connection string does not contain AllowUserVariables=true");
            }

            ConnectionString = connectionString;
            Formats = formats ?? throw new ArgumentNullException(nameof(formats));
            Events = events ?? throw new ArgumentNullException(nameof(events));
        }
    }
}
