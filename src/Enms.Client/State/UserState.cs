using System.Security.Claims;
using Enms.Business.Models.Composite;

namespace Enms.Client.State;

public record UserState(
  ClaimsPrincipal ClaimsPrincipal,
  MaybeRepresentingUserModel User
);
