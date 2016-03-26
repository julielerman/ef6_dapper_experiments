using System.Collections.Generic;

namespace Dapper.Domain

{
  public class DapperDesigner
    {
    public DapperDesigner() {
      Products = new List<Product>();
      Clients = new List<Client>();
    }
    public int Id { get; set; }
    public string LabelName { get; set; }
    public string Founder { get; set; }
    public Dapperness Dapperness { get; set; }
    //has to be many to many
    public List<Client> Clients { get; set; }
    public List<Product> Products { get; set; }

    public ContactInfo ContactInfo { get; set; }
  }

  public class MiniDesigner {
    public int Id { get; set; }
    public string Name { get; set; }
    public string FoundedBy { get; set; }
  }

}
