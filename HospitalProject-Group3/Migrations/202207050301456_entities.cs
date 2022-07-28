namespace HospitalProject_Group3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        AppointmentDateTime = c.DateTime(nullable: false),
                        StaffID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffID, cascadeDelete: true)
                .Index(t => t.StaffID)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        PatientFName = c.String(),
                        PatientLName = c.String(),
                        Gender = c.Int(nullable: false),
                        PhoneNumber = c.String(),
                        BloodType = c.String(),
                        Email = c.String(),
                        BornDate = c.DateTime(nullable: false),
                        PatientHasPhoto = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        InsuranceID = c.Int(nullable: false),
                        Medication_MedicationID = c.Int(),
                    })
                .PrimaryKey(t => t.PatientID)
                .ForeignKey("dbo.Insurances", t => t.InsuranceID, cascadeDelete: true)
                .ForeignKey("dbo.Medications", t => t.Medication_MedicationID)
                .Index(t => t.InsuranceID)
                .Index(t => t.Medication_MedicationID);
            
            CreateTable(
                "dbo.Bills",
                c => new
                    {
                        BillID = c.Int(nullable: false, identity: true),
                        BillDetail = c.String(),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PatientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BillID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.DonorTransplants",
                c => new
                    {
                        DonorID = c.Int(nullable: false, identity: true),
                        OrganType = c.String(),
                        SurgeryPlan = c.String(),
                        WaitListNumber = c.String(),
                        PatientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonorID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.Insurances",
                c => new
                    {
                        InsuranceID = c.Int(nullable: false, identity: true),
                        InsuranceCompany = c.String(),
                        InsurancePlan = c.String(),
                    })
                .PrimaryKey(t => t.InsuranceID);
            
            CreateTable(
                "dbo.MedicalRecords",
                c => new
                    {
                        RecordID = c.Int(nullable: false, identity: true),
                        RecordDetail = c.String(),
                        RecordDate = c.DateTime(nullable: false),
                        PatientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RecordID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionID = c.Int(nullable: false, identity: true),
                        PrescriptionDate = c.DateTime(nullable: false),
                        DoctorNote = c.String(),
                        StaffID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
                        MedicationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrescriptionID)
                .ForeignKey("dbo.Medications", t => t.MedicationID, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.StaffID, cascadeDelete: true)
                .Index(t => t.StaffID)
                .Index(t => t.PatientID)
                .Index(t => t.MedicationID);
            
            CreateTable(
                "dbo.Medications",
                c => new
                    {
                        MedicationID = c.Int(nullable: false, identity: true),
                        MedicationName = c.String(),
                        MedicationBrand = c.String(),
                        MedicationDetail = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MedicationID);
            
            CreateTable(
                "dbo.Staffs",
                c => new
                    {
                        StaffID = c.Int(nullable: false, identity: true),
                        StaffFName = c.String(),
                        StaffLName = c.String(),
                        StaffBio = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        HireDate = c.DateTime(nullable: false),
                        StaffHasPhoto = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        RoleID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffID)
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        RoleType = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        RoomNumber = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Shifts",
                c => new
                    {
                        ShiftID = c.Int(nullable: false, identity: true),
                        ShiftDay = c.Int(nullable: false),
                        ShiftTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ShiftID);
            
            CreateTable(
                "dbo.ShiftStaffs",
                c => new
                    {
                        Shift_ShiftID = c.Int(nullable: false),
                        Staff_StaffID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Shift_ShiftID, t.Staff_StaffID })
                .ForeignKey("dbo.Shifts", t => t.Shift_ShiftID, cascadeDelete: true)
                .ForeignKey("dbo.Staffs", t => t.Staff_StaffID, cascadeDelete: true)
                .Index(t => t.Shift_ShiftID)
                .Index(t => t.Staff_StaffID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.Appointments", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Prescriptions", "StaffID", "dbo.Staffs");
            DropForeignKey("dbo.ShiftStaffs", "Staff_StaffID", "dbo.Staffs");
            DropForeignKey("dbo.ShiftStaffs", "Shift_ShiftID", "dbo.Shifts");
            DropForeignKey("dbo.Staffs", "RoleID", "dbo.Roles");
            DropForeignKey("dbo.Roles", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.Prescriptions", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Prescriptions", "MedicationID", "dbo.Medications");
            DropForeignKey("dbo.Patients", "Medication_MedicationID", "dbo.Medications");
            DropForeignKey("dbo.MedicalRecords", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Patients", "InsuranceID", "dbo.Insurances");
            DropForeignKey("dbo.DonorTransplants", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.Bills", "PatientID", "dbo.Patients");
            DropIndex("dbo.ShiftStaffs", new[] { "Staff_StaffID" });
            DropIndex("dbo.ShiftStaffs", new[] { "Shift_ShiftID" });
            DropIndex("dbo.Roles", new[] { "DepartmentID" });
            DropIndex("dbo.Staffs", new[] { "RoleID" });
            DropIndex("dbo.Prescriptions", new[] { "MedicationID" });
            DropIndex("dbo.Prescriptions", new[] { "PatientID" });
            DropIndex("dbo.Prescriptions", new[] { "StaffID" });
            DropIndex("dbo.MedicalRecords", new[] { "PatientID" });
            DropIndex("dbo.DonorTransplants", new[] { "PatientID" });
            DropIndex("dbo.Bills", new[] { "PatientID" });
            DropIndex("dbo.Patients", new[] { "Medication_MedicationID" });
            DropIndex("dbo.Patients", new[] { "InsuranceID" });
            DropIndex("dbo.Appointments", new[] { "PatientID" });
            DropIndex("dbo.Appointments", new[] { "StaffID" });
            DropTable("dbo.ShiftStaffs");
            DropTable("dbo.Shifts");
            DropTable("dbo.Departments");
            DropTable("dbo.Roles");
            DropTable("dbo.Staffs");
            DropTable("dbo.Medications");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.MedicalRecords");
            DropTable("dbo.Insurances");
            DropTable("dbo.DonorTransplants");
            DropTable("dbo.Bills");
            DropTable("dbo.Patients");
            DropTable("dbo.Appointments");
        }
    }
}
