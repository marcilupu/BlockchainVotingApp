const fs = require('fs');

const ABIfilePath = 'metadata/contracts_Election_sol_Election.abi';
const bytecodefilePath = 'metadata/contracts_Election_sol_Election.bin';

const contract = JSON.parse(fs.readFileSync('build/contracts/Election.json', 'utf8'));

const abiFs = require('fs'); 
const bytecodeFs = require('fs');

const abi = JSON.stringify(contract.abi);
const bytecode = JSON.stringify(contract.bytecode);

abiFs.writeFile(ABIfilePath, abi, (err) => {
  if (err) {
    console.error('Error writing ABI to the file:', err);
  } else {
    console.log('ABI has been written to the file successfully.');
  }
});

bytecodeFs.writeFile(bytecodefilePath, bytecode, (err) => {
    if (err) {
      console.error('Error writing ABI to the file:', err);
    } else {
      console.log('ABI has been written to the file successfully.');
    }
  });






// const fs = require('fs');
// const contract = JSON.parse(fs.readFileSync('./build/contracts/Election.json', 'utf8'));
// console.log(JSON.stringify(contract.bytecode));

// const abiFs = require('fs'); 
// const bytecodeFs = require('fs');

// const abi = JSON.stringify(contract.abi);
// const bytecode = JSON.stringify(contract.bytecode);

// const ABIfilePath = 'D:/Master/Dizertatie/workspace/BlockchainVotingApp/BlockchainVotingApp/BlockchainVotingApp.SmartContract/Contracts/contracts_Election_sol_Election.abi';
// const bytecodefilePath = 'D:/Master/Dizertatie/workspace/BlockchainVotingApp/BlockchainVotingApp/BlockchainVotingApp.SmartContract/Contracts/contracts_Election_sol_Election.bin';

// abiFs.writeFile(ABIfilePath, abi, (err) => {
//   if (err) {
//     console.error('Error writing ABI to the file:', err);
//   } else {
//     console.log('ABI has been written to the file successfully.');
//   }
// });

// bytecodeFs.writeFile(bytecodefilePath, bytecode, (err) => {
//     if (err) {
//       console.error('Error writing ABI to the file:', err);
//     } else {
//       console.log('ABI has been written to the file successfully.');
//     }
//   });