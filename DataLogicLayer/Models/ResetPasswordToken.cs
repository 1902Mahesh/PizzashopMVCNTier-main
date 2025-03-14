using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class ResetPasswordToken
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public bool IsUsed { get; set; }

    public DateTime Expirytime { get; set; }
}
