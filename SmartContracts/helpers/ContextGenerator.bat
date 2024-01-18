@echo Compile...

set "contextIdentifier=%1"

set "verifierContext=%1/verifier/"

cd "%verifierContext%"

CALL zokrates compile -i "Verifier.zok" -s "abi.json" -o "out" -r "out.r1cs"

@echo Perform the setup phase...
CALL zokrates setup

@echo Export verifier...
CALL zokrates export-verifier -o "../contracts/Verifier.sol"

cd "../"
:: compile election
@echo Deploy smart contract...

CALL truffle compile --all 

CALL truffle migrate --f 3

@echo Generate new abi and bytecode files...
::CALL solcjs --bin --abi --include-path "../node_modules/" --base-path . "contracts/Election.sol" -o "metadata"
node "../helpers/GenerateABIandBytecode.js"

@echo JOB DONE