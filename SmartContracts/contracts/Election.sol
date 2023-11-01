//Because Ethereum is an open source project, the first line shows the contract’s open source license.
//The second line specifies the Solidity version necessary to execute this contract.

// SPDX-License-Identifier: GPL-3.0
pragma solidity >=0.8.0;

contract Election {
    //Election properties
    mapping(uint => bool) public voters;

    uint votesCount;
    mapping(uint => uint) public votes;
    mapping(uint => bool) public usersVoted;

    mapping(uint => uint) public candidatesVotesCount;
    mapping(uint => bool) public candidates;

    //Is the election is ongoing, changes to the election must not be made (eg. description or candidates of the election)
    bool public IsUpcomingElection = true;

    //Test to see if is cheaper to use a list of voters as a parameters and a loop to add the voters 
    //instead using a single voterId and made too many transactions
    function addVoter(uint voterId) public {
        require(voterId != 0, "The voterId is null");

        require(IsUpcomingElection, "The election should be upcoming in order to be changed");

        //Do not add a voter if he has been already added
        require(!voters[voterId], "The voter has already been added");
        
        voters[voterId] = true;
    }

    function addVoters(uint[] memory votersList) public {
        require(IsUpcomingElection);

        uint lenght = votersList.length;

        for(uint i = 0; i < lenght; i++)
        {
            addVoter(votersList[i]);
        }
    }

    function addCandidate(uint candidateId) public {
        require(candidateId != 0, "The candidate id is null");

        require(IsUpcomingElection, "The election should be upcoming in order to be changed");

        require(!candidates[candidateId], "The candidate has already been added");
        
        candidatesVotesCount[candidateId] = 0;
        candidates[candidateId] = true;
    }

    function castVote(uint voterId, uint candidateId) public {
        require(voterId != 0, "The voterId is null");
        require(candidateId != 0, "The candidateId is null");

        //The election should be ongoing in order to voter
        require(!IsUpcomingElection, "The election is still upcoming, you cannot vote!");

        //The voter has to be in the appropriate voters list
        require(voters[voterId], "The voter has to be in the election list");

        //The candidate has to be in the candidates list for the current election
        require(candidatesVotesCount[candidateId] >= 0, "The candidate has to be in the candidatesVotesCount list");
        require(candidates[candidateId], "The candidate has to be in the election candidates list");

        //If the voter has already voted, he cannot vote again
        require(!usersVoted[voterId], "The user can vote only once");
        
        votes[voterId] = candidateId;

        usersVoted[voterId] = true;

        candidatesVotesCount[candidateId]++;

        votesCount++;
    }

    //Get the votes of the user
    function getUserVote(uint voterId) public view returns (uint VoterId, uint CandidateId){
        //Check if user exists
        require(voterId != 0, "The voterId is null");
        
        require(usersVoted[voterId], "Key does not exists. The user has not voted");

        return (voterId, votes[voterId]);
    }

    //Check if the user has already voted (the voter is in the votes mapping)
    //The function returns true if the voter has already voted, than returns false
    function checkUserHasVoted(uint voterId) public view returns (bool) {
        require(voterId != 0, "The voterId is null");

        if(votes[voterId] > 0 && usersVoted[voterId])
            return true;
        else
            return false;
    }

    //If the election change the state from upcoming to ungoing, set the IsUpcomingElection to true and do not let the users to made any other changes to the contract
    function changeElectionState(bool electionState) public{
        IsUpcomingElection = electionState;
    }
}