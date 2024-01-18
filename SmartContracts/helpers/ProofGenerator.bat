set "contextIdentifier=%1"
set "arguments=%2"

set "verifierContext=%1/verifier/"

cd "%verifierContext%"

cd "%electionVerifierDirPath%"

CALL zokrates compute-witness -a %arguments% -o "%electionVerifierDirPath%\witness"

CALL zokrates generate-proof