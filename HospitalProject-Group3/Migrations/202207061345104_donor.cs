namespace HospitalProject_Group3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DonorTransplants", "PatientID", "dbo.Patients");
            DropIndex("dbo.DonorTransplants", new[] { "PatientID" });
            DropTable("dbo.DonorTransplants");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.DonorID);
            
            CreateIndex("dbo.DonorTransplants", "PatientID");
            AddForeignKey("dbo.DonorTransplants", "PatientID", "dbo.Patients", "PatientID", cascadeDelete: true);
        }
    }
}
