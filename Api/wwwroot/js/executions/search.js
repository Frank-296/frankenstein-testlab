// GET: BusinessProcess/Get/5
$('#testApplicationId').on('change', function () {
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
                    $('#businessProcessId').empty();
                    row += '<option value="" disabled selected>Select business process</option>';
                    $.each(data, function (i, t) {
                        row += '<option value=' + t.businessProcessId + '>' + t.name + '</option>';
                    });
                    $('#businessProcessId').html(row);

                    if (data.length == 0)
                        customSwal.fire('Warning', 'This test application hasn\'t business processes!', 'warning');
                    else {
                        $('#businessProcessId').prop('disabled', false);
                        customSwal.close();
                    }
                });
            }, 2000);
        }
    });
});

// GET: TestCases/Get/5
$('#businessProcessId').on('change', function () {
    var businessProcessId = $(this).val();

    customSwal.fire({
        title: 'Loading test cases...',
        html: '<div style="min-height:150px"><div class="spinner"></div></div>',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showConfirmButton: false,
        showCancelButton: false,
        showCloseButton: false,
        didOpen: () => {
            setTimeout(function () {
                $.getJSON('/Home/TestCases', { id: businessProcessId }, function (data) {
                    var row = '';
                    $('#testCaseId').empty();
                    row += '<option value="" disabled selected>Select test case</option>';
                    $.each(data, function (i, t) {
                        row += '<option value=' + t.testCaseId + '>' + t.name + '</option>';
                    });
                    $('#testCaseId').html(row);

                    if (data.length == 0) {
                        customSwal.fire('Warning', 'This business process hasn\'t test cases!', 'warning');
                    }
                    else {
                        $('#testCaseId').prop('disabled', false);
                        customSwal.close();
                    }
                });
            }, 2000);
        }
    });
});