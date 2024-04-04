namespace Enms.Business.Models.Composite;

public record MaybeRepresentingUserModel(
  UserModel User,
  RepresentativeModel? Representative
);
