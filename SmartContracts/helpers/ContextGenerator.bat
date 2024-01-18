@echo compile...

set verifierZokPath=%1

CALL zokrates compile -i "%verifierZokPath%"

@echo perform the setup phase...
CALL zokrates setup

@echo export-verifier...
CALL zokrates export-verifier

@echo JOB DONE