using System.ComponentModel.DataAnnotations;

public class UserModel {
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required][EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }

    [Required][RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be 10 digits")]
    public string Phone { get; set; }

    [Required][MinLength(8, ErrorMessage = "Min 8 characters")]
    public string Password { get; set; }
}