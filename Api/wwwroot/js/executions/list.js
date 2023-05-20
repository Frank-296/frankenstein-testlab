var table;

// GET: Executions
function List(title, testCaseId) {
    table = $('#table').DataTable({
        processing: true,
        serverSide: true,
        ajax: {
            type: 'POST',
            url: '/Executions/List/' + testCaseId,
            dataType: 'JSON',
            data: function (data) {
                data.tableLength = table.columns(':visible').count();
            }
        },
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="fas fa-file-excel"></i>',
                titleAttr: 'Export Excel',
                className: 'btn btn-excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                sheetName: title,
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
                data: 'executionId', render: function (executionId) {
                    return '<div class="btn-group w-100">'
                        + '<button class="btn btn-sky" onclick="Get(\'' + executionId + '\')" title="Details"><i class="fa fa-circle-info"></i></button>'
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
                    var dateInput = $('<input type="date" class="form-control form-control-sm" />').appendTo(div).on('change', function () {
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });

                    $('<button class="btn btn-sm btn-caution" type="reset"><i class="fa fa-times"></i></button>').appendTo(div).on('click', function () {
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

                        select.append('<option value="' + testEnvironment.id + '">' + testEnvironment.text + '</option>');
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

                        select.append('<option value="' + executionStatus.id + '">' + executionStatus.text + '</option>');
                    });

                    $('<button class="btn btn-sm btn-caution" type="reset"><i class="fa fa-times"></i></button>').appendTo(div).on('click', function () {
                        select.val('');
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });
                }
            });
        }
    });

    $('#table tfoot th').each(function (colIdx) {
        var title = $(this).text().toLowerCase();

        if (colIdx != 0 && colIdx != 2 && colIdx != 3 && colIdx != 7 && colIdx != 8) {
            $(this).html('<input type="search" class="form-control form-control-sm" placeholder="Filter by ' + title + '..." />');
        }
    });

    table.on('draw.dt', function () {
        var info = table.page.info();

        table.column(0, { search: 'applied', order: 'applied', page: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1 + info.start;
            table.cell(cell).invalidate('dom');
        });
    }).draw();
}