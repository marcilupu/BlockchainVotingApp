:: Bat input
:: -- %1 - represents the context identifier (directory where verifier is generated)
:: -- %2 - represents the proof identifier (it will be used to name the witness and proof file).
:: -- %3 - represents the witness (userId)

set "verifierContext=%1/verifier"
set "witnessPath=%2.witness"
set "proofPath=%2"

cd "%verifierContext%"


CALL zokrates compute-witness -a %3 -o "%witnessPath%"

@echo Witness generated

CALL zokrates generate-proof -w "%witnessPath%" -j "%proofPath%"

@echo Proof generated