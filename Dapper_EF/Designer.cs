using System.Collections.Generic;

namespace Dapper.Domain

{
  public class DapperDesigner
    {
    public int Id { get; set; }
    public string LabelName { get; set; }
    public string Founder { get; set; }
    public Dapperness Dapperness { get; set; }
    //has to be many to many
    public List<Client> Clients { get; set; }
    public List<Product> Products { get; set; }

    public ContactInfo ContactInfo { get; set; }
  }
 
}
