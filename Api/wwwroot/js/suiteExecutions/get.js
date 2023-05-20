// GET: SuiteExecutions/Get/5
function Get(id) {
    customSwal.fire({
        title: 'Loading...',
        html: '<div style="min-height:150px"><div class="spinner"></div></div>',
        allowEscapeKey: false,
        allowOutsideClick: false,
        showConfirmButton: false,
        showCancelButton: false,
        showCloseButton: false,
        didOpen: () => {
            setTimeout(function () {
                $.ajax({
                    type: 'GET',
                    url: '/SuiteExecutions/Get/' + id,
                    dataType: 'JSON',
                    success: function (response) {
                        document.getElementById('testApplicationName').innerText = response.businessProcess.testApplication.name;
                        document.getElementById('testApplicationDescription').innerText = response.businessProcess.testApplication.description;
                        document.getElementById('businessProcessName').innerText = response.businessProcess.name;
                        document.getElementById('businessProcessDescription').innerText = response.businessProcess.description;
                        document.getElementById('testCasesExecuted').innerText = response.executions.length;
                        document.getElementById('executionDate').innerText = new Date(response.executionDate).toLocaleString([], { hour12: true });
                        document.getElementById('executionRemarks').innerText = response.remarks;
                        document.getElementById('executor').innerText = response.executor;
                        document.getElementById('company').innerText = response.company;
                        document.getElementById('role').innerText = response.role;
                        document.getElementById('email').innerText = '';
                        document.getElementById('phoneNumber').innerText = '';

                        $('#detailsModal').modal('show');
                        customSwal.close();
                    },
                    error: function () {
                        table.ajax.reload(null, false);
                        customSwal.fire('Incorrect', 'No test suite execution info found!', 'error');
                    }
                });
            }, 2000);
        }
    });
}