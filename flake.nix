rec {
  description = "ENMS";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";

    flake-utils.url = "github:numtide/flake-utils";

    poetry2nix.url = "github:nix-community/poetry2nix";
  };

  outputs = { self, nixpkgs, flake-utils, ... } @rawInputs:
    flake-utils.lib.eachDefaultSystem (system:
      let
        pkgs = import nixpkgs {
          inherit system;
          config = { allowUnfree = true; };
          overlays = [
            (final: prev: {
              nodejs = prev.nodejs_20;
              dotnet-sdk = prev.dotnet-sdk_8;
              dotnet-runtime = prev.dotnet-runtime_8;
              dotnet-aspnetcore = prev.dotnet-aspnetcore_8;
            })
          ];
        };

        mkEnvWrapper = env: name: pkgs.writeShellApplication {
          name = name;
          runtimeInputs = [ env ];
          text = ''
            export PYTHONPREFIX=${env}
            export PYTHONEXECUTABLE=${env}/bin/python

            # shellcheck disable=SC2125
            export PYTHONPATH=${env}/lib/**/site-packages

            ${name} "$@"
          '';
        };

        poetry2nix = rawInputs.poetry2nix.lib.mkPoetry2Nix { inherit pkgs; };

        pythonEnv = poetry2nix.mkPoetryEnv {
          projectDir = self;
          preferWheels = true;
          checkGroups = [ ];
          overrides = poetry2nix.defaultPoetryOverrides.extend (final: prev: {
            pyright = prev.pyright.overridePythonAttrs (old: {
              postInstall = (old.postInstall or "") + ''
                wrapProgram $out/bin/pyright \
                  --prefix PATH : ${pkgs.lib.makeBinPath [ pkgs.nodejs ]}
                wrapProgram $out/bin/pyright-langserver \
                  --prefix PATH : ${pkgs.lib.makeBinPath [ pkgs.nodejs ]}
              '';
            });
          });
        };
      in
      {
        devShells.deploy = pkgs.mkShell {
          packages = with pkgs; [
            # Scripts
            just
            nushell

            # C#
            dotnet-sdk
            dotnet-runtime
            dotnet-aspnetcore
          ];
        };
        devShells.docs = pkgs.mkShell {
          packages = with pkgs; [
            # Scripts
            just
            nushell

            # C#
            dotnet-sdk
            dotnet-runtime
            dotnet-aspnetcore

            # Documentation
            mdbook
            openjdk
            plantuml
            graphviz
            mdbook-plantuml
            pandoc-plantuml-filter
          ];
        };
        devShells.check = pkgs.mkShell {
          packages = with pkgs; [
            # Scripts
            just
            nushell

            # Nix
            nixpkgs-fmt

            # C#
            dotnet-sdk
            dotnet-runtime
            dotnet-aspnetcore

            # Python
            poetry
            pythonEnv
            (mkEnvWrapper pythonEnv "pyright")
            (mkEnvWrapper pythonEnv "pyright-langserver")

            # Markdown
            markdownlint-cli
            nodePackages.markdown-link-check

            # Spelling
            nodePackages.cspell
            hunspell
            hunspellDicts.hr-hr
            hunspellDicts.en-us-large

            # Misc
            nodePackages.prettier
          ];
        };
        devShells.default = pkgs.mkShell {
          PGHOST = "localhost";
          PGPORT = "5432";
          PGDATABASE = "enms";
          PGUSER = "enms";
          PGPASSWORD = "enms";

          DOXYGEN_DOT_PATH = "${pkgs.graphviz}/bin/dot";
          DOXYGEN_PLANTUML_JAR_PATH = "${pkgs.plantuml}/lib/plantuml.jar";

          COMPOSE_PROFILES = "*";

          packages =
            let
              usql = pkgs.writeShellApplication {
                name = "usql";
                runtimeInputs = [ pkgs.usql ];
                text = ''
                  usql \
                    pg://enms:enms@localhost/enms?sslmode=disable \
                    "$@"
                '';
              };

              mermerd = pkgs.writeShellApplication {
                name = "mermerd";
                runtimeInputs = [ pkgs.mermerd ];
                text = ''
                  mermerd \
                    --connectionString postgresql://enms:enms@localhost:5432/enms \
                    "$@"
                '';
              };

              nushell = pkgs.writeShellApplication {
                name = "nu";
                runtimeInputs = [ pkgs.nushell ];
                text = ''
                  nu \
                    --plugins "[ ${pkgs.nushellPlugins.polars}/bin/nu_plugin_polars ]" \
                    "$@"
                '';
              };
            in
            with pkgs;
            [
              # Version Control
              git
              dvc-with-remotes

              # Nix
              nil
              nixpkgs-fmt

              # C#
              dotnet-sdk
              dotnet-runtime
              dotnet-aspnetcore
              omnisharp-roslyn
              netcoredbg

              # Python
              poetry
              pythonEnv
              (mkEnvWrapper pythonEnv "pyright")
              (mkEnvWrapper pythonEnv "pyright-langserver")

              # Markdown
              marksman
              markdownlint-cli
              nodePackages.markdown-link-check

              # PostgreSQL
              usql
              postgresql_14
              mermerd

              # MailHog
              mailhog

              # Spelling
              nodePackages.cspell
              hunspell
              hunspellDicts.hr-hr
              hunspellDicts.en-us-large

              # Scripts
              just
              nushell

              # Documentation
              simple-http-server
              pandoc
              mdbook
              openjdk
              plantuml
              graphviz
              mdbook-plantuml
              pandoc-plantuml-filter

              # Misc
              nodePackages.prettier
              nodePackages.yaml-language-server
              nodePackages.vscode-langservers-extracted
              taplo
            ];
        };

        packages.default =
          pkgs.buildDotnetModule
            rec {
              pname = "enms";
              version = "0.1.0";

              src = self;
              projectFile = "src/Enms.Server/Enms.Server.csproj";
              nugetDeps = ./deps.nix;
              executables = [ "Enms.Server" ];
              makeWrapperArgs = [
                "--set DOTNET_CONTENTROOT ${placeholder "out"}/lib/${pname}"
              ];

              dotnet-sdk = pkgs.dotnet-sdk;
              dotnet-runtime = pkgs.dotnet-aspnetcore;

              meta = {
                description = description;
                homepage = "https://github.com/altibiz/enms";
                license = pkgs.lib.licenses.mit;
              };
            };

        packages.docker = pkgs.dockerTools.buildImage
          {
            name = "altibiz/enms";
            tag = "latest";
            created = "now";
            copyToRoot = pkgs.buildEnv {
              name = "image-root";
              paths = [ self.packages.${system}.default ];
              pathsToLink = [ "/bin" ];
            };
            config = {
              Cmd = [ "enms" ];
            };
          };
      });
}
