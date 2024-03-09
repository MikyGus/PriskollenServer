﻿namespace PriskollenServer.Library.Models;
public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Distance { get; set; } = null;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public StoreChain? StoreChain { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
}