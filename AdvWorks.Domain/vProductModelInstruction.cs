namespace AdvWorks.Domain
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Production.vProductModelInstructions")]
  public partial class vProductModelInstruction
  {
    [Key]
    [Column(Order = 0)]
    public int ProductModelID { get; set; }

    [Key]
    [Column(Order = 1)]
    [StringLength(50)]
    public string Name { get; set; }

    public string Instructions { get; set; }

    public int? LocationID { get; set; }

    public decimal? SetupHours { get; set; }

    public decimal? MachineHours { get; set; }

    public decimal? LaborHours { get; set; }

    public int? LotSize { get; set; }

    [StringLength(1024)]
    public string Step { get; set; }

    [Key]
    [Column(Order = 2)]
    public Guid rowguid { get; set; }

    [Key]
    [Column(Order = 3)]
    public DateTime ModifiedDate { get; set; }
  }
}