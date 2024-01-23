const ElectionComponent = function () {
    const context = {
        modal: null,
        ids: {
            voteModal: {
                initiatorButton: '[data-vote]',
                initiatorButtonElectionId: 'data-vote-election-id',
                target: '#voteModal',
                content: '#voteModalContent',
                candidateSelect: '[data-candidate-select]',
                proofInput: '[data-proof-form]'
            }
        },
        state: {
            jQuery: {
                candidateSelect: null,
                proofInput: null,
            },
            electionId: null,
            proofContent: null
        },
        apis: {
            vote: '/Election/Vote',
            voteModal: '/Election/GetVoteModal'
        }
    }

    /**
     * 
     * */
    const initState = function (formContent) {
        // Get the container of the modal.
        let container = context.modal.find('.modal-body');

        container.html(formContent);

        // Reinitialize jquery for select and form
        context.state.jQuery.candidateSelect = container.find(context.ids.voteModal.candidateSelect);
        context.state.jQuery.proofInput = container.find(context.ids.voteModal.proofInput);

        // Initialise form select and dropzone and assign the values to context.form
        context.state.jQuery.candidateSelect.select2({
            dropdownParent: context.ids.voteModal.content,
            placeholder: "Select the election's candidate",
            width: '100%'
        });

        context.state.jQuery.proofInput.dropzone({
            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 10, // MB
            maxFiles: 1,
            success: function (file, response) {
                context.state.proofContent = response.content
            }
        });
    }

    const lock = function (message) {

        context.modal.block({
            message: '<h1>' + message + '</h1>',
            css: { border: '3px #a00' }
        });

        return () => context.modal.unblock();
    }


    const clearState = function () {
        // Get the container of the modal.
        let container = context.modal.find('.modal-body');

        container.html('');

        // Reset all state variables.
        context.state.jQuery.candidateSelect = null;
        context.state.jQuery.proofInput = null;

        context.state.electionId = null;
        context.state.proofContent = null;
    }

    const open = function (electionId) {
        context.state.electionId = electionId;

        context.modal.modal('show');
    }

    const vote = function () {
        var candidate = context.state.jQuery.candidateSelect.val();
        console.log(candidate);
        console.log(context.state.proofContent);

        //Prevent closing the modal until the request is sent
        const unlock = lock('Voting...');
         
        $.ajax({
            url: context.apis.vote,
            type: 'POST',
            data: 'candidateId=' + candidate + "&proof=" + context.state.proofContent,
            success: function () {
                context.modal.modal("hide");
                //location.href = context.apis.listElections;
            },
            complete: function () {
                //unlock the modal
                unlock();
            }
        });
    }

    const init = function () {
        // Initialise jquery
        context.modal = $(context.ids.voteModal.target);

        // Initialise events
        $(context.ids.voteModal.initiatorButton).on('click', function (e) {
            electionId = $(this).attr(context.ids.voteModal.initiatorButtonElectionId);

            open(electionId);
        });


        context.modal.on('shown.bs.modal', function () {

            const unlock = lock('Loading modal content...');

            // Retrieve the modal content from backend.
            $.ajax({
                url: context.apis.voteModal + '?electionId=' + context.state.electionId,
                type: 'GET',
                success: function (result) {
                    // On succes, fill the modal body and initialise inner components.
                    initState(result);
                },
                complete: function () {
                    unlock();
                }
            });
        });

        context.modal.on('hide.bs.modal', function () {
            clearState();
        });

        context.modal.find('.submit-vote').on('click', function (event) {
            event.preventDefault();
            event.stopPropagation();

            vote();
        });
    }

    return {
        init: init
    }
}();

$(function () {
    ElectionComponent.init();
})