// GET: BusinessProcess/Get/5
$('#suiteTestApplicationId').on('change', function () {
    var testApplicationId = $(this).val();

    customSwal.fire({
        title: 'Loading business processes...',
        html: '<div style="min-height:150px"><div class="spinner"></div></div>',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showConfirmButton: false,
        showCancelButton: false,
        showCloseButton: false,
        didOpen: () => {
            setTimeout(function () {
                $.getJSON('/Home/BusinessProcesses', { id: testApplicationId }, function (data) {
                    var row = '';
                    $('#suiteBusinessProcessId').empty();
                    row += '<option value="" disabled selected>Select business process</option>';
                    $.each(data, function (i, t) {
                        row += '<option value=' + t.businessProcessId + '>' + t.name + '</option>';
                    });
                    $('#suiteBusinessProcessId').html(row);

                    if (data.length == 0)
                        customSwal.fire('Warning', 'This test application hasn\'t business processes!', 'warning');
                    else {
                        $('#suiteBusinessProcessId').prop('disabled', false);
                        customSwal.close();
                    }
                });
            }, 2000);
        }
    });
});