var chart = null;

// POST: Reports/Create
$('#create').on('submit', function () {
    var form = $(this);

    if (form[0].checkValidity()) {
        var reportTitle = this.reportTitle.value;
        var startDate = this.startDate.value;
        var endDate = this.endDate.value;

        customSwal.fire({
            title: 'Generating report...',
            html: '<div style="min-height:150px"><div class="spinner"></div></div>',
            allowEscapeKey: false,
            allowOutsideClick: false,
            showConfirmButton: false,
            showCancelButton: false,
            showCloseButton: false,
            didOpen: () => {
                setTimeout(function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Reports/Create',
                        data: $('#create').serialize(),
                        dataType: 'JSON',
                        success: function (response) {
                            if (chart != null) {
                                chart.destroy();
                            }

                            var passExecutions = 0;
                            var failExecutions = 0;
                            var stopExecutions = 0;
                            var notRunExecutions = 0;

                            response.forEach(element => {
                                element.forEach(child => {
                                    switch (child.executionStatus) {
                                        case 1:
                                            notRunExecutions += 1;
                                            break;

                                        case 2:
                                            stopExecutions += 1;
                                            break;

                                        case 3:
                                            failExecutions += 1;
                                            break;

                                        case 4:
                                            passExecutions += 1;
                                            break;
                                    }
                                });
                            });

                            var totalExecutions = passExecutions + failExecutions + stopExecutions + notRunExecutions;
                            var ctx = document.getElementById('chart').getContext('2d');

                            chart = new Chart(ctx, {
                                type: 'doughnut',
                                data: {
                                    labels: ['Pass', 'Fail', 'Stop', 'Not run'],
                                    datasets: [{
                                        label: 'Executions',
                                        backgroundColor: ['rgba(95, 167, 88, 0.6)', 'rgba(215, 75, 61, 0.6)', 'rgba(255, 227, 81, 0.6)', 'rgba(114, 165, 217, 0.6)'],
                                        borderColor: ['rgba(95, 167, 88, 0.6)', 'rgba(215, 75, 61, 0.6)', 'rgba(255, 227, 81, 0.6)', 'rgba(114, 165, 217, 0.6)'],
                                        data: [passExecutions, failExecutions, stopExecutions, notRunExecutions],
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
                                            text: reportTitle
                                        }
                                    }
                                }
                            });

                            chart.canvas.parentNode.style.height = '400px';
                            chart.canvas.parentNode.style.width = '400px';

                            if (totalExecutions > 0) {
                                if (document.getElementById('downloadReportButton')) {
                                    document.getElementById('downloadReportButton').remove();
                                }
                                $('<a class="btn btn-kiwi" id="downloadReportButton" href="/Reports/Download?startDate=' + startDate + '&endDate=' + endDate + '&reportTitle=' + reportTitle + '"><i class="fa fa-download"></i> Download report</a>')
                                    .appendTo(document.getElementById('downloadReport'));

                                $('#footerDiv').show();
                            }
                            else {
                                $('#footerDiv').hide();
                            }

                            customSwal.close();
                        },
                        error: function () {
                            customSwal.fire('Incorrect', 'An unexpected error occurred!', 'error');
                        }
                    });
                }, 2000);
            }
        });
    }

    return false;
});