﻿namespace PriskollenServer.Library.Models;
public class StoreChain
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string Image { get; set; } = string.Empty;
}