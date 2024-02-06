const AddElectionPage = function () {
    const context = {
        ids: {
            form: '#addForm',
            button: '#addButton',
            startDatePicker: '#startDatepicker',
            endDatePicker: '#endDatepicker',
            county: '#county'
        },
        apis: {
            add: '/manage/elections/createElection',
            index: '/manage/elections/index'
        },
        jquery: {
            form: null,
            button: null,
            startDatePicker: null,
            endDatePicker: null,
            county: null
        }
    }

    const showLoader = function () {
        Swal.fire({
            title: 'Creating election',
            html: 'Please wait, election creation is in progress...',
            didOpen: () => {
                Swal.showLoading()
            },
            allowOutsideClick: false
        });
    }

    const save = function () {

        showLoader();

        $.ajax({
            url: context.apis.add,
            method: 'POST',
            data: context.jquery.form.serialize(),
            success: function () {
                Swal.fire({
                    title: 'Good job!',
                    text: 'The election has been successfully generated! You will be redirected to election view',
                    icon: 'success',
                    buttonsStyling: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    }
                })

                // Apply a timeout, then redirect the user to the elections view.
                setTimeout(() => location.href = context.apis.index, 3000);
            },
            error: function (message) {
                Swal.fire({
                    title: 'Creation failed',
                    text: message.responseText ?? 'Failed to create a new election',
                    icon: 'error',
                    buttonsStyling: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    }
                })
            }
        });
    }

    const init = function () {
        context.jquery.startDatePicker = $(context.ids.startDatePicker);
        context.jquery.endDatePicker = $(context.ids.endDatePicker)
        context.jquery.county = $(context.ids.county)
        context.jquery.form = $(context.ids.form);
        context.jquery.button = $(context.ids.button);

        context.jquery.startDatePicker.datepicker({
            todayHighlight: true,
            orientation: "bottom"
        });

        context.jquery.endDatePicker.datepicker({
            todayHighlight: true,
            orientation: "bottom"
        });

        context.jquery.county.select2({
            placeholder: "Select the election's county"
        });

        context.jquery.button.on('click', function () {
            event.preventDefault();

            save();
        });
    }

    return {
        init: init
    }
}();


$(function () {
    AddElectionPage.init();
});


