$(document).ready(function () {
    $('#FechaInicioBtn').click(function () {
        $('#FechaInicio').datepicker('show');
    });

    $('#FechaFinalBtn').click(function () {
        $('#FechaFinal').datepicker('show');
    });

    $('#FechaInicio').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true,
        toggleActive: true,
        onSelect: function (formattedDate) {
            $('#FechaInicio').val(formattedDate);
        }
    });

    $('#FechaFinal').datepicker({
        format: 'dd/mm/yyyy',
        autoclose: true,
        todayHighlight: true,
        clearBtn: true,
        toggleActive: true,
        onSelect: function (formattedDate) {
            $('#FechaFinal').val(formattedDate);
        }
    });
});