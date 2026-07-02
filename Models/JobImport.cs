namespace QueenswoodSoftware.Utils.Models
{
    /// <summary>
    /// Represents the payload for importing job data.
    /// </summary>
    public class JobImportPayload
    {
        public string? Version { get; set; }
        public int Count { get; set; }
        public List<CaseItem> Cases { get; set; }
    }

    /// <summary>
    /// Represents an individual case item in the import payload.
    /// </summary>
    public class CaseItem
    {
        public string? CaseReference { get; set; }
        public string? SummaryOfWork { get; set; }
        public string? ServiceManagerJob { get; set; }
        public string? CaseCreatedByUser { get; set; }
        public string? CaseCreatedByUserEmail { get; set; }
        public string? CaseStatus { get; set; }
        public string? Platform { get; set; }
        public string? PlatformId { get; set; }
        public long TenantNumber { get; set; }
        public long CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyType { get; set; }
        public string? ClientReference { get; set; }
        public string? OrderNumber { get; set; }
        public string? DealType { get; set; }
        public string? WorkDescription { get; set; }
        public JobDetail? Job { get; set; }
        public SiteVisit? SiteVisit { get; set; }
        public Vehicle? Vehicle { get; set; }
        public Contact? Contact { get; set; }
        public SalesOptions? SalesOptions { get; set; }
    }

    public class JobDetail
    {
        public string? JobType { get; set; } // Install, Repair, Reinstall, Deinstall
        public string? JobClientType { get; set; }
        public int? UnitLocationID { get; set; }
    }

    /// <summary>
    /// Represents a site visit in the import payload.
    /// </summary>
    public class SiteVisit
    {
        public DateTime BookedDateTime { get; set; }
        public int EstimatedDurationSeconds { get; set; }
        public Address Address { get; set; }
        public Technician Technician { get; set; }
    }

    /// <summary>
    /// Represents an address in the import payload.
    /// </summary>
    public class Address
    {
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? Address4 { get; set; }
        public string? Suburb { get; set; }
        public string? Town { get; set; }
        public string? Province { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
    }

    /// <summary>
    /// Represents a technician in the import payload.
    /// </summary>
    public class Technician
    {
        public long TechnicianId { get; set; }
        public string? TechName { get; set; }
        public string? TechSurname { get; set; }
        public string? TechFitmentCentre { get; set; }
        public string? TechEmail { get; set; }
    }

    /// <summary>
    /// Represents a vehicle in the import payload.
    /// </summary>
    public class Vehicle
    {
        public string? Channel { get; set; }
        public long VehicleId { get; set; }
        public string? UniqueAccountingVehicleId { get; set; }
        public string? VehicleType { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public string? Year { get; set; }
        public string? Colour { get; set; }
        public string? SecondaryColour { get; set; }
        public string? RegistrationNr { get; set; }
        public string? VinNr { get; set; }
        public string? EngineNr { get; set; }
        public string? FleetNr { get; set; }
        public string? Odo { get; set; }
        public string? UnitSerialNr { get; set; }
        public int UnitLocationId { get; set; }
        public string? TrackingCellNrIN { get; set; }
        public string? TrackingCellNrOUT { get; set; }
        public string? SimSerialNrIN { get; set; }
        public string? SimSerialNrOUT { get; set; }
        public string? Depot { get; set; }
        public List<EmergencyContact>? EmergencyContacts { get; set; }
    }

    /// <summary>
    /// Represents an emergency contact in the import payload.
    /// </summary>
    public class EmergencyContact
    {
        public string? ContactName { get; set; }
        public string? ContactCell { get; set; }
        public string? ContactEmail { get; set; }
    }

    /// <summary>
    /// Represents a contact in the import payload.
    /// </summary>
    public class Contact
    {
        public long PersonId { get; set; }
        public string? ContactName { get; set; }
        public string? CellNumber { get; set; }
        public string? BusinessPhone { get; set; }
        public string? Email { get; set; }
    }

    /// <summary>
    /// Represents sales options in the import payload.
    /// </summary>
    public class SalesOptions
    {
        public int TwoThreeWire { get; set; }
        public string? ReportingHubName { get; set; }
        public string? InternalName { get; set; }
        public string? MarketingName { get; set; }
        public string? HardwarePlatform { get; set; }
        public List<Feature>? Features { get; set; }
    }

    /// <summary>
    /// Represents a feature in the import payload.
    /// </summary>
    public class Feature
    {
        public string? FeatureName { get; set; }
        public List<Option>? Options { get; set; }
    }

    /// <summary>
    /// Represents an option in the import payload.
    /// </summary>
    public class Option
    {
        public int OptionId { get; set; }
        public string? OptionName { get; set; }
        public string? InOut { get; set; } = "Y";
        public string? DisplayLabel { get; set; }
        public string? OptionValue { get; set; }
        public bool IsVisibleMRMHandset { get; set; }
    }
}