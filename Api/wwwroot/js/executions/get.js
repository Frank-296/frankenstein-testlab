// GET: Executions/Get/5
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
                    url: '/Executions/Get/' + id,
                    dataType: 'JSON',
                    success: function (response) {
                        document.getElementById('testReportDiv').classList.remove('bg-sky');
                        document.getElementById('testReportDiv').classList.remove('bg-pineapple');
                        document.getElementById('testReportDiv').classList.remove('bg-mammee');
                        document.getElementById('testReportDiv').classList.remove('bg-grass');
                        document.getElementById('executionStatusTd').classList.remove('bg-sky');
                        document.getElementById('executionStatusTd').classList.remove('bg-pineapple');
                        document.getElementById('executionStatusTd').classList.remove('bg-mammee');
                        document.getElementById('executionStatusTd').classList.remove('bg-grass');

                        $('#testDataDiv').hide();
                        $('#defectDiv').hide();

                        switch (response.executionStatus) {
                            case 1:
                                document.getElementById('testReportDiv').classList.add('bg-sky');
                                document.getElementById('executionStatusTd').classList.add('bg-sky');
                                break;

                            case 2:
                                document.getElementById('testReportDiv').classList.add('bg-pineapple');
                                document.getElementById('executionStatusTd').classList.add('bg-pineapple');
                                break;

                            case 3:
                                document.getElementById('testReportDiv').classList.add('bg-mammee');
                                document.getElementById('executionStatusTd').classList.add('bg-mammee');
                                break;

                            case 4:
                                document.getElementById('testReportDiv').classList.add('bg-grass');
                                document.getElementById('executionStatusTd').classList.add('bg-grass');
                                break;
                        }

                        if (document.getElementById('downloadReportButton')) {
                            document.getElementById('downloadReportButton').remove();
                        }

                        $('<button class="btn btn-kiwi" id="downloadReportButton"><i class="fa fa-download"></i> Download report</button>')
                            .appendTo(document.getElementById('downloadReport'))
                            .on('click', function () {
                                var link = document.createElement('a');

                                link.href = 'data:text/html;base64,' + response.testReport;
                                link.download = 'index.html';
                                link.click();
                            }
                        );

                        if (document.getElementById('downloadTestDataButton')) {
                            document.getElementById('downloadTestDataButton').remove();
                        }

                        if (response.testData != null) {
                            $('<button class="btn btn-kiwi" id="downloadTestDataButton"><i class="fa fa-download"></i> Download test data</button>')
                                .appendTo(document.getElementById('downloadTestData'))
                                .on('click', function () {
                                    var link = document.createElement('a');

                                    link.href = 'data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,' + response.testData;
                                    link.download = response.testCase.name + '.xlsx';
                                    link.click();
                                }
                                );

                            var workbook = XLSX.read(atob(response.testData), {
                                type: 'binary'
                            });

                            try {
                                workbook.SheetNames.forEach(function (sheetName) {
                                    var excelJson = XLSX.utils.sheet_to_json(workbook.Sheets[sheetName]);

                                    var headers = Object.keys(excelJson[0]);
                                    var headerRow = '<tr>';

                                    $.each(headers, function (i, header) {
                                        headerRow += '<th>' + header + '</th>';
                                    });

                                    headerRow += '</tr>';

                                    var table = '<table id="testDataDetails" class="table table-sm table-bordered table-striped" cellspacing="0" width="100%">' + headerRow + '</table>';

                                    $('#testDataCard').html(table);

                                    var records = '';

                                    $.each(excelJson, function (i, row) {
                                        records += '<tr>';

                                        $.each(headers, function (j, header) {
                                            records += '<td>' + row[header] + '</td>';
                                        });

                                        records += '</tr>';
                                    });

                                    $('#testDataDetails').append(records);
                                    $('#testDataDiv').show();
                                });
                            }
                            catch (error) {
                                console.error(error);
                            }
                        }

                        document.getElementById('defectStatusTd').classList.remove('bg-rain');
                        document.getElementById('defectStatusTd').classList.remove('bg-sky');
                        document.getElementById('defectStatusTd').classList.remove('bg-grape');
                        document.getElementById('defectStatusTd').classList.remove('bg-mammee');
                        document.getElementById('defectStatusTd').classList.remove('bg-mist');
                        document.getElementById('defectStatusTd').classList.remove('bg-rain');
                        document.getElementById('defectStatusTd').classList.remove('bg-eggplant');
                        document.getElementById('defectStatusTd').classList.remove('bg-grass');
                        document.getElementById('defectStatusTd').classList.remove('bg-olive');
                        document.getElementById('defectStatusTd').classList.remove('bg-pineapple');
                        document.getElementById('defectStatusTd').classList.remove('bg-storm');

                        if (response.defect != null) {
                            switch (response.defect.defectStatus) {
                                case 1:
                                    document.getElementById('defectStatusTd').classList.add('bg-rain');
                                    break;

                                case 2:
                                    document.getElementById('defectStatusTd').classList.add('bg-sky');
                                    break;

                                case 3:
                                    document.getElementById('defectStatusTd').classList.add('bg-grape');
                                    break;

                                case 4:
                                    document.getElementById('defectStatusTd').classList.add('bg-mammee');
                                    break;

                                case 5:
                                    document.getElementById('defectStatusTd').classList.add('bg-mist');
                                    break;

                                case 6:
                                    document.getElementById('defectStatusTd').classList.add('bg-rain');
                                    break;

                                case 7:
                                    document.getElementById('defectStatusTd').classList.add('bg-eggplant');
                                    break;

                                case 8:
                                    document.getElementById('defectStatusTd').classList.add('bg-grass');
                                    break;

                                case 9:
                                    document.getElementById('defectStatusTd').classList.add('bg-olive');
                                    break;

                                case 10:
                                    document.getElementById('defectStatusTd').classList.add('bg-pineapple');
                                    break;

                                case 11:
                                    document.getElementById('defectStatusTd').classList.add('bg-storm');
                                    break;
                            }

                            document.getElementById('assignee').innerText = response.defect.user.displayName;
                            document.getElementById('assigneeEmail').innerText = '';
                            document.getElementById('assigneePhoneNumber').innerText = '';
                            document.getElementById('summary').innerText = response.defect.summary;
                            document.getElementById('defectDescription').innerText = response.defect.description;
                            document.getElementById('defectPriority').innerText = find(defectPriorityCatalogue, parseFloat(response.defect.defectPriority)).text;
                            document.getElementById('defectSeverity').innerText = find(defectSeverityCatalogue, parseFloat(response.defect.defectSeverity)).text;

                            if (response.defect.defectStatus == 8) {
                                document.getElementById('defectStatus').innerHTML = '<span>' + find(defectStatusCatalogue, parseFloat(response.defect.defectStatus)).text + '</span> <i class="fa fa-triangle-exclamation" style="text-align:right" data-toggle="tooltip" data-placement="right" title="The defect of this test case has been fixed, now it can be retested."></i>';
                            }
                            else {
                                document.getElementById('defectStatus').innerText = find(defectStatusCatalogue, parseFloat(response.defect.defectStatus)).text;
                            }

                            $('#defectDiv').show();
                        }

                        document.getElementById('testApplicationName').innerText = response.testCase.businessProcess.testApplication.name;
                        document.getElementById('testApplicationDescription').innerText = response.testCase.businessProcess.testApplication.description;
                        document.getElementById('businessProcessName').innerText = response.testCase.businessProcess.name;
                        document.getElementById('businessProcessDescription').innerText = response.testCase.businessProcess.description;
                        document.getElementById('testCaseName').innerText = response.testCase.name;
                        document.getElementById('testCaseDescription').innerText = response.testCase.description;
                        document.getElementById('testCaseTestType').innerText = find(testTypeCatalogue, parseFloat(response.testCase.testType)).text;
                        document.getElementById('testCasePreconditions').innerText = response.testCase.preconditions;
                        document.getElementById('executionDate').innerText = new Date(response.executionDate).toLocaleString([], { hour12: true });
                        document.getElementById('executionTestEnvironment').innerText = find(testEnvironmentCatalogue, parseFloat(response.testEnvironment)).text;
                        document.getElementById('executionBrowserDriver').innerText = find(browserDriverCatalogue, parseFloat(response.browserDriver)).text;

                        if (response.executionStatus == 1) {
                            if (response.defect != null) {
                                document.getElementById('executionStatus').innerHTML = '<span>' + find(executionStatusCatalogue, parseFloat(response.executionStatus)).text + '</span> <i class="fa fa-triangle-exclamation" style="text-align:right" data-toggle="tooltip" data-placement="right" title="This test case has a defect assigned, it is necessary to execute it again to get the final execution status."></i>';
                            }
                        }
                        else {
                            document.getElementById('executionStatus').innerText = find(executionStatusCatalogue, parseFloat(response.executionStatus)).text;
                        }
                        document.getElementById('executionRemarks').innerText = response.remarks;
                        document.getElementById('executor').innerText = response.user.displayName;
                        document.getElementById('company').innerText = response.user.company.name;
                        document.getElementById('role').innerText = response.user.userRoles[0].role.name;
                        document.getElementById('email').innerText = '';
                        document.getElementById('phoneNumber').innerText = '';
                        document.getElementById('testReportFrame').srcdoc = atob(response.testReport);

                        $('#detailsModal').modal('show');
                        customSwal.close();
                    },
                    error: function () {
                        table.ajax.reload(null, false);
                        customSwal.fire('Incorrect', 'No execution info found!', 'error');
                    }
                });
            }, 2000);
        }
    });
}