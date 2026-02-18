using System;
using System.Collections.Generic;

namespace Logs.Models;

public partial class UsersCredential
{
    public int Userid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
