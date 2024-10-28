using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Customer
{
    [Key]
    public int ID { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }
}
