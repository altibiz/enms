# Izvještaj o ENMS projektu

Ovo je izvještaj o ENMS projektu. Podijeljen je na tri dijela.

## Razvojno okruženje

Razvojno okruženje prikazuje alate i tehnologije korištene u ENMS projektu.
Ukratko, ENMS projekt je full stack web aplikacija koja koristi PostgreSQL kao
bazu podataka, ASP.NET Core kao backend i Blazor kao frontend. Razvojno
okruženje također uključuje konfiguracije različitih alata koje koriste
programeri kao što su Visual Studio Code, Helix, Just i Nix. Konačno, projekt je
hostan na GitHubu i koristi GitHub akcije za kontinuiranu integraciju na Azure
cloud app servisu koji je povezan s PostgreSQL bazom podataka.

## Backend

Backend dio izvještaja opisuje serversku stranu ENMS projekta. Backend je
RESTful API koji koristi `Enms.Data` projekt za interakciju s bazom podataka
putem `EntityFrameworkCore` i `Enms.Business` projekt za implementaciju poslovne
logike. Server također koristi `Enms.Server` projekt kao startup projekt.
Backend također koristi `OrchardCore` biblioteku za autentikaciju, autorizaciju
i upravljanje korisnicima.

## Frontend

Frontend dio izvještaja opisuje klijentsku stranu ENMS projekta. Frontend je
Blazor projekt koji koristi `Enms.Client` projekt za pružanje korisničkog
sučelja.
