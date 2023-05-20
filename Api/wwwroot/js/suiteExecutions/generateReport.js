var executionsTable;
var chart = null;

// GET: SuiteExecutions/Executions/Get/5
function GenerateReport(id) {
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
                        if (chart != null) {
                            chart.destroy();
                        }

                        var ctx = document.getElementById('chart').getContext('2d');

                        chart = new Chart(ctx, {
                            type: 'doughnut',
                            data: {
                                labels: ['Pass', 'Fail', 'Stop', 'Not run'],
                                datasets: [{
                                    label: 'Executions',
                                    backgroundColor: ['rgba(95, 167, 88, 0.6)', 'rgba(215, 75, 61, 0.6)', 'rgba(255, 227, 81, 0.6)', 'rgba(114, 165, 217, 0.6)'],
                                    borderColor: ['rgba(95, 167, 88, 0.6)', 'rgba(215, 75, 61, 0.6)', 'rgba(255, 227, 81, 0.6)', 'rgba(114, 165, 217, 0.6)'],
                                    data: [response.passExecutions, response.failExecutions, response.stopExecutions, response.notRunExecutions],
                                    borderWidth: 2
                                }]
                            },
                            options: {
                                animation: {
                                    duration: 2000,
                                },
                                responsive: true,
                                plugins: {
                                    legend: {
                                        position: 'top'
                                    },
                                    title: {
                                        display: true,
                                        text: response.businessProcess.name
                                    }
                                }
                            }
                        });

                        chart.canvas.parentNode.style.height = '400px';
                        chart.canvas.parentNode.style.width = '400px';

                        executionsTable = $('#executionsTable').DataTable({
                            processing: true,
                            data: response.executions,
                            buttons: [
                                {
                                    extend: 'excelHtml5',
                                    text: '<i class="fas fa-file-excel"></i>',
                                    titleAttr: 'Export Excel',
                                    className: 'btn btn-excel',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4, 5, 6]
                                    },
                                    sheetName: response.businessProcess.name
                                },
                                {
                                    extend: 'pdf',
                                    text: '<i class="fas fa-file-pdf"></i>',
                                    titleAttr: 'Export PDF',
                                    className: 'btn btn-pdf',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4, 5, 6]
                                    },
                                    customize: function (pdf) {
                                        pdf.content[1].margin = [80, 0, 80, 0]
                                    }
                                },
                                {
                                    extend: 'print',
                                    text: '<i class="fas fa-print"></i>',
                                    titleAttr: 'Print',
                                    className: 'btn btn-print',
                                    exportOptions: {
                                        columns: [0, 1, 2, 3, 4, 5, 6]
                                    }
                                },
                                {
                                    extend: 'colvis',
                                    text: '<i class="fas fa-eye"></i>',
                                    titleAttr: 'Show/Hide columns',
                                    className: 'btn btn-colvis rounded-0',
                                    columns: [1, 2, 3, 4, 5, 6]
                                },
                                {
                                    extend: 'pageLength',
                                    text: '<i class="fas fa-list-alt"></i>',
                                    titleAttr: 'Show/Hide Records',
                                    className: 'btn btn-rowvis',
                                },
                            ],
                            columns: [
                                {
                                    className: 'text-center',
                                    defaultContent: '',
                                    searchable: false,
                                    orderable: false,
                                    autoWidth: true,
                                    width: '30px'
                                },
                                {
                                    data: 'testCase',
                                    name: 'testCase',
                                    autoWidth: true,
                                },
                                {
                                    data: 'executionDate',
                                    name: 'executionDate',
                                    render: function (data, type, row) {
                                        return new Date(row.executionDate).toLocaleString([], { hour12: true });
                                    },
                                    autoWidth: true,
                                },
                                {
                                    data: 'testEnvironment',
                                    name: 'testEnvironment',
                                    render: function (data, type, row) {
                                        return find(testEnvironmentCatalogue, parseFloat(row.testEnvironment)).text;
                                    },
                                    autoWidth: true,
                                },
                                {
                                    data: 'executor',
                                    name: 'executor',
                                    autoWidth: true,
                                },
                                {
                                    data: 'company',
                                    name: 'company',
                                    autoWidth: true,
                                },
                                {
                                    data: 'role',
                                    name: 'role',
                                    autoWidth: true,
                                },
                                {
                                    data: 'executionStatus',
                                    name: 'executionStatus',
                                    render: function (data, type, row) {
                                        return find(executionStatusCatalogue, parseFloat(row.executionStatus)).text;
                                    },
                                    autoWidth: true,
                                },
                                {
                                    render: function (data, type, row) {
                                        return '<div class="btn-group w-100">'
                                            + '<button class="btn btn-kiwi" onclick="Download(\'' + row.executionId + '\')" title="Download individual report"><i class="fa fa-download"></i></button>'
                                            + '</div>'
                                    },
                                    orderable: false,
                                    searchable: false,
                                    autoWidth: true,
                                    width: '4%'
                                }
                            ],
                            lengthMenu: [
                                [10, 25, 50, 100, 500, 1000],
                                ['10 records', '25 records', '50 records', '100 records', '500 records', '1000 records']
                            ],
                            order: [],
                            lengthChange: false,
                            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6"f>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-7"i><"col-sm-12 col-md-5"p>>',
                            scrollY: window.innerHeight - 365,
                            scrollX: true,
                            scrollCollapse: true,
                            paging: true,
                            retrieve: true,
                            language: {
                                url: '../lib/datatables/json/English.json',
                            },
                            initComplete: function () {
                                this.api().columns().every(function (colIdx) {
                                    var that = this;

                                    $('input', this.footer()).on('keyup change clear search', function () {
                                        if (that.search() !== this.value) {
                                            that.search(this.value).draw();
                                        }
                                    });

                                    if (colIdx == 2) {
                                        var div = $('<div class="input-group"></div>').appendTo($(that.footer()).empty());
                                        var dateInput = $('<input type="date" class="form-control form-control-sm" disabled />').appendTo(div).on('change', function () {
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });

                                        $('<button class="btn btn-sm btn-caution" type="reset" disabled><i class="fa fa-times"></i></button>').appendTo(div).on('click', function () {
                                            dateInput.val('');
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });
                                    }

                                    if (colIdx == 3) {
                                        var div = $('<div class="input-group"></div>').appendTo($(that.footer()).empty());
                                        var select = $('<select class="form-control form-control-sm"><option value="" disabled selected>Select environment...</option></select>').appendTo(div).on('change', function () {
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });

                                        that.data().unique().sort().each(function (d, j) {
                                            var testEnvironment = find(testEnvironmentCatalogue, parseFloat(d));

                                            select.append('<option value="' + testEnvironment.text + '">' + testEnvironment.text + '</option>');
                                        });

                                        $('<button class="btn btn-sm btn-caution" type="reset"><i class="fa fa-times"></i></button>').appendTo(div).on('click', function () {
                                            select.val('');
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });
                                    }

                                    if (colIdx == 7) {
                                        var div = $('<div class="input-group"></div>').appendTo($(that.footer()).empty());
                                        var select = $('<select class="form-control form-control-sm"><option value="" disabled selected>Select status...</option></select>').appendTo(div).on('change', function () {
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });

                                        that.data().unique().sort().each(function (d, j) {
                                            var executionStatus = find(executionStatusCatalogue, parseFloat(d));

                                            select.append('<option value="' + executionStatus.text + '">' + executionStatus.text + '</option>');
                                        });

                                        $('<button class="btn btn-sm btn-caution" type="reset"><i class="fa fa-times"></i></button>').appendTo(div).on('click', function () {
                                            select.val('');
                                            if (that.search() !== this.value) {
                                                that.search(this.value).draw();
                                            }
                                        });
                                    }
                                });

                                executionsTable.on('order.dt search.dt', function () {
                                    var i = 1;

                                    executionsTable.cells(null, 0, { search: 'applied', order: 'applied', page: 'applied' }).every(function (cell) {
                                        this.data(i++);
                                        executionsTable.cell(cell).invalidate('dom');
                                    });
                                }).draw();
                            }
                        });

                        $('#reportModal').modal('show');
                        customSwal.close();
                    },
                    error: function () {
                        executionsTable.ajax.reload(null, false);
                        customSwal.fire('Incorrect', 'No test suite execution info found!', 'error');
                    }
                });

                $('#executionsTable tfoot th').each(function (colIdx) {
                    var title = $(this).text().toLowerCase();

                    if (colIdx != 0 && colIdx != 2 && colIdx != 3 && colIdx != 7 && colIdx != 8) {
                        $(this).html('<input type="search" class="form-control form-control-sm" placeholder="Filter by ' + title + '..." />');
                    }
                });
            }, 2000);
        }
    });
}

$('#reportModal').on('shown.bs.modal', function () {
    executionsTable.columns.adjust().responsive.recalc();
});