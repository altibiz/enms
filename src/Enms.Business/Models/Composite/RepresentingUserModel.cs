namespace Enms.Business.Models.Composite;

public record RepresentingUserModel(
  UserModel User,
  RepresentativeModel Representative
);
