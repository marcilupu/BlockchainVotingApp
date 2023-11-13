/**
 * @desc This component is responsible with managing the election. Within this component the add candidate engine is handled.
 * */
const EditCandidatePage = function () {
    const context = {
        ids: {
            editCandidateModal: {
                initiatorButton: '[data-edit-candidate]',
                dataEditCandidateId: 'data-edit-candidate-id',
                target: 'editCandidateModal',
                form: 'editCandidateForm',
                firstName: '[data-first-name]',
                lastName: '[data-last-name]',
                organization: '[data-organization]',
                biography: '[data-biography]'
            },
            deleteCandidateModal: {
                initiatorButton: '[data-delete-candidate]',
                dataDeleteCandidateId: 'data-delete-candidate-id',
                target: 'deleteCandidateModal',
            }
        },
        apis: {
            listElections: '/Manage/Elections/Index',
            editCandidateUrl: '/Manage/Candidates/Edit',
            deleteCandidateUrl: '/Manage/Candidates/Delete'
        }
    }

    const editCandidateEngine = function () {
        const internalContext = {
            editCandidateModal: {
                target: $("#" + context.ids.editCandidateModal.target),
                form: $("#" + context.ids.editCandidateModal.form)
            },
            candidateId: null
        };

        const openEditModal = function (candidateId) {
            internalContext.candidateId = candidateId;
            let card = $('[data-candidate-container-id=' + candidateId + ']');

            let candidate = {
                id: candidateId,
                name: card.find('[data-candidate-name]').attr('data-candidate-name'),
                organization: card.find('[data-candidate-organization]').attr('data-candidate-organization'),
                biography: card.find('[data-candidate-biography]').attr('data-candidate-biography')
            };

            const names = candidate.name.split(" ");
            internalContext.editCandidateModal.form.find(context.ids.editCandidateModal.firstName).val(names[0]);
            internalContext.editCandidateModal.form.find(context.ids.editCandidateModal.lastName).val(names[1]);
            internalContext.editCandidateModal.form.find(context.ids.editCandidateModal.organization).val(candidate.organization);
            internalContext.editCandidateModal.form.find(context.ids.editCandidateModal.biography).val(candidate.biography);

            internalContext.editCandidateModal.target.modal('show');
        }

        const edit = function () {
            internalContext.editCandidateModal.target.block({
                message: '<h1>Editing...</h1>',
                css: { border: '3px #a00' }
            });

            $.ajax({
                url: context.apis.editCandidateUrl,
                type: 'POST',
                data: internalContext.editCandidateModal.form.serialize() + '&candidateId=' + internalContext.candidateId,
                success: function () {
                    internalContext.editCandidateModal.target.modal("hide");
                    location.href = context.apis.listElections;
                },
                complete: function () {
                    internalContext.editCandidateModal.target.unblock();
                }
            });
        }

        const init = function () {
            internalContext.editCandidateModal.target.find('.submit-edit-candidate').on('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                edit();
            });
        };

        return {
            init: init,
            openEditModal: openEditModal
        };
    }()

    const deleteCandidateEngine = function () {
        const internalContext = {
            deleteCandidateModal: {
                target: $('#' + context.ids.deleteCandidateModal.target),
            },
            candidateId: null
        }

        const openDeleteModal = function (candidateId) {
            internalContext.candidateId = candidateId;

            internalContext.deleteCandidateModal.target.modal("show");
        }

        const deleteCandidate = function () {
            $.ajax({
                url: context.apis.deleteCandidateUrl,
                method: 'POST',
                data: {
                    id: internalContext.candidateId
                },
                success: function () {
                    internalContext.deleteCandidateModal.target.modal('hide');
                    location.href = context.apis.listElections
                }
            });
        }

        const init = function () {
            internalContext.deleteCandidateModal.target.find('.submit-delete-candidate').on('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                deleteCandidate();
            });
        }

        return {
            init: init,
            openDeleteModal: openDeleteModal
        }
    }();

    const init = function () {
        editCandidateEngine.init();
        deleteCandidateEngine.init();

        $(context.ids.editCandidateModal.initiatorButton).on('click', function (e) {
            candidateId = $(this).attr(context.ids.editCandidateModal.dataEditCandidateId);
            editCandidateEngine.openEditModal(candidateId);
        });

        $(context.ids.deleteCandidateModal.initiatorButton).on('click', function (e) {
            candidateId = $(this).attr(context.ids.deleteCandidateModal.dataDeleteCandidateId);
            deleteCandidateEngine.openDeleteModal(candidateId);
        });
    }

    return {
        init: init
    }
}();

$(function () {
    EditCandidatePage.init();
});

