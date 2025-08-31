﻿using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_Application.Models;

public class Donation
{
    [EmailAddress]
    public required string Email { get; set; }
    public required int Amount { get; set; }
}
