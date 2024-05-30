$(document).ready(function () {
    // Ensure that the table element exists
    if ($('#productTable').length) {
        $('#productTable').DataTable({
            "ajax": {
                url: '/Admin/Product/GetProducts',
                dataSrc: 'data',
                error: function (xhr, error, thrown) {
                    console.error("AJAX error response:", xhr.responseText);
                    console.error("Error details:", error, thrown);
                    alert("An error occurred while fetching data. Please check the console for more details.");
                }
            },
            "columns": [
                { data: 'title' },
                { data: 'isbn' },
                { data: 'price' },
                { data: 'author' },
                { data: 'category.name' },
                {
                    data: 'id',
                    render: function (data) {
                        return `
                        <div class="d-flex justify-content-center">
                            <a class="btn btn-secondary" href="/Admin/Product/Upsert/${data}">
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pencil" viewBox="0 0 16 16">
                                    <path d="M12.146.146a.5.5 0 0 1 .708 0l3 3a.5.5 0 0 1 0 .708l-10 10a.5.5 0 0 1-.168.11l-5 2a.5.5 0 0 1-.65-.65l2-5a.5.5 0 0 1 .11-.168zM11.207 2.5 13.5 4.793 14.793 3.5 12.5 1.207zm1.586 3L10.5 3.207 4 9.707V10h.5a.5.5 0 0 1 .5.5v.5h.5a.5.5 0 0 1 .5.5v.5h.293zm-9.761 5.175-.106.106-1.528 3.821 3.821-1.528.106-.106A2.517 2.517 0 0 0 6 14.5H5v-.5a.5.5 0 0 0-.5-.5H4v-.5a.5.5 0 0 0-.5-.5H3v-.293a2.517 2.517 0 0 0-.968-.532z"/>
                                </svg>
                            </a>
                            &nbsp;
                            <a class="btn btn-danger" href="/Admin/Product/Delete/${data}">
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                    <path d="M5.5 5.5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zm3 0a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM4.118 1.58A.5.5 0 0 1 4.5 1h7a.5.5 0 0 1 .382.18l1 1a.5.5 0 0 1-.764.64l-.641-.647H4.118l-.641.646a.5.5 0 0 1-.764-.639l1-1zM4.5 2.5v-.5H11v.5a.5.5 0 0 1-.5.5H5a.5.5 0 0 1-.5-.5zM2.118 4.58A.5.5 0 0 1 2.5 4h11a.5.5 0 0 1 .382.18l1 1a.5.5 0 0 1-.764.64l-.641-.647H2.118l-.641.646a.5.5 0 0 1-.764-.639l1-1zM2 4.5a.5.5 0 0 1 .5.5v7a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2v-7a.5.5 0 0 1 1 0v7a3 3 0 0 1-3 3H4a3 3 0 0 1-3-3v-7a.5.5 0 0 1 .5-.5z"/>
                                </svg>
                            </a>
                        </div>`;
                    }
                }
            ],
            "order": [[0, 'asc']],
            "initComplete": function (settings, json) {
                console.log("DataTable initialized", json);
            }
        });
    } else {
        console.error("Table element with id 'productTable' not found.");
    }
});
