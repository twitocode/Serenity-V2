using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Serenity.Database.Entities;

public class Friendship
{
    [Key]
    public string Id { get; set; }

    public List<User> Users { get; set; }
}
