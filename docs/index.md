# ENMS Documentation

This is documentation for the ENMS project.

The documentation is split in two sections. The [index](#index) provides a
bottom-up approach and the [FAQ](#faq) provides a top-down approach.

## Index

Here is an outline of the various topics covered by the documentation giving the
reader a bottom up perspective. Read this if you already know what piece of
information you need.

- [Scripts](docs/scripts.md)
- [Project structure](docs/structure/index.md)
  - [Testing](docs/structure/test.md)
  - [Database schema](docs/structure/data/index.md)
  - [Business layer](docs/structure/business/index.md)
  - [Server](docs/structure/server.md)
  - [Client](docs/structure/client.md)
  - [Faking measurements](docs/structure/fake.md)
- [User](docs/user/index.md)
  - [Tenant](docs/user/tenant/index.md)
  - [Subnet](docs/user/subnet/index.md)
  - [Operator](docs/user/operator/index.md)
  - [Admin](docs/user/admin/index.md)
- [Report](docs/report/en/index.md)
  - [Development environment](docs/report/en/development-environment.md)
  - [Backend](docs/report/en/backend.md)
  - [Frontend](docs/report/en/frontend.md)

## FAQ

Here is a question answering portion of the documentation giving the reader a
top down perspective. Read this if you don't know what piece of information you
need.

### How do I prepare ENMS for development?

Install [just](https://github.com/casey/just#packages),
[docker](https://docs.docker.com/engine/install/),
[node](https://nodejs.org/en/download) and
[dotnet](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md?WT.mc_id=dotnet-35129-website)
and run `just prepare` from the command line. If on Windows please make sure you
have `pwsh.exe` installed by running `pwsh -v` on the command line and if not
[install it](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows).

### How do I start developing ENMS?

After [preparing ENMS for development](#how-do-i-prepare-enms-for-development),
open ENMS in your editor of choice (we recommend
[Visual Studio Code](https://code.visualstudio.com/)) and run `just dev` on the
command line. We also recommend seeding the database with measurements
continuously by running `just fake` on another command line. After the server
starts, navigate to [http://localhost:5000] in your browser of choice. Hot
reload is enabled by default so there is no need of rerunning any commands on
changes.

### How do I debug ENMS?

Start [developing ENMS](#how-do-i-start-developing-enms) except running
`just dev` on the command line and instead run the appropriate configuration in
your editor of choice. For Visual Studio Code, this will be the `debug`
configuration and for Visual Studio this will be the `Enms.Server` startup
project.

### I have changed the database scheme of ENMS - how do I create a migration?

Run `just migrate {{name-of-migration}}` from the command line.

### How do I run tests on ENMS?

Most likely, your editor of choice will discover tests and you can run them from
there, but if you prefer, you can run `just test` on the command line. Please
refer to the
[dotnet test documentation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test)
for more details on the various options you can pass. The most common use case
is to limit the tests ran to a specific name and you can do that with
`just test --filter "FullyQualifiedName~{{your-test-name}}"`.

### Something has gone wrong with my development ENMS database - how do I reset it?

Run `just clean` from the command line.

### I am getting cryptic errors - what is the panic button?

Run `just purge` from the command line.
