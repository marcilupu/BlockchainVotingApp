//Starting with the pragma solidity version
pragma solidity >=0.4.22 <0.8.0;

contract Election {
	// Model a Candidate
	struct Candidate {
		uint id;
		string name;
		uint voteCount;
	}

	event votedEvent (
        uint indexed _candidateId
    );

	// Store account that have voted
	// this is simple going to take an account and give a boolean is the account has voted
	mapping(address => bool) public voters;

	// Store Candidates
	// Fetch Candidates
	// candidateId => candidate structure type
	mapping(uint => Candidate) public candidates;

	// Store Candidates count
	uint public candidatesCount;

	// Constructor
	constructor() public {
		addCandidate("Candidate 1");
		addCandidate("Candidate 2"); 
	} 

	function addCandidate (string memory _name) private {
		candidatesCount ++;
		candidates[candidatesCount] = Candidate(candidatesCount, _name, 0);
	}

	function vote (uint _candidateId) public {
		//require that they haven't voted before
		//if the statement is false, the require() will return false and exist the function
		require(!voters[msg.sender]);

		//require a valid candidate
		require(_candidateId > 0 && _candidateId <= candidatesCount);

		// record that the voter has voted
		// With msg.sender I read the account which voted
		voters[msg.sender] = true;

		//update candidate vote count
		candidates[_candidateId].voteCount ++;

		// trigger voted event
    	emit votedEvent(_candidateId);
	}
}