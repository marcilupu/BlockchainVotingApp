const fs = require('fs');

const ABIfilePath = 'metadata/contracts_sol.abi';
const bytecodefilePath = 'metadata/contracts_sol.bin';

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

 