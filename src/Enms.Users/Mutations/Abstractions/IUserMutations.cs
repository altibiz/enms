using Enms.Users.Entities;

namespace Enms.Users.Mutations.Abstractions;

public interface IUserMutations
{
  Task Create(UserEntity user);

  Task Update(UserEntity user);

  Task Delete(UserEntity user);
}
