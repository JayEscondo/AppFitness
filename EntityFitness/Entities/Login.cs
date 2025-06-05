using System;
using System.Collections.Generic;

namespace EntitiesFitness;

public partial class Login
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
