using System.Text.RegularExpressions;

namespace FiapCloudGames.Domain.Validators;

public static class PasswordValidator
{
    private const int MinLength = 8;
    
    public static bool IsValid(string password, out string errorMessage)
    {
        errorMessage = string.Empty;
        
        if (string.IsNullOrWhiteSpace(password))
        {
            errorMessage = "A senha não pode estar vazia.";
            return false;
        }
        
        if (password.Length < MinLength)
        {
            errorMessage = $"A senha deve ter no mínimo {MinLength} caracteres.";
            return false;
        }
        
        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            errorMessage = "A senha deve conter pelo menos uma letra maiúscula.";
            return false;
        }
        
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            errorMessage = "A senha deve conter pelo menos uma letra minúscula.";
            return false;
        }
        
        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            errorMessage = "A senha deve conter pelo menos um número.";
            return false;
        }
        
        if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':;{}|<>_\-+=\[\]\\\/`~]"))
        {
            errorMessage = "A senha deve conter pelo menos um caractere especial.";
            return false;
        }
        
        return true;
    }
}
