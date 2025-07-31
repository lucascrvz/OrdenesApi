using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class EditOrderDto
{
    public string Cliente { get; set; } = null!;
    public List<int> ProductoIds { get; set; } = new();
}
