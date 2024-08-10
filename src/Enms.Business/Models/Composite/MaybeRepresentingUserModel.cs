namespace Enms.Business.Models.Composite;

public record MaybeRepresentingUserModel(
  UserModel User
)
{
  public virtual RepresentativeModel? MaybeRepresentative { get; set; }
}
