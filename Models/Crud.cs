namespace QueenswoodSoftware.Utils.Models
{
    /// <summary>
    /// CRUD and lifecycle operations for data entities.
    /// </summary>
    public enum Crud
    {
        /// <summary>Create a new record.</summary>
        CREATE,

        /// <summary>Read existing data.</summary>
        READ,

        /// <summary>Update an existing record.</summary>
        UPDATE,

        /// <summary>Delete a record.</summary>
        DELETE,

        /// <summary>Open or load a record for editing.</summary>
        OPEN,

        /// <summary>Mark a record as active.</summary>
        ACTIVE
    }
}