namespace HospitalProject_Group3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donorTransplant : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DonorTransplants",
                c => new
                    {
                        DonorTransplantID = c.Int(nullable: false, identity: true),
                        OrganType = c.String(),
                        SurgeryPlan = c.String(),
                        WaitListNumber = c.String(),
                        PatientID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonorTransplantID)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .Index(t => t.PatientID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DonorTransplants", "PatientID", "dbo.Patients");
            DropIndex("dbo.DonorTransplants", new[] { "PatientID" });
            DropTable("dbo.DonorTransplants");
        }
    }
}
