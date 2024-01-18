@echo Create election infrastructure...
set "rootPath=%1"
set "individualElectionVerifierPath=%2"
set "generatedABIDirPath=%3"

:: move verifier.sol file to 
move "%rootPath%\verifier.sol" "%rootPath%\contracts\Verifier.sol"

mkdir "%rootPath%\%individualElectionVerifierPath%"
:: move out, out.r1cs
:: verification.key
:: proving.key 
:: in Verifier folder
move "%rootPath%\out" "%rootPath%\%individualElectionVerifierPath%"
move "%rootPath%\out.r1cs" "%rootPath%\%individualElectionVerifierPath%"
move "%rootPath%\proving.key" "%rootPath%\%individualElectionVerifierPath%"
move "%rootPath%\verification.key" "%rootPath%\%individualElectionVerifierPath%"
move "%rootPath%\abi.json" "%rootPath%\%individualElectionVerifierPath%"

:: compile election
@echo Deploy smart contract...
CALL truffle migrate --reset --compile-all 

@echo Generate new abi and bytecode files...
CALL solcjs --bin --abi --include-path node_modules/ --base-path . contracts/Election.sol -o "%generatedABIDirPath%"

@echo JOB DONE