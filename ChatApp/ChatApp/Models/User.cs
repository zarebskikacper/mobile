using System.ComponentModel.DataAnnotations;

namespace ChatApp.Models;
public class User
{
    [Required(ErrorMessage = "Pole imię wymagane")][MinLength(3, ErrorMessage = "Twoje imię jest zbyt krótkie")]
    public string Imie { get; set; }
    [Required(ErrorMessage = "Pole nazwisko wymagane")][MinLength(3, ErrorMessage = "Twoje nazwisko jest zbyt krótkie")]
    public string Nazwisko { get; set; }
}
