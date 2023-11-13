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
            },
            editElectionModal: {
                initiatorButton: '[data-edit-election]',
                dataEditElectionId: 'data-edit-election-id',
                target: '#editElectionModal',
                form: '#editElectionForm',
                nameInput: '#editElectionName',
                contractAddressInput: '#editContractAddress',
                editStartDate: '#editStartDate',
                editEndDate: '#editEndDate',
                editElectionRules: '#editElectionRules',
                editCountySelect: '#editCounty',
            },
            deleteElectionModal: {
                initiatorButton: '[data-delete-election]',
                dataDeleteElectionId: 'data-delete-election-id',
                target: 'deleteElectionModal'
            }
        },
        apis: {
            addCandidate: '/Manage/Candidates/AddCandidate',
            listElections: '/Manage/Elections/Index',
            editElectionUrl: '/Manage/Elections/Edit',
            deleteElectionUrl: '/Manage/Elections/Delete',
            editCandidateUrl: '/Manage/Candidates/Edit'
        }
    }

    const editElectionEngine = function () {
        const internalContext = {
            editElectionModal: {
                target: $(context.ids.editElectionModal.target),
                form: $(context.ids.editElectionModal.form),
                nameInput: $(context.ids.editElectionModal.nameInput),
                contractAddressInput: $(context.ids.editElectionModal.contractAddressInput),
                editStartDate: $(context.ids.editElectionModal.editStartDate),
                editEndDate: $(context.ids.editElectionModal.editEndDate),
                editElectionRules: $(context.ids.editElectionModal.editElectionRules),
                editCountySelect: $(context.ids.editElectionModal.editCountySelect),
            },
            electionEditId: null
        }

        const openEditModal = function (electionEditId) {

            let card = $('[data-election-id=' + electionEditId + ']');
            internalContext.electionEditId = electionEditId;

            let election = {
                id: electionEditId,
                name: card.find('[data-election-name]').val(),
                contractAddress: card.find('[data-election-contract]').val(),
                startDate: card.find('[data-election-start-date]').val(),
                endDate: card.find('[data-election-end-date]').val(),
                rules: card.find('[data-election-rules]').val(),
                county: card.find('[data-election-county]').val()
            };

            internalContext.editElectionModal.nameInput.val(election.name);
            internalContext.editElectionModal.contractAddressInput.val(election.contractAddress);
            internalContext.editElectionModal.editStartDate.val(election.startDate);
            internalContext.editElectionModal.editEndDate.val(election.endDate);
            internalContext.editElectionModal.editElectionRules.val(election.rules);


            internalContext.editElectionModal.editCountySelect.select2({
                dropdownParent: internalContext.editElectionModal.target,
                placeholder: election.county && election.county != '' ? election.county : 'Not selected',
                width: '100%'
            });

            internalContext.editElectionModal.target.modal('show');
        }

        const editElection = function () {
            //Prevent closing the modal until the request is sent
            internalContext.editElectionModal.target.block({
                message: '<h1>Editing...</h1>',
                css: { border: '3px #a00' }
            });

            $.ajax({
                url: context.apis.editElectionUrl,
                type: 'POST',
                data: internalContext.editElectionModal.form.serialize() + '&electionId=' + internalContext.electionEditId,
                success: function () {
                    internalContext.editElectionModal.target.modal("hide");
                    location.href = context.apis.listElections;
                },
                complete: function () {
                    //unlock the modal
                    internalContext.editElectionModal.target.unblock();
                }
            });
        }

        const init = function () {
            $('#editStartDatepicker').datepicker({
                todayHighlight: true,
                orientation: "bottom"
            });

            $('#editEndDatepicker').datepicker({
                todayHighlight: true,
                orientation: "bottom"
            });

            $('#editCounty').select2({
                placeholder: "Select the election's county"
            });

            internalContext.editElectionModal.target.find('.submit-edit-election').on('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                editElection();
            });
        }

        return {
            init: init,
            openEditModal: openEditModal
        }
    }();

    const deleteElectionEngine = function () {
        internalContext = {
            deleteElectionModal: {
                target: $("#" + context.ids.deleteElectionModal.target)
            },
            electionId: null
        }

        const openDeleteModal = function (electionId) {
            internalContext.electionId = electionId;

            internalContext.deleteElectionModal.target.modal("show");
        }

        const deleteElection = function () {
            $.ajax({
                url: context.apis.deleteElectionUrl,
                method: 'POST',
                data: {
                    id: internalContext.electionId
                },
                success: function () {
                    internalContext.deleteElectionModal.target.modal('hide');
                    location.href = context.apis.listElections
                }
            });
        }

        const init = function () {
            internalContext.deleteElectionModal.target.find('.submit-delete-election').on('click', function (event) {
                event.preventDefault();
                event.stopPropagation();

                deleteElection();
            });
        }

        return {
            init: init,
            openDeleteModal: openDeleteModal
        };
    }();

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
                        location.href = context.apis.listElections
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
        editElectionEngine.init();
        deleteElectionEngine.init();

        $(context.ids.addCandidateModal.initiatorButtonAttr).on('click', function (e) {
            electionId = $(this).attr(context.ids.addCandidateModal.dataElectionIdAttr);
            addCandidateEngine.openModal(electionId);
        });

        $(context.ids.editElectionModal.initiatorButton).on('click', function (e) {
            electionId = $(this).attr(context.ids.editElectionModal.dataEditElectionId);
            editElectionEngine.openEditModal(electionId)
        });

        $(context.ids.deleteElectionModal.initiatorButton).on('click', function (e) {
            electionId = $(this).attr(context.ids.deleteElectionModal.dataDeleteElectionId);
            deleteElectionEngine.openDeleteModal(electionId);
        });
    }

    return {
        init: init
    }
}();

$(function () {
    ElectionPageComponent.init();
});

