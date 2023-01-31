using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class RegisterInputModel
    {
        public string FullNome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem")]
        public string ConfirmPassword { get; set; }

        public bool agreeTerms { get; set; } = false;
        public string ReturnUrl { get; set; }
    }
}
