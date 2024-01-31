//Because Ethereum is an open source project, the first line shows the contract’s open source license.
//The second line specifies the Solidity version necessary to execute this contract.

import './Verifier.sol';

// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.8.0;

contract Election {
    Verifier public verifier;

    constructor(){
        verifier = new Verifier();
    }

    uint public votesCount;
    mapping(uint256 => uint) public votes;
    mapping(uint256 => bool) public usersVoted;

    mapping(uint => uint) public candidatesVotesCount;
    mapping(uint => bool) public candidates;

    //Is the election is ongoing, changes to the election must not be made (eg. description or candidates of the election)
    bool public IsUpcomingElection = true;

    function addCandidate(uint candidateId) public {
        require(candidateId != 0, "The candidate id is null");

        require(IsUpcomingElection, "The election should be upcoming in order to be changed");

        require(!candidates[candidateId], "The candidate has already been added");
        
        candidatesVotesCount[candidateId] = 0;
        candidates[candidateId] = true;
    }

    function getCandidateResults(uint candidateId) public view returns (uint result){
        require(candidateId != 0, "The candidate id is null");
        require(candidatesVotesCount[candidateId] >= 0, "The candidate has to be in the candidatesVotesCount list");
        require(candidates[candidateId], "The candidate has to be in the election candidates list");
        
        return candidatesVotesCount[candidateId];
    }

    function castVote(uint256 ax, uint256 ay, uint256 bx0, uint256 bx1, uint256 by0, uint256 by1, uint256 cx, uint256 cy, uint256 proofSha, uint candidateId) public returns (bool){
        //todo: check the other args
        require(candidateId != 0, "The candidateId is null");

        //The election should be ongoing in order to voter
        require(!IsUpcomingElection, "The election is still upcoming, you cannot vote!");
 
        //The candidate has to be in the candidates list for the current election
        require(candidatesVotesCount[candidateId] >= 0, "The candidate has to be in the candidatesVotesCount list");
        require(candidates[candidateId], "The candidate has to be in the election candidates list");

        require(!usersVoted[proofSha], "The user can vote only once");

        // Use ZKP to proove the voter is in the appropriate voters list
        //Verifier.Proof memory proof= getProof(ax, ay, bx0, bx1, by0, by1, cx, cy);
            
        // try verifier.verifyTx(proof) 
        // {        
        //     votes[proofSha] = candidateId;
        //     usersVoted[proofSha] = true;

        //     candidatesVotesCount[candidateId]++;
        //     votesCount++;
        //     return true;
        // }
        // catch
        // {
        //     require(false, "The voter is invalid");
        //     return false;
        // }
    }

    //Get the votes of the user
    function getUserVote(uint256 proofSha) public view returns (uint CandidateId){
        require(usersVoted[proofSha], "Key does not exists. The user has not voted");
        return votes[proofSha];
    }

    //Check if the user has already voted (the voter is in the votes mapping)
    //The function returns true if the voter has already voted, than returns false
    // the purpose of this function will be modified, the user has to introduce its proof in order to check if he has voted...
    // function checkUserHasVoted(uint256 proofSha) public view returns (bool) {
    //     require(proofSha != 0, "The voterId is null");

    //     if(votes[proofSha] > 0 && usersVoted[proofSha])
    //         return true;
    //     else
    //         return false;
    // }

    //If the election change the state from upcoming to ungoing, set the IsUpcomingElection to true and do not let the users to made any other changes to the contract
    function changeElectionState(bool electionState) public{
        IsUpcomingElection = electionState;
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