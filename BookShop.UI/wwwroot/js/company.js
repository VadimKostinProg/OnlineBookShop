$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#companiesTable").DataTable({
        "ajax": { url: '/admin/companies/getall' },
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "15%" },
            { data: 'region', "width": "15%" },
            { data: 'postalCode', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return '<div clas="w-75 btn-group" role="group"> ' +
                        '<a href="companies/edit?companyId=' + data + '" class="btn btn-dark mb-2">' +
                            '<i class="bi bi-pencil-square" ></i>' +
                        '</a>' +
                        '<a href="companies/delete?companyId=' + data + '" class="btn btn-danger mb-2">' +
                            '<i class="bi bi-trash"></i>' +
                        '</a>' +
                    '</div >'
                },
                "width": "25%"
            }
        ]
    });
}