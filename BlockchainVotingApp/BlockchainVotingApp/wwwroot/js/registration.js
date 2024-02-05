const RegistrationComponent = function () {
    const context = {
        modal: null,
        ids: {
            voteModal: {
                initiatorButton: '[data-register]',
                initiatorButtonElectionId: 'data-register-election-id',
                target: '#registerModal',
                content: '#registerModalContent',
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
            proofModal: '/Register/GetProofModal',
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
            init: function () {
                this.on("sending", function (file, xhr, formData) {

                    Swal.fire({
                        title: 'Registering...',
                        html: 'Please wait...',
                        didOpen: () => {
                            Swal.showLoading()
                        },
                        allowOutsideClick: false
                    });

                    formData.append("electionId", context.state.electionId);
                });
            },
            success: function (file, response) {
                context.state.proofContent = response.content

                Swal.fire({
                    title: 'Good job!',
                    text: 'Congratulations, you have successfully registered for this election and you can vote now!',
                    icon: 'success',
                    buttonsStyling: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    }
                })

                location.reload();
            },
            error: function (result) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Your proof is wrong or you have already registered',
                    buttonsStyling: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    }
                })
                context.modal.modal("hide");
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
    RegistrationComponent.init();
})