using FiapCloudGames.Application.DTOs;
using FiapCloudGames.Domain.Interfaces;
using FiapCloudGames.Domain.Validators;

namespace FiapCloudGames.Application.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return null;
        
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.CreatedAt
        );
    }
    
    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
    {
        var users = await _unitOfWork.Users.GetAllAsync();
        return users.Select(u => new UserResponse(
            u.Id,
            u.Name,
            u.Email,
            u.Role,
            u.CreatedAt
        ));
    }
    
    public async Task<(bool Success, string Message)> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return (false, "Usuário não encontrado.");
        }
        
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            user.Name = request.Name;
        }
        
        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            if (!EmailValidator.IsValid(request.Email))
            {
                return (false, "E-mail inválido.");
            }
            
            var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == request.Email && u.Id != id);
            if (existingUser != null)
            {
                return (false, "E-mail já está em uso.");
            }
            
            user.Email = request.Email;
        }
        
        user.UpdatedAt = DateTime.UtcNow;
        
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Usuário atualizado com sucesso.");
    }
    
    public async Task<(bool Success, string Message)> DeleteUserAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            return (false, "Usuário não encontrado.");
        }
        
        await _unitOfWork.Users.DeleteAsync(user);
        await _unitOfWork.SaveChangesAsync();
        
        return (true, "Usuário deletado com sucesso.");
    }
}
