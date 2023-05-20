// GET: Executions/Get/5
function Download(id) {
    customSwal.fire({
        title: 'Downloading...',
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
                        var link = document.createElement('a');

                        link.href = 'data:text/html;base64,' + response.testReport;
                        link.download = 'index.html';
                        link.click();

                        customSwal.close();
                    }
                });
            }, 2000);
        }
    });
}