//Because Ethereum is an open source project, the first line shows the contract’s open source license.
//The second line specifies the Solidity version necessary to execute this contract.

import './Verifier.sol';

// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.8.0;

contract Registration {
    Verifier public verifier;

    constructor(){
        verifier = new Verifier();
    }
 
    function register(uint256 ax, uint256 ay, uint256 bx0, uint256 bx1, uint256 by0, uint256 by1, uint256 cx, uint256 cy) public view returns (bool){
        //todo: check the args

        // Use ZKP to proove the voter is in the appropriate voters list
        Verifier.Proof memory proof= getProof(ax, ay, bx0, bx1, by0, by1, cx, cy);
            
        try verifier.verifyTx(proof) 
        {
            return true;
        }
        catch
        {
            require(false, "The voter is invalid");
            return false;
        }
    }
 
    //private functions
    function getProof(uint256 ax, uint256 ay, uint256 bx0, uint256 bx1, uint256 by0, uint256 by1, uint256 cx, uint256 cy) public pure returns (Verifier.Proof memory proof) {
        Pairing.G1Point memory a;
        Pairing.G2Point memory b;
        Pairing.G1Point memory c;

        a.X = ax;
        a.Y = ay;

        b.X = [bx0,bx1];
        b.Y = [by0,by1];

        c.X = cx;
        c.Y = cy;

        proof.a = a;
        proof.b = b;
        proof.c = c;

        return proof;
    }
}