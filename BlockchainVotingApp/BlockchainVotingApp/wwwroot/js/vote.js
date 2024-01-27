const VoteComponent = function () {
    const context = {
        modal: null,
        ids: {
            voteModal: {
                initiatorButton: '[data-get-vote]',
                initiatorButtonElectionId: 'data-get-vote-id',
                target: '#voteModal',
                content: '#voteModalContent',
                proofInput: '[data-proof-form]'
            }
        },
        state: {
            jQuery: {
                proofInput: null,
            },
            electionId: null,
            proofContent: null
        },
        apis: {
            proofModal: '/Election/GetProofModal',
            voteDetails: '/Election/VoteDetails'
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
        context.state.jQuery.proofInput = container.find(context.ids.voteModal.proofInput);

        // Initialise form select and dropzone and assign the values to context.form
        context.state.jQuery.proofInput.dropzone({
            paramName: "file", // The name that will be used to transfer the file
            maxFilesize: 10, // MB
            maxFiles: 1,
            success: function (file, response) {
                context.state.proofContent = response.content

                location.href = context.apis.voteDetails + "?electionId=" + context.state.electionId + "&proof=" + context.state.proofContent
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
        context.state.jQuery.proofInput = null;

        context.state.electionId = null;
        context.state.proofContent = null;
    }

    const open = function (electionId) {
        context.state.electionId = electionId;

        context.modal.modal('show');
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
                url: context.apis.proofModal,
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
    }

    return {
        init: init
    }
}();

$(function () {
    VoteComponent.init();
})