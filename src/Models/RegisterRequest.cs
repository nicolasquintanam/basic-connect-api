namespace BasicConnectApi.Models;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterRequest
{
    [Required(ErrorMessage="The 'first_name' field is required.")]
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage="The 'last_name' field is required.")]
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [Required(ErrorMessage="The 'email' field is required.")]
    [EmailAddress(ErrorMessage="The 'email' field is not a valid e-mail address.")]
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    [Required(ErrorMessage="The 'password' field is required.")]
    public string Password { get; set; }
}