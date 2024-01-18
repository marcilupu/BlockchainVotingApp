@echo Deploy smart contract...
CALL truffle migrate --reset --compile-all 
 
@echo Generate new abi and bytecode files...
::CALL solcjs --bin --abi --include-path node_modules/ --base-path . contracts/Election.sol -o "D:\Master\Dizertatie\workspace\BlockchainVotingApp\BlockchainVotingApp\BlockchainVotingApp.SmartContract\Contracts"
node GenerateABIandBytecode.js

@echo JOB DONE