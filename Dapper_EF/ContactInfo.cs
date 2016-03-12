namespace Dapper.Domain
{
  public class ContactInfo
  {
    public int Id { get; set; }
    public string Twitter { get; set; }
    public DapperDesigner Designer { get; private set; }
  }
}