namespace Enms.Business.Models.Composite;

public record RepresentingUserModel(
  UserModel User
) : MaybeRepresentingUserModel(User)
{
  public required RepresentativeModel Representative { get; set; }

  public override RepresentativeModel? MaybeRepresentative
  {
    get { return Representative; }
    set { Representative = value ?? RepresentativeModel.New(User); }
  }
}
