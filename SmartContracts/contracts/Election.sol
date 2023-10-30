// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.8.0;

contract Election {
    //Election properties
    mapping(uint => bool) public voters;

    mapping(uint => uint) public votes;

    mapping(uint => uint) public candidatesVotesCount;
    mapping(uint => bool) public candidates;

    uint votesCount;

    //Is the election is ongoing, changes to the election must not be made (eg. description or candidates of the election)
    bool public IsUpcomingElection;

    //Test to see if is cheaper to use a list of voters as a parameters and a loop to add the voters 
    //instead using a single voterId and made too many transactions
    function addVoter(uint voterId) public {
        require(IsUpcomingElection);
        
        voters[voterId] = true;
    }

    function addVoters(uint[] memory votersList) public {
        require(IsUpcomingElection);

        uint lenght = votersList.length;

        for(uint i = 0; i < lenght; i++)
        {
            voters[i] = true;
        }
    }

    function addCandidate(uint candidateId) public {
        require(IsUpcomingElection);
        
        candidatesVotesCount[candidateId] = 0;
    }

    function castVote(uint voterId, uint candidateId) public {
        //The election should be ongoing in order to voter
        require(!IsUpcomingElection);

        //The voter has to be in the appropriate voters list
        require(voters[voterId]);

        //The candidate has to be in the candidates list for the current election
        require(candidatesVotesCount[candidateId] >= 0);

        //If the voter has already voted, he cannot vote again
        require(votes[voterId] != 0);
        
        votes[voterId] = candidateId;

        candidatesVotesCount[candidateId]++;

        votesCount++;
    }

    //Get the votes of the user
    function getUserVote(uint voterId) public view returns (uint, uint){
        require(votes[voterId] >= 0, "Key does not exists");

        return (voterId, votes[voterId]);
    }

    //Check if the user has already voted (the voter is in the votes mapping)
    function checkUserHasVoted(uint voterId) public view returns (bool) {
        require(votes[voterId] !=0 , "The user has already voted");

        return true;
    }

    //If the election change the state from upcoming to ungoing, set the IsUpcomingElection to true and do not let the users to made any other changes to the contract
    function changeElectionState() public{
        IsUpcomingElection = true;
    }
}