namespace AdvWorks.Domain
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  [Table("Production.ProductDocument")]
  public partial class ProductDocument
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ProductID { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual Product Product { get; set; }
  }
}