$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#productsTable").DataTable({
        "ajax": { url: '/admin/products/getall' },
        "columns": [
            { data: 'title', "width": "15%" },
            { data: 'description', "width": "15%" },
            { data: 'categoryName', "width": "15%" },
            { data: 'isbn', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'price', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return '<div clas="w-75 btn-group" role="group"> ' +
                        '<a href="products/edit?productId=' + data + '" class="btn btn-dark mb-2">' +
                            '<i class="bi bi-pencil-square" ></i>' +
                        '</a>' +
                        '<a href="products/delete?productId=' + data + '" class="btn btn-danger mb-2">' +
                            '<i class="bi bi-trash"></i>' +
                        '</a>' +
                    '</div >'
                },
                "width": "25%"
            }
        ]
    });
}