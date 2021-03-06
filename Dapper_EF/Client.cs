﻿using System.Collections.Generic;

namespace DapperDesigners.Domain
{
  public class Client
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public Dapperness DefaultDappernessLevel { get; set; }
    public List<DapperDesigner> Designers { get; set; }
  }
}