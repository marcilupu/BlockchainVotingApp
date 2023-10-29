/**
 * @desc This component is responsible with managing the election. Within this component the add candidate engine is handled.
 * */
const ElectionPageComponent = function () {
    const context = {
        ids: {
            addCandidateModal: {
                initiatorButtonAttr: '[data-create-candidate]',
                dataElectionIdAttr: 'data-election-id',
                target: "#addCandidateModal",
                form: "#addCandidateForm"
            }
        },
        apis: {
            addCandidate: '/Manage/Candidates/AddCandidate',
            listElections: '/Manage/Elections/Index'
        }
    }

    /**
     * Responsible with opening the modal and send the post request to the server in order to add a new candidate.
     * */
    const addCandidateEngine = function () {
        const internalContext = {
            target: $(context.ids.addCandidateModal.target),
            form: $(context.ids.addCandidateModal.form),
            electionId: null,
            candidatesTable: {
                target: '[data-candidates-table]'
            }
        }

        const openModal = function (electionId) {
            internalContext.electionId = electionId;
            internalContext.target.modal("show");
        }

        const save = function () {
            if (internalContext.electionId != null) {
                // Prevent closing the modal until the request is sent
                internalContext.target.block({
                    message: '<h1>Saving...</h1>',
                    css: { border: '3px #a00' }
                });

                $.ajax({
                    url: context.apis.addCandidate,
                    type: 'POST',
                    data: internalContext.form.serialize() + '&ElectionId=' + internalContext.electionId,
                    success: function () {
                        internalContext.target.modal("hide");
                        location.href = context.apis.listElections;
                    },
                    complete: function () {
                        //unlock the modal
                        internalContext.target.unblock();
                    }
                });
            }
        }

        const init = function () {
            $(internalContext.candidatesTable.target).DataTable();

            internalContext.target.find('.submit-usage').on('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                save();
            });

            internalContext.target.on('hidden.bs.modal', function () {
                //Reset the values of the modal form
                internalContext.electionId = null;
                internalContext.form.trigger('reset');
            });
        }

        return {
            init: init,
            openModal: openModal
        }
    }();

    const init = function () {
        addCandidateEngine.init();

        $(context.ids.addCandidateModal.initiatorButtonAttr).on('click', function (e) {
            electionId = $(this).attr(context.ids.addCandidateModal.dataElectionIdAttr);
            addCandidateEngine.openModal(electionId);
        });
    }

    return {
        init: init
    }
}();

$(function () {
    ElectionPageComponent.init();
});

