$(function () {
    $('#startDatepicker').datepicker({
        todayHighlight: true,
        orientation: "bottom"
    });

    $('#endDatepicker').datepicker({
        todayHighlight: true,
        orientation: "bottom"
    });

    $('#county').select2({
        placeholder: "Select the election's county"
    });
});


