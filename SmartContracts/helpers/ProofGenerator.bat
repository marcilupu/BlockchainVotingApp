set "electionVerifierDirPath=%1"
set "arguments=%2"
set "proofPath=%3"

cd "%electionVerifierDirPath%"

CALL zokrates compute-witness -a %arguments% -o "%electionVerifierDirPath%\witness"

CALL zokrates generate-proof

xcopy "%electionVerifierDirPath%\proof.json" "%proofPath%"