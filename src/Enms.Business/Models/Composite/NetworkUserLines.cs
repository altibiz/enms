using Enms.Business.Models.Abstractions;

namespace Enms.Business.Models.Composite;

public record NetworkUserLines(
  NetworkUserModel NetworkUser,
  List<ILine> Lines
);
